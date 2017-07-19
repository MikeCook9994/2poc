using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace _2pok
{
    /// <summary>
    /// Provides an implementation for setting up os level mouse hooks. Hooks can track events globally
    /// or locally within the application that initialized the hooks. Upon initialization, a
    /// callbacks must be set up for the following fields to various fields to handle their respective events:
    /// 
    /// +----------- Field Name ------------+------------- Delegate Type -------------+
    /// | 1) leftMouseButtonUpCallback      | <see cref="MouseClickEventHandler"/>    |
    /// | 2) leftMouseButtonDownCallback    | <see cref="MouseClickEventHandler"/>    |
    /// | 3) rightMouseButtonUpCallback     | <see cref="MouseClickEventHandler"/>    |
    /// | 4) rightMouseButtonDownCallback   | <see cref="MouseClickEventHandler"/>    |
    /// | 5) middleMouseButtonUpCallback    | <see cref="MouseClickEventHandler"/>    |
    /// | 6) middleMouseButtonDownCallback  | <see cref="MouseClickEventHandler"/>    |
    /// | 7) scrollMouseWheelCallback       | <see cref="MouseScrollEventHandler"/>   |
    /// | 8) moveMouseCallback              | <see cref="MouseMovementEventHandler"/> |
    /// +-----------------------------------+-----------------------------------------+
    /// </summary>
    public class MouseMonitor
    {
        /// <summary>
        /// The delegate that defines callbacks for handling mouse movement events.
        /// </summary>
        /// <param name="point">The current position of the mouse pointer represented as an xy-coordinate.</param>
        public delegate void MouseMovementEventHandler(Utils.POINT point);

        /// <summary>
        /// The delegate that defines callbacks for handling mouse scroll events.
        /// </summary>
        /// <param name="wheelDelta">Represents the amount to scroll in clicks as a multiple of 120. Positive values 
        /// represent that the mouse wheel has been scrolled up, away from the user. Negative wheels represent that the
        /// wheel has been scrolled down, toward the user.</param>
        public delegate void MouseScrollEventHandler(int wheelDelta);

        /// <summary>
        /// The delegate that defines callbacks for handling mouse button click events.
        /// </summary>
        public delegate void MouseClickEventHandler();

        /// <summary>
        /// The user defined callback for handling left mouse button up events.
        /// </summary>
        public MouseClickEventHandler leftMouseButtonUpCallback;

        /// <summary>
        /// The user defined callback for handling left mouse button down events.
        /// </summary>
        public MouseClickEventHandler leftMouseButtonDownCallback;

        /// <summary>
        /// The user defined callback for handling right mouse button up events.
        /// </summary>
        public MouseClickEventHandler rightMouseButtonUpCallback;

        /// <summary>
        /// The user defined callback for handling right mouse button down events.
        /// </summary>
        public MouseClickEventHandler rightMouseButtonDownCallback;

        /// <summary>
        /// The user defined callback for handling middle mouse button up events.
        /// </summary>
        public MouseClickEventHandler middleMouseButtonUpCallback;

        /// <summary>
        /// The user defined callback for handling middle mouse button down events.
        /// </summary>
        public MouseClickEventHandler middleMouseButtonDownCallback;

        /// <summary>
        /// The user defined callback for handling mouse scroll events.
        /// </summary>
        public MouseScrollEventHandler scrollMouseWheelCallback;

        /// <summary>
        /// The user defined callback for handling mouse movement events.
        /// </summary>
        public MouseMovementEventHandler moveMouseCallback;

        /// <summary>
        /// The id assigned after keyboard hooks have been set using 
        /// <see cref="Utils.SetWindowsHookEx(Utils.HookType, Utils.InputProc, int, int)"/>.
        /// </summary>
        private int hookID = 0;

        /// <summary>
        /// True if the hooks are set for the entire system; false if they are only set for the local application.
        /// </summary>
        bool Global;

        /// <summary>
        /// Denotes if the object has been deconstructed or disposed and the hooks associated with the monitor's
        /// HookID have been unset.
        /// </summary>
        bool IsFinalized = false;

        /// <summary>
        /// The instantiated delegate of the callback for our hooks. We need this to keep the delegate from getting
        /// garbage collected.
        /// </summary>
        private Utils.InputProc HookCallback = null;

        /// <summary>
        /// Sets our mouse hooks when constructed.
        /// </summary>
        /// <param name="global">Denotes if the hooks should be set globally or only for the local application.</param>
        public MouseMonitor(bool global)
        {
            this.Global = global;
            this.HookCallback = new Utils.InputProc(MouseHookCallback);

            using (Process currProcess = Process.GetCurrentProcess())
            using (ProcessModule currModule = currProcess.MainModule)
            {
                if (this.Global)
                {
                    this.hookID = Utils.SetWindowsHookEx(Utils.HookType.WH_MOUSE_LL, this.HookCallback, Utils.GetModuleHandle(currModule.ModuleName), 0);
                }
                else
                {
                    this.hookID = Utils.SetWindowsHookEx(Utils.HookType.WH_MOUSE, this.HookCallback, Utils.GetModuleHandle(currModule.ModuleName), Utils.GetCurrentThreadId());
                }
            }
        }

        /// <summary>
        /// Unsets our hooks when the monitor is deconstructed.
        /// </summary>
        ~MouseMonitor()
        {
            if (!IsFinalized)
            {
                Utils.UnhookWindowsHookEx(this.hookID);
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
                Utils.UnhookWindowsHookEx(this.hookID);
                IsFinalized = true;
            }
        }

        /// <summary>
        /// Handles mouse input events as the callback for mouse hooks. 
        /// </summary>
        /// <param name="nCode">Defines what contents wParam and lParam contain based on it's value.</param>
        /// <param name="wParam">If nCode is zero or greater, it contains the type of event
        /// (<see cref="Utils.MouseMessages"/>).</param>
        /// <param name="lParam">If nCode is zero or greater, it contains additional information stored in a 
        /// <see cref="Utils.MSLLHOOKSTRUCT"/> such as the position of the mouse cursor or the amount the
        /// mouse wheel was scrolled.</param>
        /// <returns>Unused.</returns>
        private int MouseHookCallback(int nCode,  IntPtr wParam, IntPtr lParam)
        {
            // For all of these events, scrolling and mouse movement are the only ones that we need additional
            // information for. All other cases, just knowing the event occured is sufficient to handle it.
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
                    this.scrollMouseWheelCallback(hookStruct.mouseData);
                }
                else if (Utils.MouseMessages.WM_MOUSEMOVE == (Utils.MouseMessages)wParam)
                {
                    Utils.MSLLHOOKSTRUCT hookStruct = Marshal.PtrToStructure<Utils.MSLLHOOKSTRUCT>(lParam);
                    this.moveMouseCallback(hookStruct.pt);
                }
            }
            return Utils.CallNextHookEx(this.hookID, nCode, wParam, lParam);
        }

        /// <summary>
        /// Retrieves the current position of the cursor.
        /// </summary>
        /// <returns>The current value of the cursor represented as an xy-coordinate.</returns>
        public static Utils.POINT GetCursorPosition()
        {
            Utils.POINT lpPoint;
            Utils.GetCursorPos(out lpPoint);
            return lpPoint;
        }
    }
}
