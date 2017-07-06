using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace _2pok
{
    class MouseMonitor
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public long x;
            public long y;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEHOOKSTRUCT {
            public POINT pt;
            public uint hwnd;
            public uint wHitTestCode;
            public UIntPtr dwExtraInfo;
        };

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

        public delegate void LocalMouseEventHandler(MOUSEHOOKSTRUCT mouseInput);

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
        private Utils.CallbackDelegate hookCallback = null;

        bool global = false;
        bool isFinalized = false;

        public MouseMonitor(bool global)
        {
            this.global = global;
            this.hookCallback = new Utils.CallbackDelegate(HookCallback);

            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                if (this.global)
                {
                    this.hookID = Utils.SetWindowsHookEx(Utils.HookType.WH_MOUSE_LL, this.hookCallback, Utils.GetModuleHandle(curModule.ModuleName), 0);
                    Console.WriteLine(this.hookID);
                }
                else
                {
                    this.hookID = Utils.SetWindowsHookEx(Utils.HookType.WH_MOUSE, this.hookCallback, 0, Utils.GetCurrentThreadId());
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

        private int HookCallback(int nCode, int wParam, int lParam)
        {
            if (nCode >= 0 && MouseMessages.WM_LBUTTONDOWN == (MouseMessages)wParam)
            {
                MOUSEHOOKSTRUCT hookStruct = (MOUSEHOOKSTRUCT)Marshal.PtrToStructure((IntPtr)lParam, typeof(MOUSEHOOKSTRUCT));
                this.leftMouseButtonDownCallback(hookStruct);
            }
            else if (nCode >= 0 && MouseMessages.WM_LBUTTONUP == (MouseMessages)wParam)
            {
                MOUSEHOOKSTRUCT hookStruct = (MOUSEHOOKSTRUCT)Marshal.PtrToStructure((IntPtr)lParam, typeof(MOUSEHOOKSTRUCT));
                this.leftMouseButtonUpCallback(hookStruct);
            }
            else if (nCode >= 0 && MouseMessages.WM_RBUTTONDOWN == (MouseMessages)wParam)
            {
                MOUSEHOOKSTRUCT hookStruct = (MOUSEHOOKSTRUCT)Marshal.PtrToStructure((IntPtr)lParam, typeof(MOUSEHOOKSTRUCT));
                this.rightMouseButtonDownCallback(hookStruct);
            }
            else if (nCode >= 0 && MouseMessages.WM_RBUTTONUP == (MouseMessages)wParam)
            {
                MOUSEHOOKSTRUCT hookStruct = (MOUSEHOOKSTRUCT)Marshal.PtrToStructure((IntPtr)lParam, typeof(MOUSEHOOKSTRUCT));
                this.rightMouseButtonUpCallback(hookStruct);
            }
            else if (nCode >= 0 && MouseMessages.WM_MBUTTONDOWN == (MouseMessages)wParam)
            {
                MOUSEHOOKSTRUCT hookStruct = (MOUSEHOOKSTRUCT)Marshal.PtrToStructure((IntPtr)lParam, typeof(MOUSEHOOKSTRUCT));
                this.middleMouseButtonDownCallback(hookStruct);
            }
            else if (nCode >= 0 && MouseMessages.WM_MBUTTONUP == (MouseMessages)wParam)
            {
                MOUSEHOOKSTRUCT hookStruct = (MOUSEHOOKSTRUCT)Marshal.PtrToStructure((IntPtr)lParam, typeof(MOUSEHOOKSTRUCT));
                this.middleMouseButtonUpCallback(hookStruct);
            }
            else if (nCode >= 0 && MouseMessages.WM_MOUSEWHEEL == (MouseMessages)wParam)
            {
                MOUSEHOOKSTRUCT hookStruct = (MOUSEHOOKSTRUCT)Marshal.PtrToStructure((IntPtr)lParam, typeof(MOUSEHOOKSTRUCT));
                if(wParam > 0)
                {
                    this.scrollWheelUpCallback(hookStruct);
                }
                else
                {
                    this.scrollWheelDownCallback(hookStruct);
                }
            }
            else if (nCode >= 0 && MouseMessages.WM_MOUSEMOVE == (MouseMessages)wParam)
            {
                MOUSEHOOKSTRUCT hookStruct = (MOUSEHOOKSTRUCT)Marshal.PtrToStructure((IntPtr)lParam, typeof(MOUSEHOOKSTRUCT));
                this.mouseMovedCallback(hookStruct);
            }

            return Utils.CallNextHookEx(hookID, nCode, wParam, lParam);
        }
    }
}
