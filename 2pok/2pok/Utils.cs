using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

namespace _2pok
{
    /// <summary>
    /// Provides a number of static utility methods and structures used through the application.
    /// </summary>
    public class Utils
    {
        /// <summary>
        /// Sets up os level hooks for events based on the parameters.
        /// </summary>
        /// <param name="idHook">The type of hooks to be set up.</param>
        /// <param name="lpfn">The callback used to handle when an event is triggered that hooks have been set up for. Follows 
        /// the form of <see cref="Utils.InputProc"/> for it's parameters.</param> 
        /// <param name="hInstance">The handle of the current module . Retreived with 
        /// <see cref="Utils.GetModuleHandle(string)"/>.</param>
        /// <param name="threadId">If zero is provided, the hooks are global, otherwise they listen for events within 
        /// the window for the specified process ID.</param>
        /// <returns>An integer ID associated with the registerd hooks. Used to unset the hooks later.</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern int SetWindowsHookEx(Utils.HookType idHook, InputProc lpfn, int hInstance, int threadId);

        /// <summary>
        /// Unsets hooks with an associated hook ID.
        /// </summary>
        /// <param name="hhk">The hook ID identifying the hooks to be unset.</param>
        /// <returns>True if the hooks were successfully unset, false otherwise.</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern bool UnhookWindowsHookEx(int hhk);

        /// <summary>
        /// Calls the next hook associated with the same hook ID listening for the same hook type.
        /// </summary>
        /// <param name="idHook">The associated hook id.</param>
        /// <param name="nCode">The nCode provided as a parameter to the lpfn callback of the form 
        /// <see cref="Utils.InputProc"/>.</param>
        /// <param name="wParam">The wParam provided as a parameter to the lpfn callback of the form 
        /// <see cref="Utils.InputProc"/>.</param>
        /// <param name="lParam">The lParam provided as a parameter to the lpfn callback of the form 
        /// <see cref="Utils.InputProc"/>.</param>
        /// <returns>Unsued.</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Retreives the current position of the mouse cursor.
        /// </summary>
        /// <param name="lpPoint">A <see cref="Utils.POINT"/> struct to fill with the current location.</param>
        /// <returns>True if the operation was successful. False otherwise.</returns>
        [DllImport("User32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern bool GetcursorPos(out Utils.POINT lpPoint);

        /// <summary>
        /// Gets the current thread ID.
        /// </summary>
        /// <returns>The current thread ID.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern int GetCurrentThreadId();

        /// <summary>
        /// Retreives the module handle given a module name.
        /// </summary>
        /// <param name="lpModuleName">The name of the module the handle is to be retreived for.</param>
        /// <returns>The handle associated with the provided module name.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern int GetModuleHandle(string lpModuleName);

        /// <summary>
        /// Defines the delegate that is used as a callback for 
        /// <see cref="Utils.SetWindowsHookEx(HookType, InputProc, int, int)"/>.
        /// </summary>
        /// <param name="nCode">Provides information about the values contained in wParam and lParam.</param>
        /// <param name="wParam">Contains additional information about the event. Depends on the type of hook and the 
        /// event that occured.</param>
        /// <param name="lParam">Contains additional information about the event. Depends on the type of hook and the 
        /// event that occured.</param>
        /// <returns>Unused. For convention and potentially correc implementation, the implementation should return 
        /// the result of <see cref="Utils.CallNextHookEx(int, int, IntPtr, IntPtr)"/> so additional hooks are notified
        /// of the event.</returns>
        public delegate int InputProc(int nCode, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// The values for some of the hook types that can be set up.
        /// </summary>
        public enum HookType : int
        {
            WH_JOURNALRECORD = 0,
            WH_JOURNALPLAYBACK = 1,
            WH_KEYBOARD = 2,
            WH_GETMESSAGE = 3,
            WH_CALLWNDPROC = 4,
            WH_CBT = 5,
            WH_SYSMSGFILTER = 6,
            WH_MOUSE = 7,
            WH_HARDWARE = 8,
            WH_DEBUG = 9,
            WH_SHELL = 10,
            WH_FOREGROUNDIDLE = 11,
            WH_CALLWNDPROCRET = 12,
            WH_KEYBOARD_LL = 13,
            WH_MOUSE_LL = 14
        }

        /// <summary>
        /// The values for some of the keyboard events that can be returned through mouse hooks.
        /// </summary>
        public enum MouseMessages : int
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

        /// <summary>
        /// The values for some of the keyboard events that can be returned through keyboard hooks.
        /// </summary>
        public enum KeyEvents : int
        {
            KeyDown = 0x0100,
            KeyUp = 0x0101,
            SKeyDown = 0x0104,
            SKeyUp = 0x0105
        }

        /// <summary>
        /// Represents an point on the screen as an xy-coordinate
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public int x;
            public int y;
        };

        /// <summary>
        /// The structure passed as a parameter into the callback for mouse hooks.
        /// Pt represents the position of the cursor as an xy-coordinate.
        /// MouseData contains additional data about the event depending on which event occurs. For example, 
        /// if the event is scroll event, it will denote the amount the wheel was scrolled. 
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public int mouseData;
            public int flags;
            public int time;
            public int dwExtraInfo;
        };

        /// <summary>
        /// Serializes an object into an array of bytes.
        /// </summary>
        /// <param name="obj">The object to be serialized.</param>
        /// <returns>The array of bytes representing the serialized object.</returns>
        public static byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Deserializes an array of bytes into an object.
        /// </summary>
        /// <param name="arrBytes">The array of bytes to be deserialized.</param>
        /// <returns>The deserialized object.</returns>
        public static Object ByteArrayToObject(byte[] arrBytes)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                Object obj = binForm.Deserialize(memStream);
                return obj;
            }
        }
    }
}
