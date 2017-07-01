using System;
using WindowsInput.Native;

namespace _2pok
{
    [Serializable]
    public class KeyboardInput
    {
        public VirtualKeyCode key { get; set; }
        public bool isBeingPressed { get; set; }

        public KeyboardInput(VirtualKeyCode key, bool pressedState)
        {
            this.key = key;
            this.isBeingPressed = pressedState;
        }
    }
}
