using System.Collections.Generic;
using System.Threading.Tasks;
using _2pok.interfaces;
using WindowsInput.Native;

namespace _2pok
{
    class NetworkKeyboard : IKeyboard
    {
        HashSet<VirtualKeyCode> pressedKeys;
        IInputSender inputSender;
        KeyboardMonitor keyboardMonitor;

        public NetworkKeyboard(IInputSender inputSender, KeyboardMonitor keyboardMonitor)
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
                await SendKeyEventAsync(key, true);
            }
        }

        public async void ReleaseKey(VirtualKeyCode key)
        {
            if (this.pressedKeys.Contains(key))
            {
                this.pressedKeys.Remove(key);
                await SendKeyEventAsync(key, false);
            }
        }

        private async Task SendKeyEventAsync(VirtualKeyCode key, bool isKeyPress)
        {
            KeyboardInput keyboardInput = new KeyboardInput(key, isKeyPress);
            await this.inputSender.SendKeyboardInputAsync(keyboardInput);
        }
    }
}