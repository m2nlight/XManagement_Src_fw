using System;
using System.Drawing;

namespace BobWei.CSharp.Common.Utility
{
    /// <summary>
    /// Gui类
    /// </summary>
    public static class GuiUtility
    {
        #region AnimateWindowStyle enum

        /// <summary>
        /// 动画窗口风格
        /// </summary>
        public enum AnimateWindowStyle
        {
            None = 0, //无
            SlideInUpToDown, //由上向下滑入
            SlideInDownToUp, //由下向上滑入
            SlideInLeftToRight, //由左向右滑入
            SlideInRightToLeft, //由右向左滑入
            SlideOutUpToDown, //由上向下滑出
            SlideOutDownToUp, //由下向上滑出
            SlideOutLeftToRight, //由左向右滑出
            SlideOutRightToLeft, //由右向左滑出
            FadeIn, //淡入(顶层窗口)
            FadeOut, //淡出(顶层窗口)
            CenterIn, //从中心扩展
            CenterOut //向中心收缩
        }

        #endregion

        /// <summary>
        /// 将某个窗口置前显示
        /// </summary>
        /// <param name="handle">窗口的HWND</param>
        public static void SetWindowFront(IntPtr handle)
        {
            NativeMethods.SetWindowPos(handle, IntPtr.Zero, 0, 0, 0, 0,
                                       NativeConstants.SWP_NOSIZE | NativeConstants.SWP_NOMOVE |
                                       NativeConstants.SWP_SHOWWINDOW);
        }

        /// <summary>
        /// 将窗口置前，但不激活
        /// </summary>
        /// <param name="handle"></param>
        public static void SetWindowFrontWithNoTopmostNoActivite(IntPtr handle)
        {
            NativeMethods.SetWindowPos(handle, IntPtr.Zero, 0, 0, 0, 0,
                                       NativeConstants.SWP_NOSIZE | NativeConstants.SWP_NOMOVE |
                                       NativeConstants.SWP_NOACTIVATE);
        }

        /// <summary>
        /// 将某个窗口显示在工作区内
        /// </summary>
        /// <param name="windowArea">窗口区域</param>
        /// <param name="workingArea">工作区域</param>
        /// <returns>窗口的合适位置</returns>
        public static Point SetWindowFitLocation(Rectangle windowArea, Rectangle workingArea)
        {
            Point location = windowArea.Location;
            if (windowArea.Left < workingArea.Left)
            {
                location.X = workingArea.Left;
            }
            else if (windowArea.Right > workingArea.Right)
            {
                location.X = workingArea.Right - windowArea.Width;
            }
            if (windowArea.Top < workingArea.Top)
            {
                location.Y = workingArea.Top;
            }
            else if (windowArea.Bottom > workingArea.Bottom)
            {
                location.Y = workingArea.Bottom - windowArea.Height;
            }
            return location;
        }

        /// <summary>
        /// 显示一个包含Yes和No的前置消息框
        /// </summary>
        /// <param name="text"></param>
        /// <param name="caption"></param>
        /// <returns>return true for Yes(6),false for No(7)</returns>
        public static bool ShowMessageBoxYesNo(string text, string caption)
        {
            return NativeConstants.IDYES ==
                   NativeMethods.MessageBoxW(IntPtr.Zero, text, caption,
                                             NativeConstants.MB_YESNO | NativeConstants.MB_ICONQUESTION |
                                             NativeConstants.MB_TOPMOST | NativeConstants.MB_TASKMODAL);
        }

        /// <summary>
        /// 显示一个包含Hand图标的前置消息框
        /// </summary>
        /// <param name="text"></param>
        /// <param name="caption"></param>
        public static void ShowMessageBoxHand(string text, string caption)
        {
            NativeMethods.MessageBoxW(IntPtr.Zero, text, caption,
                                      NativeConstants.MB_OK | NativeConstants.MB_ICONHAND | NativeConstants.MB_TOPMOST |
                                      NativeConstants.MB_TASKMODAL);
        }

        //public static void NoCloseButton(IntPtr handle)
        //{
        //    int style=NativeMethods.GetWindowLongW(handle,NativeConstants.GWL_STYLE);
        //    NativeMethods.SetWindowLongW(handle, NativeConstants.GWL_EXSTYLE, style | NativeConstants.CS_NOCLOSE);
        //}

        /// <summary>
        /// 动画窗口
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="time"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        [CLSCompliantAttribute(false)]
        public static bool AnimateWindow(IntPtr handle, uint time, AnimateWindowStyle style)
        {
            uint flags = 0;
            switch (style)
            {
                case AnimateWindowStyle.None:
                    return false;
                case AnimateWindowStyle.SlideInUpToDown:
                    flags = NativeConstants.AW_SLIDE + NativeConstants.AW_VER_POSITIVE + NativeConstants.AW_ACTIVATE;
                    break;
                case AnimateWindowStyle.SlideInDownToUp:
                    flags = NativeConstants.AW_SLIDE + NativeConstants.AW_VER_NEGATIVE + NativeConstants.AW_ACTIVATE;
                    break;
                case AnimateWindowStyle.SlideInLeftToRight:
                    flags = NativeConstants.AW_SLIDE + NativeConstants.AW_HOR_POSITIVE + NativeConstants.AW_ACTIVATE;
                    break;
                case AnimateWindowStyle.SlideInRightToLeft:
                    flags = NativeConstants.AW_SLIDE + NativeConstants.AW_HOR_NEGATIVE + NativeConstants.AW_ACTIVATE;
                    break;
                case AnimateWindowStyle.SlideOutUpToDown:
                    flags = NativeConstants.AW_SLIDE + NativeConstants.AW_VER_POSITIVE + NativeConstants.AW_HIDE;
                    break;
                case AnimateWindowStyle.SlideOutDownToUp:
                    flags = NativeConstants.AW_SLIDE + NativeConstants.AW_VER_NEGATIVE + NativeConstants.AW_HIDE;
                    break;
                case AnimateWindowStyle.SlideOutLeftToRight:
                    flags = NativeConstants.AW_SLIDE + NativeConstants.AW_HOR_POSITIVE + NativeConstants.AW_HIDE;
                    break;
                case AnimateWindowStyle.SlideOutRightToLeft:
                    flags = NativeConstants.AW_SLIDE + NativeConstants.AW_HOR_NEGATIVE + NativeConstants.AW_HIDE;
                    break;
                case AnimateWindowStyle.FadeIn:
                    flags = NativeConstants.AW_BLEND + NativeConstants.AW_ACTIVATE;
                    break;
                case AnimateWindowStyle.FadeOut:
                    flags = NativeConstants.AW_BLEND + NativeConstants.AW_HIDE;
                    break;
                case AnimateWindowStyle.CenterIn:
                    flags = NativeConstants.AW_CENTER + NativeConstants.AW_ACTIVATE;
                    break;
                case AnimateWindowStyle.CenterOut:
                    flags = NativeConstants.AW_CENTER + NativeConstants.AW_HIDE;
                    break;
            }
            return NativeMethods.AnimateWindow(handle, time, flags);
        }
    }
}