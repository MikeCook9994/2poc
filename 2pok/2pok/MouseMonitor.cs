using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace _2pok
{
    public class MouseMonitor
    {
        private enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205,
            WM_MBUTTONDOWN = 0x0207,
            WM_MBUTTONUP = 0x0208
        }

        public delegate void LocalMouseEventHandler(Utils.MSLLHOOKSTRUCT mouseInput);

        public LocalMouseEventHandler leftMouseButtonUpCallback;
        public LocalMouseEventHandler leftMouseButtonDownCallback;

        public LocalMouseEventHandler rightMouseButtonUpCallback;
        public LocalMouseEventHandler rightMouseButtonDownCallback;

        public LocalMouseEventHandler middleMouseButtonUpCallback;
        public LocalMouseEventHandler middleMouseButtonDownCallback;

        public LocalMouseEventHandler scrollWheelUpCallback;
        public LocalMouseEventHandler scrollWheelDownCallback;

        public LocalMouseEventHandler mouseMovedCallback;

        private int hookID = 0;
        private Utils.InputProc hookCallback = null;

        bool global = false;
        bool isFinalized = false;

        public MouseMonitor(bool global)
        {
            this.global = global;
            this.hookCallback = new Utils.InputProc(HookCallback);

            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                if (this.global)
                {
                    this.hookID = Utils.SetWindowsHookEx(Utils.HookType.WH_MOUSE_LL, this.hookCallback, Utils.GetModuleHandle(curModule.ModuleName), 0);
                }
                else
                {
                    this.hookID = Utils.SetWindowsHookEx(Utils.HookType.WH_MOUSE, this.hookCallback, Utils.GetModuleHandle(curModule.ModuleName), Utils.GetCurrentThreadId());
                }
            }
        }

        ~MouseMonitor()
        {
            if (!isFinalized)
            {
                Utils.UnhookWindowsHookEx(this.hookID);
                isFinalized = true;
            }
        }

        public void Dispose()
        {
            if (!isFinalized)
            {
                Utils.UnhookWindowsHookEx(this.hookID);
                isFinalized = true;
            }
        }

        private int HookCallback(int nCode,  IntPtr wParam, IntPtr lParam)
        {
            if(nCode >= 0)
            {
                if (MouseMessages.WM_LBUTTONDOWN == (MouseMessages)wParam)
                {
                    Utils.MSLLHOOKSTRUCT hookStruct = Marshal.PtrToStructure<Utils.MSLLHOOKSTRUCT>(lParam);
                    this.leftMouseButtonDownCallback(hookStruct);
                }
                else if (MouseMessages.WM_LBUTTONUP == (MouseMessages)wParam)
                {
                    Utils.MSLLHOOKSTRUCT hookStruct = Marshal.PtrToStructure<Utils.MSLLHOOKSTRUCT>(lParam);
                    this.leftMouseButtonUpCallback(hookStruct);
                }
                else if (MouseMessages.WM_RBUTTONDOWN == (MouseMessages)wParam)
                {
                    Utils.MSLLHOOKSTRUCT hookStruct = Marshal.PtrToStructure<Utils.MSLLHOOKSTRUCT>(lParam);
                    this.rightMouseButtonDownCallback(hookStruct);
                }
                else if (MouseMessages.WM_RBUTTONUP == (MouseMessages)wParam)
                {
                    Utils.MSLLHOOKSTRUCT hookStruct = Marshal.PtrToStructure<Utils.MSLLHOOKSTRUCT>(lParam);
                    this.rightMouseButtonUpCallback(hookStruct);
                }
                else if (MouseMessages.WM_MBUTTONDOWN == (MouseMessages)wParam)
                {
                    Utils.MSLLHOOKSTRUCT hookStruct = Marshal.PtrToStructure<Utils.MSLLHOOKSTRUCT>(lParam);
                    this.middleMouseButtonDownCallback(hookStruct);
                }
                else if (MouseMessages.WM_MBUTTONUP == (MouseMessages)wParam)
                {
                    Utils.MSLLHOOKSTRUCT hookStruct = Marshal.PtrToStructure<Utils.MSLLHOOKSTRUCT>(lParam);
                    this.middleMouseButtonUpCallback(hookStruct);
                }
                else if (MouseMessages.WM_MOUSEWHEEL == (MouseMessages)wParam)
                {
                    Utils.MSLLHOOKSTRUCT hookStruct = Marshal.PtrToStructure<Utils.MSLLHOOKSTRUCT>(lParam);
                    if (hookStruct.mouseData > 0)
                    {
                        this.scrollWheelUpCallback(hookStruct);
                    }
                    else
                    {
                        this.scrollWheelDownCallback(hookStruct);
                    }
                }
                else if (MouseMessages.WM_MOUSEMOVE == (MouseMessages)wParam)
                {
                    Utils.MSLLHOOKSTRUCT hookStruct = Marshal.PtrToStructure<Utils.MSLLHOOKSTRUCT>(lParam);
                    this.mouseMovedCallback(hookStruct);
                }
            }
            return Utils.CallNextHookEx(this.hookID, nCode, wParam, lParam);
        }
    }
}
