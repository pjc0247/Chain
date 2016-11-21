using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

using Chain;

using Newtonsoft.Json;

namespace export_tasks
{
    class IoInfo
    {
        public string Name;
        public string Path;
        public Type Type;
    }
    class TaskInfo
    {
        public string Name;
        public IoInfo[] Inputs;
        public IoInfo[] Outputs;
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var dlls = Directory.EnumerateFiles(".", "*.dll");

            foreach(var dll in dlls)
            {
                Assembly assembly;
                IEnumerable<Type> tasks;

                try
                {
                    // FULL LOAD
                    assembly = Assembly.LoadFrom(dll);
                    tasks = assembly.GetExportedTypes()
                        .Where(x => x.IsSubclassOf(typeof(ChainTask)));
                }
                catch(Exception e)
                {
                    continue;
                }
                
                foreach(var task in tasks)
                {
                    var taskInfo = new TaskInfo();
                    var inputs = new List<IoInfo>();
                    var outputs = new List<IoInfo>();

                    var privateFields = task.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                    var inKeys = task.GetFields(BindingFlags.Public | BindingFlags.Static)
                        .Where(x => x.FieldType == typeof(string))
                        .Where(x => x.Name.StartsWith("IN_"))
                        .Select(x => x.Name);

                    foreach(var field in privateFields)
                    {
                        if (inKeys.Contains("IN_" + field.Name) == false)
                            continue;
                        inputs.Add(new IoInfo()
                        {
                            Name = field.Name,
                            Path = field.Name,
                            Type = field.FieldType
                        });
                    }

                    var outKeys = task.GetFields(BindingFlags.Public | BindingFlags.Static)
                        .Where(x => x.FieldType == typeof(string))
                        .Where(x => x.Name.StartsWith("OUT_"));

                    foreach (var outKey in outKeys)
                    {
                        outputs.Add(new IoInfo()
                        {
                            Name = outKey.Name.Substring("OUT_".Length),
                            Path = (string)outKey.GetValue(null)
                        });
                    }

                    taskInfo.Name = task.Name;
                    taskInfo.Inputs = inputs.ToArray();
                    taskInfo.Outputs = outputs.ToArray();

                    var json = JsonConvert.SerializeObject(taskInfo, Formatting.Indented);
                    Console.WriteLine(json);
                }
            }
        }

    }
}
