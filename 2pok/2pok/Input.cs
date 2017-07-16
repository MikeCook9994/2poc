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
        /// Constructor to be used for a standard mouse button press with no additional data. The second field is set 
        /// to the minimum value for an integer as a default value.
        /// </summary>
        /// <param name="mouseEvent">The mouse event that occurred. See <see cref="Utils.MouseMessages>"/> for a 
        /// culled list of standard mouse messages.</param>
        public Input(Utils.MouseMessages mouseEvent)
        {
            this.keyEvent = (Utils.KeyEvents)int.MinValue;
            this.mouseEvent = mouseEvent;

            this.key = (VirtualKeyCode)int.MinValue;
            this.point = new Utils.POINT(int.MinValue, int.MinValue);
            this.wheelDelta = int.MinValue;
        }

        /// <summary>
        /// Constructor to be used with a mouse movement event.
        /// </summary>
        /// <param name="mouseEvent">The mouse event that occurred. See <see cref="Utils.MouseMessages>"/> for a 
        /// culled list of standard mouse messages.</param>
        /// <param name="point">The location of the mouse as an xy-coordinate point (<see cref="Utils.POINT"/>).
        /// </param>
        public Input(Utils.MouseMessages mouseEvent, Utils.POINT point)
        {
            this.keyEvent = (Utils.KeyEvents)int.MinValue;
            this.mouseEvent = mouseEvent;

            this.key = (VirtualKeyCode)int.MinValue;
            this.wheelDelta = int.MinValue;
            this.point = point;
        }

        /// <summary>
        /// Constructor to be used with a scroll wheel mouse event.
        /// </summary>
        /// <param name="mouseEvent">The mouse event that occurred. See <see cref="Utils.MouseMessages>"/> for a 
        /// culled list of standard mouse messages.</param>
        /// <param name="wheelDelta">Represents the amount to scroll in clicks as a multiple of 120. Positive values 
        /// represent that the mouse wheel has been scrolled up, away from the user. Negative wheels represent that the
        /// wheel has been scrolled down, toward the user.</param>
        public Input(Utils.MouseMessages mouseEvent, int wheelDelta)
        {
            this.keyEvent = (Utils.KeyEvents)int.MinValue;
            this.mouseEvent = mouseEvent;

            this.key = (VirtualKeyCode)int.MinValue;
            this.point = new Utils.POINT(int.MinValue, int.MinValue);
            this.wheelDelta = wheelDelta;
        }

        /// <summary>
        /// Constructor to be used with a standard keyboard event.
        /// </summary>
        /// <param name="keyEvent">The type of key event that occursed (<see cref="Utils.KeyEvents"/>).</param>
        /// <param name="key">The key code of the key that was pressed.</param>
        public Input(Utils.KeyEvents keyEvent, VirtualKeyCode key)
        {
            this.mouseEvent = (Utils.MouseMessages)int.MinValue;
            this.keyEvent = keyEvent;

            this.point = new Utils.POINT(int.MinValue, int.MinValue);
            this.wheelDelta = int.MinValue;
            this.key = key;
        }

        /// <summary>
        /// The type of mouse event that has occurred. This will either provide enough information by itself or will
        /// only ever be packaged with the point field or (exclusively) the wheelDelta field.
        /// </summary>
        [FieldOffset(0)]
        public Utils.MouseMessages mouseEvent;

        /// <summary>
        /// The type of keyboard even that has occurred. This will always be packaged with the key field.
        /// </summary>
        [FieldOffset(0)]
        public Utils.KeyEvents keyEvent;

        /// <summary>
        /// The location of the mouse as an xy-coordinate stored as a struct (<see cref="Utils.POINT"/>). This 
        /// will always be packaged with the mouseEvent field.
        /// </summary>
        [FieldOffset(4)]
        public Utils.POINT point;

        /// <summary>
        /// Represents the amount to scroll in clicks as a multiple of 120. Positive values 
        /// represent that the mouse wheel has been scrolled up, away from the user. Negative wheels represent that the
        /// wheel has been scrolled down, toward the user. This will always be packaged with the mouseEvent field.
        /// </summary>
        [FieldOffset(4)]
        public int wheelDelta;

        /// <summary>
        /// The type of key event that occursed (<see cref="Utils.KeyEvents"/>). This will always be packaged with the
        /// keyEvent field.
        /// </summary>
        [FieldOffset(4)]
        public VirtualKeyCode key;
    }
}
