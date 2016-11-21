using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Chain
{
    public class TaskContext : IDisposable
    {
        private List<Event> Events = new List<Event>();
        private List<Thread> AsyncThreads = new List<Thread>();

        private Dictionary<string, object> Storage
            = new Dictionary<string, object>();

        public CredentialsProvider CredentialProvider;
        public string WorkingDirectory;

        public TaskContext()
        {
            WorkingDirectory = $"./workspace/{Path.GetRandomFileName()}";
            Directory.CreateDirectory(WorkingDirectory);

            CredentialProvider = new CredentialsProvider();
        }
        public void Dispose()
        {
            JoinAndClearAsyncThreads(int.MaxValue);

            Directory.Delete(WorkingDirectory, true);
        }

        public void Reset()
        {
            CredentialProvider.Reset();
        }

        public object Get(Type eventType)
        {
            return Events.FirstOrDefault(x => x.GetType() == eventType || x.GetType().IsSubclassOf(eventType));
        }
        public T Get<T>()
            where T : Event
        {
            return (T)Events.FirstOrDefault(x => x is T);
        }
        public IEnumerable<T> GetAll<T>()
            where T : Event
        {
            return Events.Where(x => x is T).Select(x => (T)x);
        }

        public void Set<T>(T e)
            where T : Event
        {
            Events.Add(e);
        }
        
        internal void AddAsyncThread(Thread t)
        {
            AsyncThreads.Add(t);
        }
        internal void JoinAndClearAsyncThreads(int msTimeout)
        {
            foreach (var thread in AsyncThreads)
                thread.Join();

            AsyncThreads.Clear();
        }

        internal object In(string key)
        {
            if (Storage.ContainsKey(key) == false)
                throw new KeyNotFoundException($"{key}");
            return Storage[key];
        }
        public void Out(string key, object value)
        {
            Storage[key] = value;
        }
    }
}
