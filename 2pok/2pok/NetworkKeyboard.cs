using System.Collections.Generic;
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
            if(!this.pressedKeys.Contains(key))
            {
                this.pressedKeys.Add(key);
                KeyboardInput keyboardInput = new KeyboardInput(key, true);
                int bytesSent = await this.inputSender.SendKeyboardInputAsync(keyboardInput);
            }
        }

        public async void ReleaseKey(VirtualKeyCode key)
        {
            if(this.pressedKeys.Contains(key))
            {
                this.pressedKeys.Remove(key);
                KeyboardInput keyboardInput = new KeyboardInput(key, false);
                int bytesSent = await this.inputSender.SendKeyboardInputAsync(keyboardInput);
            }
        }
    }
}