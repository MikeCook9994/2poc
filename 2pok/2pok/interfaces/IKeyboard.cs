using WindowsInput.Native;

namespace _2pok.interfaces
{
    interface IKeyboard
    {
        void PressKey(VirtualKeyCode key);

        void ReleaseKey(VirtualKeyCode key);
    }
}
