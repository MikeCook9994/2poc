using System;
using System.Windows.Forms;

namespace _2pok
{
    [Serializable]
    public class KeyboardInput
    {
        public Keys key { get; set; }
        public bool isBeingPressed { get; set; }

        public KeyboardInput(Keys key, bool pressedState)
        {
            this.key = key;
            this.isBeingPressed = pressedState;
        }
    }
}
