using WindowsInput.Native;

namespace _2pok.interfaces
{
    /// <summary>
    /// Provides in interface for simulating common keyboard inputs including key presses and releases. 
    /// </summary>
    public interface IKeyboard
    {
        /// <summary>
        /// Sends a key press event.
        /// </summary>
        /// <param name="key">the key code of the key that was pressed</param>
        void PressKey(VirtualKeyCode key);

        /// <summary>
        /// Sends a key release event.
        /// </summary>
        /// <param name="key">the key code of the key that was pressed</param>
        void ReleaseKey(VirtualKeyCode key);
    }
}
