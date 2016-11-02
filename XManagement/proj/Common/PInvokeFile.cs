using System;

namespace BobWei.CSharp.Common
{
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct HWND__
    {

        /// int
        public int unused;
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct HICON__
    {

        /// int
        public int unused;
    }

    [CLSCompliant(false)]
    public partial class NativeMethods
    {

        /// Return Type: BOOL->int
        ///hWnd: HWND->HWND__*
        ///hWndInsertAfter: HWND->HWND__*
        ///X: int
        ///Y: int
        ///cx: int
        ///cy: int
        ///uFlags: UINT->unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "SetWindowPos")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool SetWindowPos([System.Runtime.InteropServices.InAttribute()] System.IntPtr hWnd, [System.Runtime.InteropServices.InAttribute()] System.IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        /// Return Type: BOOL->int
        ///hWnd: HWND->HWND__*
        ///dwTime: DWORD->unsigned int
        ///dwFlags: DWORD->unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "AnimateWindow")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool AnimateWindow([System.Runtime.InteropServices.InAttribute()] System.IntPtr hWnd, uint dwTime, uint dwFlags);

        /// Return Type: UINT->unsigned int
        ///lpszFile: LPCWSTR->WCHAR*
        ///nIconIndex: int
        ///phiconLarge: HICON*
        ///phiconSmall: HICON*
        ///nIcons: UINT->unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("shell32.dll", EntryPoint = "ExtractIconExW", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern uint ExtractIconExW([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string lpszFile, int nIconIndex, ref System.IntPtr phiconLarge, ref System.IntPtr phiconSmall, uint nIcons);

        /// Return Type: int
        ///hWnd: HWND->HWND__*
        ///lpText: LPCWSTR->WCHAR*
        ///lpCaption: LPCWSTR->WCHAR*
        ///uType: UINT->unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "MessageBoxW")]
        public static extern int MessageBoxW([System.Runtime.InteropServices.InAttribute()] System.IntPtr hWnd, [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string lpText, [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string lpCaption, uint uType);

        /// Return Type: LRESULT->LONG_PTR->int
        ///hWnd: HWND->HWND__*
        ///Msg: UINT->unsigned int
        ///wParam: WPARAM->UINT_PTR->unsigned int
        ///lParam: LPARAM->LONG_PTR->int
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "SendMessageW")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.SysInt)]
        public static extern int SendMessageW([System.Runtime.InteropServices.InAttribute()] System.IntPtr hWnd, uint Msg, [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.SysUInt)] uint wParam, [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.SysInt)] int lParam);

        /// Return Type: LONG->int
        ///hWnd: HWND->HWND__*
        ///nIndex: int
        ///dwNewLong: LONG->int
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "SetWindowLongW")]
        public static extern int SetWindowLongW([System.Runtime.InteropServices.InAttribute()] System.IntPtr hWnd, int nIndex, int dwNewLong);

        /// Return Type: LONG->int
        ///hWnd: HWND->HWND__*
        ///nIndex: int
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "GetWindowLongW")]
        public static extern int GetWindowLongW([System.Runtime.InteropServices.InAttribute()] System.IntPtr hWnd, int nIndex);

    }


    public partial class NativeConstants
    {

        /// SWP_NOSENDCHANGING -> 0x0400
        public const int SWP_NOSENDCHANGING = 1024;

        /// SWP_ASYNCWINDOWPOS -> 0x4000
        public const int SWP_ASYNCWINDOWPOS = 16384;

        /// SWP_NOOWNERZORDER -> 0x0200
        public const int SWP_NOOWNERZORDER = 512;

        /// SWP_NOREPOSITION -> SWP_NOOWNERZORDER
        public const int SWP_NOREPOSITION = NativeConstants.SWP_NOOWNERZORDER;

        /// SWP_FRAMECHANGED -> 0x0020
        public const int SWP_FRAMECHANGED = 32;

        /// SWP_SHOWWINDOW -> 0x0040
        public const int SWP_SHOWWINDOW = 64;

        /// SWP_NOCOPYBITS -> 0x0100
        public const int SWP_NOCOPYBITS = 256;

        /// SWP_NOACTIVATE -> 0x0010
        public const int SWP_NOACTIVATE = 16;

        /// SWP_HIDEWINDOW -> 0x0080
        public const int SWP_HIDEWINDOW = 128;

        /// SWP_DEFERERASE -> 0x2000
        public const int SWP_DEFERERASE = 8192;

        /// SWP_DRAWFRAME -> SWP_FRAMECHANGED
        public const int SWP_DRAWFRAME = NativeConstants.SWP_FRAMECHANGED;

        /// SWP_NOZORDER -> 0x0004
        public const int SWP_NOZORDER = 4;

        /// SWP_NOREDRAW -> 0x0008
        public const int SWP_NOREDRAW = 8;

        /// SWP_NOSIZE -> 0x0001
        public const int SWP_NOSIZE = 1;

        /// SWP_NOMOVE -> 0x0002
        public const int SWP_NOMOVE = 2;

        /// AW_VER_POSITIVE -> 0x00000004
        public const int AW_VER_POSITIVE = 4;

        /// AW_VER_NEGATIVE -> 0x00000008
        public const int AW_VER_NEGATIVE = 8;

        /// AW_HOR_POSITIVE -> 0x00000001
        public const int AW_HOR_POSITIVE = 1;

        /// AW_HOR_NEGATIVE -> 0x00000002
        public const int AW_HOR_NEGATIVE = 2;

        /// AW_ACTIVATE -> 0x00020000
        public const int AW_ACTIVATE = 131072;

        /// AW_CENTER -> 0x00000010
        public const int AW_CENTER = 16;

        /// AW_SLIDE -> 0x00040000
        public const int AW_SLIDE = 262144;

        /// AW_BLEND -> 0x00080000
        public const int AW_BLEND = 524288;

        /// AW_HIDE -> 0x00010000
        public const int AW_HIDE = 65536;

        /// MB_SERVICE_NOTIFICATION_NT3X -> 0x00040000L
        public const int MB_SERVICE_NOTIFICATION_NT3X = 262144;

        /// MB_SERVICE_NOTIFICATION -> 0x00200000L
        public const int MB_SERVICE_NOTIFICATION = 2097152;

        /// MB_DEFAULT_DESKTOP_ONLY -> 0x00020000L
        public const int MB_DEFAULT_DESKTOP_ONLY = 131072;

        /// MB_ERR_INVALID_CHARS -> 0x00000008
        public const int MB_ERR_INVALID_CHARS = 8;

        /// MB_CANCELTRYCONTINUE -> 0x00000006L
        public const int MB_CANCELTRYCONTINUE = 6;

        /// MB_ABORTRETRYIGNORE -> 0x00000002L
        public const int MB_ABORTRETRYIGNORE = 2;

        /// MB_ICONINFORMATION -> MB_ICONASTERISK
        public const int MB_ICONINFORMATION = NativeConstants.MB_ICONASTERISK;

        /// MB_ICONEXCLAMATION -> 0x00000030L
        public const int MB_ICONEXCLAMATION = 48;

        /// MB_USEGLYPHCHARS -> 0x00000004
        public const int MB_USEGLYPHCHARS = 4;

        /// MB_SETFOREGROUND -> 0x00010000L
        public const int MB_SETFOREGROUND = 65536;

        /// MB_ICONQUESTION -> 0x00000020L
        public const int MB_ICONQUESTION = 32;

        /// MB_ICONASTERISK -> 0x00000040L
        public const int MB_ICONASTERISK = 64;

        /// MB_YESNOCANCEL -> 0x00000003L
        public const int MB_YESNOCANCEL = 3;

        /// MB_SYSTEMMODAL -> 0x00001000L
        public const int MB_SYSTEMMODAL = 4096;

        /// MB_RETRYCANCEL -> 0x00000005L
        public const int MB_RETRYCANCEL = 5;

        /// MB_PRECOMPOSED -> 0x00000001
        public const int MB_PRECOMPOSED = 1;

        /// MB_ICONWARNING -> MB_ICONEXCLAMATION
        public const int MB_ICONWARNING = NativeConstants.MB_ICONEXCLAMATION;

        /// MB_RTLREADING -> 0x00100000L
        public const int MB_RTLREADING = 1048576;

        /// MB_DEFBUTTON4 -> 0x00000300L
        public const int MB_DEFBUTTON4 = 768;

        /// MB_DEFBUTTON3 -> 0x00000200L
        public const int MB_DEFBUTTON3 = 512;

        /// MB_DEFBUTTON2 -> 0x00000100L
        public const int MB_DEFBUTTON2 = 256;

        /// MB_DEFBUTTON1 -> 0x00000000L
        public const int MB_DEFBUTTON1 = 0;

        /// MB_TASKMODAL -> 0x00002000L
        public const int MB_TASKMODAL = 8192;

        /// MB_ICONERROR -> MB_ICONHAND
        public const int MB_ICONERROR = NativeConstants.MB_ICONHAND;

        /// MB_COMPOSITE -> 0x00000002
        public const int MB_COMPOSITE = 2;

        /// MB_APPLMODAL -> 0x00000000L
        public const int MB_APPLMODAL = 0;

        /// MB_USERICON -> 0x00000080L
        public const int MB_USERICON = 128;

        /// MB_TYPEMASK -> 0x0000000FL
        public const int MB_TYPEMASK = 15;

        /// MB_OKCANCEL -> 0x00000001L
        public const int MB_OKCANCEL = 1;

        /// MB_MODEMASK -> 0x00003000L
        public const int MB_MODEMASK = 12288;

        /// MB_MISCMASK -> 0x0000C000L
        public const int MB_MISCMASK = 49152;

        /// MB_ICONSTOP -> MB_ICONHAND
        public const int MB_ICONSTOP = NativeConstants.MB_ICONHAND;

        /// MB_ICONMASK -> 0x000000F0L
        public const int MB_ICONMASK = 240;

        /// MB_ICONHAND -> 0x00000010L
        public const int MB_ICONHAND = 16;

        /// MB_TOPMOST -> 0x00040000L
        public const int MB_TOPMOST = 262144;

        /// MB_NOFOCUS -> 0x00008000L
        public const int MB_NOFOCUS = 32768;

        /// MB_LEN_MAX -> 5
        public const int MB_LEN_MAX = 5;

        /// MB_DEFMASK -> 0x00000F00L
        public const int MB_DEFMASK = 3840;

        /// MB_YESNO -> 0x00000004L
        public const int MB_YESNO = 4;

        /// MB_RIGHT -> 0x00080000L
        public const int MB_RIGHT = 524288;

        /// MB_HELP -> 0x00004000L
        public const int MB_HELP = 16384;

        /// MB_OK -> 0x00000000L
        public const int MB_OK = 0;

        /// IDTRYAGAIN -> 10
        public const int IDTRYAGAIN = 10;

        /// IDCONTINUE -> 11
        public const int IDCONTINUE = 11;

        /// IDIGNORE -> 5
        public const int IDIGNORE = 5;

        /// IDCLOSE -> 8
        public const int IDCLOSE = 8;

        /// IDRETRY -> 4
        public const int IDRETRY = 4;

        /// IDCANCEL -> 2
        public const int IDCANCEL = 2;

        /// IDABORT -> 3
        public const int IDABORT = 3;

        /// IDHELP -> 9
        public const int IDHELP = 9;

        /// IDYES -> 6
        public const int IDYES = 6;

        /// IDOK -> 1
        public const int IDOK = 1;

        /// IDNO -> 7
        public const int IDNO = 7;

        /// GWL_STYLE -> (-16)
        public const int GWL_STYLE = -16;

        /// GWL_EXSTYLE -> (-20)
        public const int GWL_EXSTYLE = -20;

        /// <summary>
        /// 去除关闭按钮
        /// </summary>
        public const int CS_NOCLOSE = 0x200;
    }

}
