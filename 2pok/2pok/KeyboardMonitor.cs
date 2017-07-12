using System;
using System.Runtime.InteropServices;
using WindowsInput.Native;

namespace _2pok
{
    class KeyboardMonitor
    {
        [DllImport("user32.dll")]
        static public extern short GetKeyState(System.Windows.Forms.Keys nVirtKey);

        private enum KeyEvents
        {
            KeyDown = 0x0100,
            KeyUp = 0x0101,
            SKeyDown = 0x0104,
            SKeyUp = 0x0105
        }

        public delegate void LocalKeyEventHandler(VirtualKeyCode key);
        public event LocalKeyEventHandler KeyDown;
        public event LocalKeyEventHandler KeyUp;

        private int HookID = 0;
        Utils.InputProc TheHookCB = null;

        bool IsFinalized = false;
        bool Global = false;

        public KeyboardMonitor(bool Global)
        {
            this.Global = Global;
            this.TheHookCB = new Utils.InputProc(KeybHookProc);

            if (Global)
            {
                HookID = Utils.SetWindowsHookEx(Utils.HookType.WH_KEYBOARD_LL, this.TheHookCB, 0, 0); 
            }
            else
            {
                HookID = Utils.SetWindowsHookEx(Utils.HookType.WH_KEYBOARD, this.TheHookCB, 0, Utils.GetCurrentThreadId()); 
            }
        }

        ~KeyboardMonitor()
        {
            if (!IsFinalized)
            {
                Utils.UnhookWindowsHookEx(HookID);
                IsFinalized = true;
            }
        }

        public void Dispose()
        {
            if (!IsFinalized)
            {
                Utils.UnhookWindowsHookEx(HookID);
                IsFinalized = true;
            }
        }

        private int KeybHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
            {
                return Utils.CallNextHookEx(this.HookID, nCode, wParam, lParam);
            }

            if (!Global)
            {
                if (nCode == 3)
                {
                    IntPtr ptr = IntPtr.Zero;
                    int keydownup = lParam.ToInt32() >> 30;

                    if (keydownup == 0)
                    {
                        if (KeyDown != null) KeyDown((VirtualKeyCode)wParam);
                    }
                    if (keydownup == -1)
                    {
                        if (KeyUp != null) KeyUp((VirtualKeyCode)wParam);
                    }
                }
            }
            else
            {
                KeyEvents kEvent = (KeyEvents)wParam;
                Int32 vkCode = Marshal.ReadInt32((IntPtr)lParam.ToInt32());

                if (kEvent == KeyEvents.KeyDown || kEvent == KeyEvents.SKeyDown)
                {
                    if (KeyDown != null) KeyDown((VirtualKeyCode)vkCode);
                }

                if (kEvent == KeyEvents.KeyUp || kEvent == KeyEvents.SKeyUp)
                {
                    if (KeyUp != null) KeyUp((VirtualKeyCode)vkCode);
                }
            }

            return Utils.CallNextHookEx(this.HookID, nCode, wParam, lParam);
        }

        public static bool GetCapslock()
        {
            return Convert.ToBoolean(GetKeyState(System.Windows.Forms.Keys.CapsLock)) & true;
        }

        public static bool GetNumlock()
        {
            return Convert.ToBoolean(GetKeyState(System.Windows.Forms.Keys.NumLock)) & true;
        }

        public static bool GetScrollLock()
        {
            return Convert.ToBoolean(GetKeyState(System.Windows.Forms.Keys.Scroll)) & true;
        }

        public static bool GetShiftPressed()
        {
            int state = GetKeyState(System.Windows.Forms.Keys.ShiftKey);
            if (state > 1 || state < -1) return true;
            return false;
        }

        public static bool GetCtrlPressed()
        {
            int state = GetKeyState(System.Windows.Forms.Keys.ControlKey);
            if (state > 1 || state < -1) return true;
            return false;
        }

        public static bool GetAltPressed()
        {
            int state = GetKeyState(System.Windows.Forms.Keys.Menu);
            if (state > 1 || state < -1) return true;
            return false;
        }
    }
}
