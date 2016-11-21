using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Chain.FileIO
{
    public class CopyFile : ChainTask
    {
        #region IN_KEYS
        public static readonly string IN_SrcPath = nameof(SrcPath);
        public static readonly string IN_DstPath = nameof(DstPath);
        #endregion

        private string SrcPath;
        private string DstPath;

        public CopyFile()
        {
        }
        public CopyFile(string srcPath, string dstPath)
        {
            SrcPath = srcPath;
            DstPath = dstPath;
        }

        public override void OnExecute()
        {
            File.Copy(SrcPath, DstPath, true);
        }
    }
}
