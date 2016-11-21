using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Chain.Common
{
    public class StartProcess : ChainTask
    {
        #region IN_KEYS
        public static readonly string IN_Path = nameof(Path);
        public static readonly string IN_Args = nameof(Args);

        public static readonly string IN_ExpectedExitCode = nameof(ExpectedExitCode);
        #endregion
        #region OUT_KEYS
        public static readonly string OUT_ExitCode = $"Common.{nameof(StartProcess)}.ExitCode";
        #endregion

        private string Path;
        private string Args;

        private int? ExpectedExitCode;

        public StartProcess(string path, string args)
        {
            Path = path;
            Args = args;
        }
        public StartProcess(string path, string args, int expectedExitCode)
        {
            Path = path;
            Args = args;
            ExpectedExitCode = expectedExitCode;
        }

        public override void OnExecute()
        {
            var p = Process.Start(Path, Args);

            p.WaitForExit();

            Context.Out(OUT_ExitCode, p.ExitCode);

            if (ExpectedExitCode.HasValue)
            {
                if (p.ExitCode != ExpectedExitCode.Value)
                    throw new InvalidOperationException($"{nameof(ExpectedExitCode)} != ExitCode, ExitCode => {p.ExitCode}");
            }
        }
    }
}
