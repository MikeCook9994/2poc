using System.Collections.Generic;
using System.Threading.Tasks;

using WindowsInput.Native;

using _2pok.interfaces;

namespace _2pok
{
    class NetworkKeyboard : IKeyboard
    {
        HashSet<VirtualKeyCode> pressedKeys;
        IInputClient inputSender;
        KeyboardMonitor keyboardMonitor;

        public NetworkKeyboard(IInputClient inputSender, KeyboardMonitor keyboardMonitor)
        {
            pressedKeys = new HashSet<VirtualKeyCode>();
            this.inputSender = inputSender;

            this.keyboardMonitor = keyboardMonitor;
            this.keyboardMonitor.KeyDown += PressKey;
            this.keyboardMonitor.KeyUp += ReleaseKey;
        }

        public async void PressKey(VirtualKeyCode key)
        {
            if (!this.pressedKeys.Contains(key))
            {
                this.pressedKeys.Add(key);
                await this.inputSender.SendInputAsync(new Input(key, Utils.KeyEvents.KeyDown));
            }
        }

        public async void ReleaseKey(VirtualKeyCode key)
        {
            if (this.pressedKeys.Contains(key))
            {
                this.pressedKeys.Remove(key);
                await this.inputSender.SendInputAsync(new Input(key, Utils.KeyEvents.KeyUp));
            }
        }
    }
}