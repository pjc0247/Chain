using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Chain.FileIO
{
    public class Zip : ChainTask
    {
        #region IN_KEYS
        public static readonly string IN_SrcPath = nameof(SrcPath);
        public static readonly string IN_DstPath = nameof(DstPath);
        #endregion

        private string SrcPath;
        private string DstPath;

        public Zip()
        {
        }
        public Zip(string srcPath, string dstPath)
        {
            SrcPath = srcPath;
            DstPath = dstPath;
        }

        public override void OnExecute()
        {
            ZipFile.CreateFromDirectory(SrcPath, DstPath);
        }
    }
}
