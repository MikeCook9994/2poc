using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace _2pok
{
    class MouseMonitor
    {
        [DllImport("user32", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern int SetWindowsHookEx(Utils.HookType idHook, MouseProc lpfn, int hInstance, int threadId);

        public delegate int MouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public int x;
            public int y;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEHOOKSTRUCT {
            public Point pt;
            public IntPtr hwnd;
            public uint wHitTestCode;
            public IntPtr dwExtraInfo;
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
        private MouseProc hookCallback = null;

        bool global = false;
        bool isFinalized = false;

        public MouseMonitor(bool global)
        {
            this.global = global;
            this.hookCallback = new MouseProc(HookCallback);

            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                if (this.global)
                {
                    this.hookID = SetWindowsHookEx(Utils.HookType.WH_MOUSE_LL, this.hookCallback, Utils.GetModuleHandle(curModule.ModuleName), 0);
                }
                else
                {
                    this.hookID = SetWindowsHookEx(Utils.HookType.WH_MOUSE, this.hookCallback, Utils.GetModuleHandle(curModule.ModuleName), Utils.GetCurrentThreadId());
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
            if (MouseMessages.WM_LBUTTONDOWN == (MouseMessages)wParam)
            {
                MOUSEHOOKSTRUCT pointerData = Marshal.PtrToStructure<MOUSEHOOKSTRUCT>(lParam);
                Console.WriteLine($"point: {pointerData.pt.x}, {pointerData.pt.y}");
                //POINT point = new POINT { byteRepresentation = lParam };
                //Console.WriteLine("X: " + point.x);
                //Console.WriteLine("Y: " + point.y);
                //MOUSEHOOKSTRUCT hookStruct = (MOUSEHOOKSTRUCT)Marshal.PtrToStructure((IntPtr)lParam, typeof(MOUSEHOOKSTRUCT));
                //this.leftMouseButtonDownCallback(hookStruct);
            }
            else if (MouseMessages.WM_LBUTTONUP == (MouseMessages)wParam)
            {
                //POINT point = new POINT { byteRepresentation = lParam };
                //Console.WriteLine("X: " + point.x);
                //Console.WriteLine("Y: " + point.y);
                //MOUSEHOOKSTRUCT hookStruct = (MOUSEHOOKSTRUCT)Marshal.PtrToStructure((IntPtr)lParam, typeof(MOUSEHOOKSTRUCT));
                //this.leftMouseButtonUpCallback(hookStruct);
            }
            else if (MouseMessages.WM_RBUTTONDOWN == (MouseMessages)wParam)
            {
                //MOUSEHOOKSTRUCT hookStruct = (MOUSEHOOKSTRUCT)Marshal.PtrToStructure((IntPtr)lParam, typeof(MOUSEHOOKSTRUCT));
                //this.rightMouseButtonDownCallback(hookStruct);
            }
            else if (MouseMessages.WM_RBUTTONUP == (MouseMessages)wParam)
            {
                //MOUSEHOOKSTRUCT hookStruct = (MOUSEHOOKSTRUCT)Marshal.PtrToStructure((IntPtr)lParam, typeof(MOUSEHOOKSTRUCT));
                //this.rightMouseButtonUpCallback(hookStruct);
            }
            else if (MouseMessages.WM_MBUTTONDOWN == (MouseMessages)wParam)
            {
                //MOUSEHOOKSTRUCT hookStruct = (MOUSEHOOKSTRUCT)Marshal.PtrToStructure((IntPtr)lParam, typeof(MOUSEHOOKSTRUCT));
                //this.middleMouseButtonDownCallback(hookStruct);
            }
            else if (MouseMessages.WM_MBUTTONUP == (MouseMessages)wParam)
            {
                //MOUSEHOOKSTRUCT hookStruct = (MOUSEHOOKSTRUCT)Marshal.PtrToStructure((IntPtr)lParam, typeof(MOUSEHOOKSTRUCT));
                //this.middleMouseButtonUpCallback(hookStruct);
            }
            else if (MouseMessages.WM_MOUSEWHEEL == (MouseMessages)wParam)
            {
                //MOUSEHOOKSTRUCT hookStruct = (MOUSEHOOKSTRUCT)Marshal.PtrToStructure((IntPtr)lParam, typeof(MOUSEHOOKSTRUCT));
                //if(wParam > 0)
                //{
                //    this.scrollWheelUpCallback(hookStruct);
                //}
                //else
                //{
                //    this.scrollWheelDownCallback(hookStruct);
                //}
            }
            else if (MouseMessages.WM_MOUSEMOVE == (MouseMessages)wParam)
            {
                //POINT point = new POINT { byteRepresentation = lParam };
                //Console.WriteLine("X: " + point.x);
                //Console.WriteLine("Y: " + point.y);
                //MOUSEHOOKSTRUCT hookStruct = (MOUSEHOOKSTRUCT)Marshal.PtrToStructure((IntPtr)lParam, typeof(MOUSEHOOKSTRUCT));
                //this.mouseMovedCallback(hookStruct);
            }

            return Utils.CallNextHookEx(this.hookID, nCode, wParam, lParam);
        }
    }
}
