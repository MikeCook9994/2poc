using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace _2pok
{
    public class MouseMonitor
    {
        public delegate void LocalMouseEventHandler(Utils.MSLLHOOKSTRUCT mouseInput);

        public LocalMouseEventHandler leftMouseButtonUpCallback;
        public LocalMouseEventHandler leftMouseButtonDownCallback;

        public LocalMouseEventHandler rightMouseButtonUpCallback;
        public LocalMouseEventHandler rightMouseButtonDownCallback;

        public LocalMouseEventHandler middleMouseButtonUpCallback;
        public LocalMouseEventHandler middleMouseButtonDownCallback;

        public LocalMouseEventHandler scrollMouseWheel;

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
                if (Utils.MouseMessages.WM_LBUTTONDOWN == (Utils.MouseMessages)wParam)
                {
                    Utils.MSLLHOOKSTRUCT hookStruct = Marshal.PtrToStructure<Utils.MSLLHOOKSTRUCT>(lParam);
                    this.leftMouseButtonDownCallback(hookStruct);
                }
                else if (Utils.MouseMessages.WM_LBUTTONUP == (Utils.MouseMessages)wParam)
                {
                    Utils.MSLLHOOKSTRUCT hookStruct = Marshal.PtrToStructure<Utils.MSLLHOOKSTRUCT>(lParam);
                    this.leftMouseButtonUpCallback(hookStruct);
                }
                else if (Utils.MouseMessages.WM_RBUTTONDOWN == (Utils.MouseMessages)wParam)
                {
                    Utils.MSLLHOOKSTRUCT hookStruct = Marshal.PtrToStructure<Utils.MSLLHOOKSTRUCT>(lParam);
                    this.rightMouseButtonDownCallback(hookStruct);
                }
                else if (Utils.MouseMessages.WM_RBUTTONUP == (Utils.MouseMessages)wParam)
                {
                    Utils.MSLLHOOKSTRUCT hookStruct = Marshal.PtrToStructure<Utils.MSLLHOOKSTRUCT>(lParam);
                    this.rightMouseButtonUpCallback(hookStruct);
                }
                else if (Utils.MouseMessages.WM_MBUTTONDOWN == (Utils.MouseMessages)wParam)
                {
                    Utils.MSLLHOOKSTRUCT hookStruct = Marshal.PtrToStructure<Utils.MSLLHOOKSTRUCT>(lParam);
                    this.middleMouseButtonDownCallback(hookStruct);
                }
                else if (Utils.MouseMessages.WM_MBUTTONUP == (Utils.MouseMessages)wParam)
                {
                    Utils.MSLLHOOKSTRUCT hookStruct = Marshal.PtrToStructure<Utils.MSLLHOOKSTRUCT>(lParam);
                    this.middleMouseButtonUpCallback(hookStruct);
                }
                else if (Utils.MouseMessages.WM_MOUSEWHEEL == (Utils.MouseMessages)wParam)
                {
                    Utils.MSLLHOOKSTRUCT hookStruct = Marshal.PtrToStructure<Utils.MSLLHOOKSTRUCT>(lParam);
                    this.scrollMouseWheel(hookStruct);
                }
                else if (Utils.MouseMessages.WM_MOUSEMOVE == (Utils.MouseMessages)wParam)
                {
                    Utils.MSLLHOOKSTRUCT hookStruct = Marshal.PtrToStructure<Utils.MSLLHOOKSTRUCT>(lParam);
                    this.mouseMovedCallback(hookStruct);
                }
            }
            return Utils.CallNextHookEx(this.hookID, nCode, wParam, lParam);
        }
    }
}
