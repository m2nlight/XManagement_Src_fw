using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace BobWei.CSharp.Common
{
    public class SystemInfo
    {
        [DllImport("kernel32")]
        public static extern void GetWindowsDirectory(StringBuilder winDir, int count);

        [DllImport("kernel32")]
        public static extern void GetSystemDirectory(StringBuilder sysDir, int count);

        [DllImport("kernel32")]
        public static extern void GetSystemInfo(ref CPU_INFO cpuinfo);

        [DllImport("kernel32")]
        public static extern void GlobalMemoryStatus(ref MEMORY_INFO meminfo);

        [DllImport("kernel32")]
        public static extern void GetSystemTime(ref SYSTEMTIME_INFO stinfo);

        public static string GetSystemInfo(IEnumerable<Assembly> assemblies)
        {
            var sb = new StringBuilder();

            try
            {
                //重要信息
                sb.AppendLine("程序集信息：");
                foreach (AssemblyName assemblyName in assemblies.Select(n => n.GetName()))
                {
                    sb.AppendLine("\t" + assemblyName);
                }
                sb.AppendLine("网络信息：");
                string hostName = string.Empty;
                try
                {
                    hostName = Dns.GetHostName();
                }
                catch (SocketException)
                {
                }
                sb.AppendLine("\tLocalHostName: " + hostName);
                if (hostName.Length > 0)
                {
                    try
                    {
                        IPAddress[] addr = Dns.GetHostAddresses(hostName);
                        for (int i = 0; i < addr.Length; i++)
                        {
                            sb.AppendLine(string.Format("\tIP Address {0}: {1}", i, addr[i]));
                        }
                    }
                    catch
                    {
                    }
                }


                //获取操作系统设置
                sb.AppendLine("计算机名：" + SystemInformation.ComputerName);
                sb.AppendLine("是否已连接网络：" + SystemInformation.Network);
                sb.AppendLine("用户域：" + SystemInformation.UserDomainName);
                sb.AppendLine("当前线程用户名：" + SystemInformation.UserName);
                sb.AppendLine("启动方式：" + SystemInformation.BootMode);
                sb.AppendLine("菜单的字体：" + SystemInformation.MenuFont);
                sb.AppendLine("显示器的数目：" + SystemInformation.MonitorCount);
                sb.AppendLine("鼠标已安装：" + SystemInformation.MousePresent);
                sb.AppendLine("鼠标按钮数：" + SystemInformation.MouseButtons);
                sb.AppendLine("是否交互模式：" + SystemInformation.UserInteractive);
                sb.AppendLine("屏幕界限：" + SystemInformation.VirtualScreen);

                //获取程序运行的相关信息.
                sb.AppendLine("命令行：" + Environment.CommandLine);
                sb.AppendLine("命令行参数：" + String.Join(", ", Environment.GetCommandLineArgs()));
                sb.AppendLine("当前目录：" + Environment.CurrentDirectory);
                sb.AppendLine("操作系统版本：" + Environment.OSVersion);
                sb.AppendLine("系统目录：" + Environment.SystemDirectory);
                sb.AppendLine("堆栈信息：" + Environment.StackTrace);
                sb.AppendLine("用户域：" + Environment.UserDomainName);
                sb.AppendLine("CLR版本：" + Environment.Version);
                sb.AppendLine("系统启动后经过的毫秒：" + Environment.TickCount);
                sb.AppendLine("进程上下文的物理内存量：" + Environment.WorkingSet);
                string[] drives = Environment.GetLogicalDrives();
                sb.AppendLine("本机磁盘驱动器：" + String.Join(", ", drives));

                // 获取本机所有环境变量
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                foreach (DictionaryEntry de in environmentVariables)
                {
                    sb.AppendLine(de.Key + "=" + de.Value);
                }

                //通过注册表获取信息
                RegistryKey rkey = Registry.LocalMachine;
                rkey = rkey.OpenSubKey("HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0");
                if (rkey != null) sb.AppendLine("处理器信息：" + rkey.GetValue("ProcessorNameString").ToString().Trim());

                rkey = Registry.LocalMachine;
                rkey = rkey.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\NetworkCards\\1");
                if (rkey != null) sb.AppendLine("网卡信息：" + (String) rkey.GetValue("Description"));

                //调用GetWindowsDirectory和GetSystemDirectory函数分别取得Windows路径和系统路径 
                const int nChars = 128;
                var buff = new StringBuilder(nChars);
                GetWindowsDirectory(buff, nChars);
                sb.AppendLine("Windows路径：" + buff);
                GetSystemDirectory(buff, nChars);
                sb.AppendLine("系统路径：" + buff);
                //调用GetSystemInfo函数获取CPU的相关信息 
                var cpuInfo = new CPU_INFO();
                GetSystemInfo(ref cpuInfo);
                //CPU信息的读取是错误的,我本只有一个CPU,读成了两个
                sb.AppendLine("本计算机中有" + cpuInfo.dwNumberOfProcessors + "个CPU");
                sb.AppendLine("CPU的类型为" + cpuInfo.dwProcessorType);
                sb.AppendLine("CPU等级为" + cpuInfo.dwProcessorLevel);
                sb.AppendLine("CPU的OEM ID为" + cpuInfo.dwOemId);
                sb.AppendLine("CPU中的页面大小为" + cpuInfo.dwPageSize);
                //调用GlobalMemoryStatus函数获取内存的相关信息 
                var memInfo = new MEMORY_INFO();
                GlobalMemoryStatus(ref memInfo);
                sb.AppendLine(memInfo.dwMemoryLoad + "%的内存正在使用");
                sb.AppendLine("物理内存共有" + memInfo.dwTotalPhys + "字节");
                sb.AppendLine("可使用的物理内存有" + memInfo.dwAvailPhys + "字节");
                sb.AppendLine("交换文件总大小为" + memInfo.dwTotalPageFile + "字节");
                sb.AppendLine("尚可交换文件大小为" + memInfo.dwAvailPageFile + "字节");
                sb.AppendLine("总虚拟内存有" + memInfo.dwTotalVirtual + "字节");
                sb.AppendLine("未用虚拟内存有" + memInfo.dwAvailVirtual + "字节");
                //调用GetSystemTime函数获取系统时间信息 
                var stInfo = new SYSTEMTIME_INFO();
                GetSystemTime(ref stInfo);
                sb.AppendLine(stInfo.wYear + "年" + stInfo.wMonth + "月" + stInfo.wDay + "日");
                sb.AppendLine((stInfo.wHour + 8) + "时" + stInfo.wMinute + "分" + stInfo.wSecond + "秒");
            }
            catch
            {
            }

            return sb.ToString();
        }

        #region Nested type： CPU_INFO

        [StructLayout(LayoutKind.Sequential)]
        [CLSCompliant(false)]
        public struct CPU_INFO
        {
            public uint dwOemId;
            public uint dwPageSize;
            public uint lpMinimumApplicationAddress;
            public uint lpMaximumApplicationAddress;
            public uint dwActiveProcessorMask;
            public uint dwNumberOfProcessors;
            public uint dwProcessorType;
            public uint dwAllocationGranularity;
            public uint dwProcessorLevel;
            public uint dwProcessorRevision;
        }

        #endregion

        #region Nested type： MEMORY_INFO

        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_INFO
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public uint dwTotalPhys;
            public uint dwAvailPhys;
            public uint dwTotalPageFile;
            public uint dwAvailPageFile;
            public uint dwTotalVirtual;
            public uint dwAvailVirtual;
        }

        #endregion

        #region Nested type： SYSTEMTIME_INFO

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEMTIME_INFO
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;
        }

        #endregion
    }
}