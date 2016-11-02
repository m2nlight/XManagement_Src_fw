/* ==============================
 * 全局程序集信息
 * GlobalAssemblyInfo.cs
 * 
 * 请把此文件引用到其他的项目中
 ==============================*/

using System.Reflection;

[assembly: AssemblyProduct("XManagement")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyVersion(RevisionClass.FullVersion)]
#if DEBUG 
[assembly : AssemblyConfiguration("Debug")] 
#else
[assembly: AssemblyConfiguration("Release")]
#endif
[assembly: AssemblyCopyright("2010 BobWei")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]


internal static class RevisionClass
{
    public const string Major = "0";
    public const string Minor = "1";
    public const string Build = "0";
    public const string Revision = "0";

    public const string MainVersion = Major + "." + Minor;
    public const string FullVersion = Major + "." + Minor + "." + Build + "." + Revision;
}



/*

其他程序集的AssemblyInfo.cs简化如下内容
所有信息数据单独填写

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("XManagement")]
[assembly: AssemblyDescription("X Management System")]

// 如果此项目向 COM 公开，则下列 GUID 用于类型库的 ID
[assembly: Guid("cbdbc1e8-4384-45b6-b10a-e8c116822d20")]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]
[assembly: StringFreezing()]
*/
