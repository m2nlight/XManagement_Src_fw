using System;
using System.IO;
using System.Text;
using BobWei.CSharp.Common.Utility;

namespace BobWei.CSharp.Common.IO
{
    /// <summary>
    /// 调试时使用的交互程序
    /// </summary>
    public sealed class ShellSingleton
    {
        private static readonly ShellSingleton Shell = new ShellSingleton();
        private string _outputFile; //输出文件名

        private ShellSingleton()
        {
        }

        /// <summary>
        /// 调试时使用的交互程序
        /// </summary>
        public static ShellSingleton Instance
        {
            get { return Shell; }
        }

        /// <summary>
        /// 指定输入信息到某个文件
        /// </summary>
        public string OutputFile
        {
            get { return _outputFile; }
            set
            {
                if (FileUtility.IsValidPath(value))
                {
                    _outputFile = value;
                    HasOutputFile = true;
                }
                else
                {
                    HasOutputFile = false;
                }
            }
        }

        /// <summary>
        /// 在输出失败时，判断是否可以输出信息到文件
        /// </summary>
        public bool HasOutputFile { get; private set; }

        /// <summary>
        /// 输出信息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void WriteLine(string format, params object[] args)
        {
            WriteLine(string.Format(format, args));
        }

        /// <summary>
        /// 输出信息
        /// </summary>
        /// <param name="output"></param>
        public void WriteLine(string output)
        {
            Console.ForegroundColor = GetConsoleColor(output);
            string outputText = string.Format(@"[{0}]{1}", DateTime.Now, output);
            Console.WriteLine(outputText);
            if (HasOutputFile) WriteToFile(outputText);
        }

        /// <summary>
        /// 只输出信息，不写入文件。信息会以 @ 开头，以区别WriteLine方法
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void WriteLineWithoutFile(string format, params object[] args)
        {
            WriteLineWithoutFile(string.Format(format, args));
        }

        /// <summary>
        /// 只输出信息，不写入文件。信息会以 @ 开头，以区别WriteLine方法
        /// </summary>
        /// <param name="output"></param>
        public void WriteLineWithoutFile(string output)
        {
            Console.ForegroundColor = GetConsoleColor(output);
            Console.WriteLine(@"@[{0}]{1}", DateTime.Now, output);
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="outputText"></param>
        private void WriteToFile(string outputText)
        {
            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(OutputFile, true, Encoding.UTF8);
                writer.WriteLine(outputText);
            }
            catch (Exception ex)
            {
                WriteLineWithoutFile("注意：没能写入调试信息到文件 {0}。{1}", OutputFile, ex.Message);
                HasOutputFile = false;
            }
            finally
            {
                if (writer != null) writer.Close();
            }
        }

        /// <summary>
        /// 根据输出文本选择控制台文字颜色
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        private static ConsoleColor GetConsoleColor(string output)
        {
            if (output.StartsWith("警告")) return ConsoleColor.Yellow;
            if (output.StartsWith("错误")) return ConsoleColor.Red;
            if (output.StartsWith("注意")) return ConsoleColor.Green;
            if (output.StartsWith("信息")) return ConsoleColor.White;
            return ConsoleColor.Gray;
        }

        /// <summary>
        /// 从标准输入流中取下一行字符串
        /// </summary>
        /// <returns></returns>
        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}