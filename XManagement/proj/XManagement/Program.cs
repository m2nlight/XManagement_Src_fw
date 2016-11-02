using System;

namespace BobWei.XManagement
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            StartProgram.Start(args);
            return 0;
        }
    }
}
