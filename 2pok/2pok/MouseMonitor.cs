using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace _2pok
{
    public class MouseMonitor
    {
        public delegate void MouseMovementEventHandler(Utils.POINT point);
        public delegate void MouseScrollEventHandler(int wheelDelta);
        public delegate void MouseClickEventHandler();

        public MouseClickEventHandler leftMouseButtonUpCallback;
        public MouseClickEventHandler leftMouseButtonDownCallback;

        public MouseClickEventHandler rightMouseButtonUpCallback;
        public MouseClickEventHandler rightMouseButtonDownCallback;

        public MouseClickEventHandler middleMouseButtonUpCallback;
        public MouseClickEventHandler middleMouseButtonDownCallback;

        public MouseScrollEventHandler scrollMouseWheel;

        public MouseMovementEventHandler mouseMovedCallback;

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
                if (Utils.MouseMessages.WM_LBUTTONDOWN == (Utils.MouseMessages)wParam)
                {
                    this.leftMouseButtonDownCallback();
                }
                else if (Utils.MouseMessages.WM_LBUTTONUP == (Utils.MouseMessages)wParam)
                {
                    this.leftMouseButtonUpCallback();
                }
                else if (Utils.MouseMessages.WM_RBUTTONDOWN == (Utils.MouseMessages)wParam)
                {
                    this.rightMouseButtonDownCallback();
                }
                else if (Utils.MouseMessages.WM_RBUTTONUP == (Utils.MouseMessages)wParam)
                {
                    this.rightMouseButtonUpCallback();
                }
                else if (Utils.MouseMessages.WM_MBUTTONDOWN == (Utils.MouseMessages)wParam)
                {
                    this.middleMouseButtonDownCallback();
                }
                else if (Utils.MouseMessages.WM_MBUTTONUP == (Utils.MouseMessages)wParam)
                {
                    this.middleMouseButtonUpCallback();
                }
                else if (Utils.MouseMessages.WM_MOUSEWHEEL == (Utils.MouseMessages)wParam)
                {
                    Utils.MSLLHOOKSTRUCT hookStruct = Marshal.PtrToStructure<Utils.MSLLHOOKSTRUCT>(lParam);
                    this.scrollMouseWheel(hookStruct.mouseData);
                }
                else if (Utils.MouseMessages.WM_MOUSEMOVE == (Utils.MouseMessages)wParam)
                {
                    Utils.MSLLHOOKSTRUCT hookStruct = Marshal.PtrToStructure<Utils.MSLLHOOKSTRUCT>(lParam);
                    this.mouseMovedCallback(hookStruct.pt);
                }
            }
            return Utils.CallNextHookEx(this.hookID, nCode, wParam, lParam);
        }
    }
}
