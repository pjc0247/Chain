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
        public string OutKey;
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
            where TCRED : IServiceCredential
        {
            CredentialOverrides[typeof(TCRED)] = key;

            return this;
        }
        public ChainTask In(string fieldKey, string outKey)
        {
            if (GetInField(fieldKey) == null)
                throw new ArgumentException($"Field({fieldKey}) not exists in Task({GetType()}).");

            InBindings.Add(new InBinding() {
                FieldKey = fieldKey,
                OutKey = outKey
            });
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

        private void WrappedExecute()
        {
            Context.Reset();

            foreach (var inBinding in InBindings)
            {
                var field = GetInField(inBinding.FieldKey);
                field.SetValue(this, Context.In(inBinding.OutKey));
            }
            foreach (var ov in CredentialOverrides)
            {
                Context.CredentialProvider.SetKeyOnce(ov.Key, ov.Value);
            }

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
