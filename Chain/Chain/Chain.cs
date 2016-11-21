using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Chain
{
    public class Event
    {
        public override string ToString()
        {
            var sb = new StringBuilder();
            var fields = GetType().GetFields();

            sb.Append(GetType().Name);
            foreach (var field in fields)
            {
                sb.Append($"  - {field.Name} = {field.GetValue(this)} \r\n");
            }

            return sb.ToString();
        }
    }

    public interface IEventPublisher
    {
        int DefaultPollingInterval { get; }

        Task<IEnumerable<Event>> GetEvents();
    }
    
    public interface IEventFilter
    {
    }

    public class PublisherInstance
    {
        public IEventPublisher Instance { get; }

        private List<Func<object, bool>> Filters
            = new List<Func<object, bool>>();
        private List<ChainTask> Subscribers
            = new List<ChainTask>();

        public PublisherInstance(IEventPublisher instance)
        {
            Instance = instance;
        }

        public PublisherInstance Filter<T>(params object[] args)
            where T : EventFilter
        {
            Filters.Add((x) => {
                var filter = (EventFilter)Activator
                    .CreateInstance(typeof(T), args);

                return filter.OnExecute((Event)x);
            });

            return this;
        }
        public PublisherInstance Filter<T>(Func<T, bool> expr)
            where T : Event
        {
            Filters.Add((x) => {
                if (x is T)
                    return expr((T)x);
                return false;
                });

            return this;
        }
        public ChainTask Task<T>(params object[] args)
            where T : ChainTask
        {
            var task = (ChainTask)Activator.CreateInstance(typeof(T), args);
            Subscribers.Add(task);
            return task;
        }

        internal void InvokeSubscribers(IEnumerable<Event> events)
        {
            if (events.Count() == 0)
                return;

            foreach (var ev in events)
            {
                var ok = Filters.Count == 0 ? true : false;
                foreach(var filter in Filters)
                {
                    if (filter.Invoke(ev))
                    {
                        ok = true;
                        break;
                    }
                }

                // FILTERED
                if (ok == false)
                    return;
            }

            foreach (var subscriber in Subscribers)
            {
                Console.WriteLine(subscriber);
                var ctx = new TaskContext();

                foreach (var ev in events)
                    ctx.Set(ev);

                subscriber.Context = ctx;
                subscriber.Execute();

                ctx.Dispose();
            }
        }
    }

    public class C
    {
        private static List<PublisherInstance> publishers 
            = new List<PublisherInstance>();

        public static PublisherInstance EventSource<T>(params object[] args)
            where T : IEventPublisher
        {
            var publisher = new PublisherInstance(
                (IEventPublisher)Activator.CreateInstance(typeof(T), args));
            publishers.Add(publisher);

            return publisher;
        }

        public static void Run()
        {
            Console.WriteLine("[RUN]");

            if (Directory.Exists("./workspace") == false)
                Directory.CreateDirectory("./workspace");

            var children = new List<Thread>();

            foreach(var publisher in publishers)
            {
                var child = new Thread(() =>
                {
                    while (true)
                    {
                        Console.WriteLine("[POLL] " + publisher.Instance);

                        var events = publisher.Instance.GetEvents().Result;

                        publisher.InvokeSubscribers(events);

                        Thread.Sleep(publisher.Instance.DefaultPollingInterval);
                    }
                });
                child.Start();

                children.Add(child);
            }

            foreach (var child in children)
                child.Join();
        }
    }
}

/*
 * 
 * Chain.AddEventSource<Github.CommitEventPublisher>()
 *      .Subscribe((event) => {
 *          
 *      })
 *      .ContinueWith((event) => {
 *          Subscribe(
 *      });
 * */