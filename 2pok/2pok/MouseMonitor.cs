using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace _2pok
{
    class MouseMonitor
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        private enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205
        }

        private int hookID = 0;
        private Utils.CallbackDelegate hookCallback = null;

        private const int WH_MOUSE_LL = 14;

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
                    this.hookID = Utils.SetWindowsHookEx(Utils.HookType.WH_KEYBOARD_LL, this.hookCallback, Utils.GetModuleHandle(curModule.ModuleName), 0);
                }
                else
                {
                    hookID = Utils.SetWindowsHookEx(Utils.HookType.WH_KEYBOARD, this.hookCallback, 0, Utils.GetCurrentThreadId());
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
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure((IntPtr)lParam, typeof(MSLLHOOKSTRUCT));
                Console.WriteLine(hookStruct.pt.x + ", " + hookStruct.pt.y);
            }
            return Utils.CallNextHookEx(hookID, nCode, wParam, lParam);
        }
    }
}
