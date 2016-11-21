using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Chain
{
    class InBinding
    {
        public string FieldKey;

        public Func<TaskContext, object> Generator;

        public static InBinding FromConstantValue(string fieldKey, object value)
        {
            var ib = new InBinding();
            ib.FieldKey = fieldKey;
            ib.Generator = (ctx) => { return value; };
            return ib;
        }
        public static InBinding FromOutKey(string fieldKey, string outKey)
        {
            var ib = new InBinding();
            ib.FieldKey = fieldKey;
            ib.Generator = (ctx) => { return ctx.In(outKey); };
            return ib;
        }
        public static InBinding FromTemplate<TEMPLATE>(string fieldKey, params object[] args)
            where TEMPLATE : IMessageTemplate
        {
            var ib = new InBinding();
            var template = (TEMPLATE)Activator.CreateInstance(typeof(TEMPLATE), args);

            ib.FieldKey = fieldKey;
            ib.Generator = (ctx) => { return template.OnExecute(ctx); };
            return ib;
        }
    }
    class OutBinding
    {
        public string FieldKey;
        public string OutKey;
    }

    public class ChainTask
    {
        private List<ChainTask> ChildTasks = new List<ChainTask>();

        private List<InBinding> InBindings = new List<InBinding>();
        private List<OutBinding> OutBindings = new List<OutBinding>();

        private Dictionary<Type, string> CredentialOverrides
            = new Dictionary<Type, string>();

        private bool RunAsync = false;
        private bool IsIgnoreException = false;

        internal protected TaskContext Context;

        public static void Execute<T>(TaskContext context, params object[] args)
        {
            var task = (ChainTask)Activator.CreateInstance(typeof(T), args);
            task.Context = context;
            task.Execute();
        }
        public static void ExecuteAsync<T>(TaskContext context, params object[] args)
        {
            var task = (ChainTask)Activator.CreateInstance(typeof(T), args);
            task.Context = context;
            task.Async().Execute();
        }

        public ChainTask Credential<TCRED>(string key)
            where TCRED : IServiceCredentials
        {
            CredentialOverrides[typeof(TCRED)] = key;

            return this;
        }

        public ChainTask Set(string fieldKey, object value)
        {
            if (GetInField(fieldKey) == null)
                throw new ArgumentException($"Field({fieldKey}) not exists in Task({GetType()}).");

            InBindings.Add(InBinding.FromConstantValue(fieldKey, value));
            return this;
        }
        public ChainTask In(string fieldKey, string outKey)
        {
            if (GetInField(fieldKey) == null)
                throw new ArgumentException($"Field({fieldKey}) not exists in Task({GetType()}).");

            InBindings.Add(InBinding.FromOutKey(fieldKey, outKey));
            return this;
        }
        public ChainTask In<TEMPLATE>(string fieldKey, params object[] args)
            where TEMPLATE : IMessageTemplate
        {
            if (GetInField(fieldKey) == null)
                throw new ArgumentException($"Field({fieldKey}) not exists in Task({GetType()}).");

            InBindings.Add(InBinding.FromTemplate<TEMPLATE>(fieldKey, args));
            return this;
        }
        public ChainTask Out(string fieldKey)
        {
            return Out(fieldKey, fieldKey);
        }
        public ChainTask Out(string fieldKey, string outKey)
        {
            OutBindings.Add(new OutBinding()
            {
                FieldKey = fieldKey,
                OutKey = outKey
            });
            return this;
        }

        public ChainTask IgnoreException()
        {
            IsIgnoreException = true;
            return this;
        }
        public ChainTask Async()
        {
            RunAsync = true;
            return this;
        }
        public ChainTask Task<T>(params object[] args)
            where T : ChainTask
        {
            var task = (ChainTask)Activator.CreateInstance(typeof(T), args);
            ChildTasks.Add(task);
            return task;
        }

        protected void Require<T>()
            where T : Event
        {
            if (Context.Get<T>() == null)
                throw new InvalidOperationException($"Require::Not found event - {typeof(T)}");
        }
        protected void Defer(Action callback)
        {
        }

        internal void Execute()
        {
            Console.WriteLine($"[ {GetType().Name} ]");
            
            if (RunAsync)
            {
                var thread = new Thread(() =>
                {
                    WrappedExecute();
                });
                thread.Start();
                Context.AddAsyncThread(thread);
            }
            else
                WrappedExecute();

            foreach(var child in ChildTasks)
            {
                child.Context = Context;
                child.Execute();
            }
        }

        private void ProcessEv2Params()
        {
            var cvts = GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                .Select(x => new
                {
                    Method = x,
                    Attr = (Ev2ParamAttribute)x.GetCustomAttribute(typeof(Ev2ParamAttribute), true)
                })
                .Where(x => x.Attr != null);

            foreach (var cvt in cvts)
            {
                var ev = Context.Get(cvt.Attr.EventType);
                if (ev != null)
                    cvt.Method.Invoke(this, new object[] { ev });
            }
        }

        private void WrappedExecute()
        {
            Context.Reset();

            foreach (var inBinding in InBindings)
            {
                var field = GetInField(inBinding.FieldKey);
                field.SetValue(this, inBinding.Generator(Context));
            }
            foreach (var ov in CredentialOverrides)
            {
                Context.CredentialProvider.SetKeyOnce(ov.Key, ov.Value);
            }

            ProcessEv2Params();

            try
            {
                OnExecute();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                if (IsIgnoreException == false)
                    throw e;
            }
        }

        public virtual void OnExecute()
        {
            // Override me
        }

        private FieldInfo GetInField(string fieldKey)
        {
            return GetType().GetField(fieldKey, BindingFlags.NonPublic | BindingFlags.Instance);
        }
    }
}
