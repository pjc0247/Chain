using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Chain.FileIO
{
    public class DeleteFile : ChainTask
    {
        #region IN_KEYS
        public static string IN_Path = nameof(Path);
        #endregion

        private string Path;

        public DeleteFile()
        {
        }
        public DeleteFile(string path)
        {
            Path = path;
        }

        public override void OnExecute()
        {
            File.Delete(Path);
        }
    }
}
