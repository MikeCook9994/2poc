using System;
using System.Runtime.InteropServices;
using WindowsInput.Native;

namespace _2pok
{
    /// <summary>
    /// A union (C# style) that can contain input data for key and mouse events. Contains fields that explicitly allow 
    /// for key presses, key releases, mouse scrolls, mouse movements, and mouse button presses. 
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Explicit)]
    public struct Input
    {
        /// <summary>
        /// Constructor to be used for a standard mouse button press. The first field contains the event type. The 
        /// second field is set to the minimum value for an integer as a default value.
        /// </summary>
        /// <param name="eventType">The mouse event that occurred. See <see cref="Utils.MouseMessages>"/> for a 
        /// culled list of standard mouse messages</param>
        public Input(Utils.MouseMessages eventType)
        {
            this.key = (VirtualKeyCode)int.MinValue;
            this.eventType = eventType;

            this.keyEvent = (Utils.KeyEvents)int.MinValue;
            this.point = new Utils.POINT(int.MinValue, int.MinValue);
            this.wheelDelta = int.MinValue;
        }

        /// <summary>
        /// Constructor to be used with a mouse movement event. The first field contains the event type. The 
        /// second field contains the location of the mouse as an (X, Y) coordinate point. <see cref="Utils.POINT"/>
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="point"></param>
        public Input(Utils.MouseMessages eventType, Utils.POINT point)
        {
            this.key = (VirtualKeyCode)int.MinValue;
            this.eventType = eventType;

            this.keyEvent = (Utils.KeyEvents)int.MinValue;
            this.wheelDelta = int.MinValue;
            this.point = point;
        }

        public Input(Utils.MouseMessages eventType, int wheelDelta)
        {
            this.key = (VirtualKeyCode)int.MinValue;
            this.eventType = eventType;

            this.keyEvent = (Utils.KeyEvents)int.MinValue;
            this.point = new Utils.POINT(int.MinValue, int.MinValue);
            this.wheelDelta = wheelDelta;
        }

        public Input(VirtualKeyCode key, Utils.KeyEvents keyEvent)
        {
            this.key = key;
            this.eventType = (Utils.MouseMessages)int.MinValue;

            this.point = new Utils.POINT(int.MinValue, int.MinValue);
            this.wheelDelta = int.MinValue;
            this.keyEvent = keyEvent;
        }

        [FieldOffset(0)]
        public Utils.MouseMessages eventType;

        [FieldOffset(0)]
        public VirtualKeyCode key;

        [FieldOffset(4)]
        public Utils.POINT point;

        [FieldOffset(4)]
        public int wheelDelta;

        [FieldOffset(4)]
        public Utils.KeyEvents keyEvent;
    }
}
