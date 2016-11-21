using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Chain.FileIO
{
    public class WriteFile : ChainTask
    {
        #region IN_KEYS
        public static string IN_Path = nameof(Path);
        public static string IN_Message = nameof(Message);
        #endregion

        private string Path;
        private string Message;

        public WriteFile()
        {
        }
        public WriteFile(string path)
        {
            Path = path;
        }
        public WriteFile(string path, string message)
        {
            Path = path;
            Message = message;
        }

        public override void OnExecute()
        {
            File.WriteAllText(Path, Message);
        }
    }

    public class AppendFile : ChainTask
    {
        #region IN_KEYS
        public static string IN_Path = nameof(Path);
        public static string IN_Message = nameof(Message);
        #endregion

        private string Path;
        private string Message;

        public AppendFile()
        {
        }
        public AppendFile(string path)
        {
            Path = path;
        }
        public AppendFile(string path, string message)
        {
            Path = path;
            Message = message;
        }

        public override void OnExecute()
        {
            File.AppendAllText(Path, Message);
        }
    }
}
