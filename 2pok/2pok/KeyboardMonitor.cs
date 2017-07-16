using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using WindowsInput.Native;

namespace _2pok
{
    /// <summary>
    /// Provides an implementation for setting up os level keyboard hooks. Hooks can track events globally
    /// or locally within the application that initialized the hooks. Upon initialization, a
    /// <see cref="LocalKeyEventHandler"/> must be set up for the KeyDown and KeyUp fields to handle their
    /// respective events.
    /// </summary>
    class KeyboardMonitor
    {
        /// <summary>
        /// Retreives the state of the key as pressed or not pressed.
        /// </summary>
        /// <param name="nVirtKey">The key to check the state of.</param>
        /// <returns>If the high order bit is one the key is pressed; otherwise if it is zero.
        /// If the low order bit is one the key is toggled; otherwise it is not toggled. This applies
        /// to keys like caps, scroll, and num lock.</returns>
        [DllImport("user32.dll")]
        static public extern short GetKeyState(System.Windows.Forms.Keys nVirtKey);

        /// <summary>
        /// The delegate that defines callbacks for handling keyboard events.
        /// </summary>
        /// <param name="key">The key code for the key that pressed.</param>
        public delegate void LocalKeyEventHandler(VirtualKeyCode key);

        /// <summary>
        /// The user defined callback for handling events that are triggered when a key is pressed.
        /// </summary>
        public event LocalKeyEventHandler KeyDown;

        /// <summary>
        /// The user defined callback for handling events that are triggered when a key is released.
        /// </summary>
        public event LocalKeyEventHandler KeyUp;

        /// <summary>
        /// The id assigned after keyboard hooks have been set using 
        /// <see cref="Utils.SetWindowsHookEx(Utils.HookType, Utils.InputProc, int, int)"/>.
        /// </summary>
        private int HookID = 0;

        /// <summary>
        /// True if the hooks are set for the entire system; false if they are only set for the local application.
        /// </summary>
        private bool Global = false;

        /// <summary>
        /// Denotes if the object has been deconstructed or disposed and the hooks associated with the monitor's
        /// HookID have been unset.
        /// </summary>
        private bool IsFinalized = false;

        /// <summary>
        /// Sets our keyboard hooks when constructed.
        /// </summary>
        /// <param name="global">Denotes if the hooks should be set globally or only for the local application.</param>
        public KeyboardMonitor(bool global)
        {
            this.Global = global;

            using (Process currProcess = Process.GetCurrentProcess())
            using (ProcessModule currModule = currProcess.MainModule)
            {
                if (global)
                {
                    this.HookID = Utils.SetWindowsHookEx(Utils.HookType.WH_KEYBOARD_LL, new Utils.InputProc(KeybHookProc), Utils.GetModuleHandle(currModule.ModuleName), 0);
                }
                else
                {
                    this.HookID = Utils.SetWindowsHookEx(Utils.HookType.WH_KEYBOARD, new Utils.InputProc(KeybHookProc), Utils.GetModuleHandle(currModule.ModuleName), Utils.GetCurrentThreadId());
                }
            }
        }

        /// <summary>
        /// Unsets our hooks when the monitor is deconstructed.
        /// </summary>
        ~KeyboardMonitor()
        {
            if (!IsFinalized)
            {
                Utils.UnhookWindowsHookEx(this.HookID);
                IsFinalized = true;
            }
        }

        /// <summary>
        /// Unsets our hooks when the monitor is disposed.
        /// </summary>
        public void Dispose()
        {
            if (!IsFinalized)
            {
                Utils.UnhookWindowsHookEx(HookID);
                IsFinalized = true;
            }
        }

        /// <summary>
        /// Handles keyboard input events as the callback for keyboard hooks. 
        /// </summary>
        /// <param name="nCode">Denotes what data is in wParam and lParam. Check MSDN for information about 
        /// the values.</param>
        /// <param name="wParam">If nCode is 3, it contains the key code of the key. Othwerise it denotes the type of key event that occured.</param>
        /// <param name="lParam">If nCode is 3, it denotes if the key was pressed or released. Otherwise it contains the key code of the key.</param>
        /// <returns>Unused.</returns>
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
                        KeyDown?.Invoke((VirtualKeyCode)wParam);
                    }
                    if (keydownup == -1)
                    {
                        KeyUp?.Invoke((VirtualKeyCode)wParam);
                    }
                }
            }
            else
            {
                Utils.KeyEvents kEvent = (Utils.KeyEvents)wParam;
                Int32 vkCode = Marshal.ReadInt32((IntPtr)lParam.ToInt32());

                if (kEvent == Utils.KeyEvents.KeyDown || kEvent == Utils.KeyEvents.SKeyDown)
                {
                    KeyDown?.Invoke((VirtualKeyCode)vkCode);
                }

                if (kEvent == Utils.KeyEvents.KeyUp || kEvent == Utils.KeyEvents.SKeyUp)
                {
                    KeyUp?.Invoke((VirtualKeyCode)vkCode);
                }
            }

            return Utils.CallNextHookEx(this.HookID, nCode, wParam, lParam);
        }

        /// <summary>
        /// Gets the state of the caps lock key.
        /// </summary>
        /// <returns>True if toggled. Otherwise false.</returns>
        public static bool GetCapslock()
        {
            return Convert.ToBoolean(GetKeyState(System.Windows.Forms.Keys.CapsLock)) & true;
        }

        /// <summary>
        /// Gets the state of the num lock key.
        /// </summary>
        /// <returns>True if toggled. Otherwise false.</returns>
        public static bool GetNumlock()
        {
            return Convert.ToBoolean(GetKeyState(System.Windows.Forms.Keys.NumLock)) & true;
        }

        /// <summary>
        /// Gets the state of the scroll lock key.
        /// </summary>
        /// <returns>true if toggled. Otherwise false.</returns>
        public static bool GetScrollLock()
        {
            return Convert.ToBoolean(GetKeyState(System.Windows.Forms.Keys.Scroll)) & true;
        }
        
        /// <summary>
        /// Get the state of the shift key.
        /// </summary>
        /// <returns>True if held. Otherwise false.</returns>
        public static bool GetShiftPressed()
        {
            int state = GetKeyState(System.Windows.Forms.Keys.ShiftKey);
            if (state > 1 || state < -1) return true;
            return false;
        }

        /// <summary>
        /// Gets the state of the ctrl key.
        /// </summary>
        /// <returns>True if held. Otherwise false.</returns>
        public static bool GetCtrlPressed()
        {
            int state = GetKeyState(System.Windows.Forms.Keys.ControlKey);
            if (state > 1 || state < -1) return true;
            return false;
        }

        /// <summary>
        /// Gets the state of the alt key.
        /// </summary>
        /// <returns>True if held. Otherwise false.</returns>
        public static bool GetAltPressed()
        {
            int state = GetKeyState(System.Windows.Forms.Keys.Menu);
            if (state > 1 || state < -1) return true;
            return false;
        }
    }
}
