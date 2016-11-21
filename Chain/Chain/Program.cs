using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chain
{
    public class Event
    {
    }

    public interface IEventPublisher
    {
        int DefaultPollingInterval { get; }

        Task<IEnumerable<Event>> GetEvents();
    }
    
    public interface IEventFilter
    {
    }

    public class SubscriberInstance
    {
        public SubscriberInstance WithFilter<T>()
        {
            return this;
        }
    }
    public class PublisherInstance
    {
        public IEventPublisher Instance { get; }

        private List<ChainTask> Subscribers
            = new List<ChainTask>();

        public PublisherInstance(IEventPublisher instance)
        {
            Instance = instance;
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

            foreach (var subscriber in Subscribers)
            {
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

        public static PublisherInstance AddEventSource<T>(params object[] args)
            where T : IEventPublisher
        {
            var publisher = new PublisherInstance(
                (IEventPublisher)Activator.CreateInstance(typeof(T), args));
            publishers.Add(publisher);

            return publisher;
        }

        public static SubscriberInstance Subscribe<T>()
        {
            var sub = new SubscriberInstance();

            return sub;
        }

        public static void Run()
        {
            Console.WriteLine("[RUN]");

            while (true)
            {
                Console.WriteLine("Poll");

                foreach (var publisher in publishers)
                {
                    var events = publisher.Instance.GetEvents().Result;

                    publisher.InvokeSubscribers(events);
                }

                Thread.Sleep(1000);
            }
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