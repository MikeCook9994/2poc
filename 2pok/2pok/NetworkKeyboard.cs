using System.Collections.Generic;
using System.Windows.Forms;
using System.Net;
using _2pok.interfaces;

namespace _2pok
{
    class NetworkKeyboard : INetworkKeyboard
    {
        HashSet<Keys> pressedKeys;
        IInputSender inputSender;
        KeyboardMonitor keyboardMonitor;

        public NetworkKeyboard(IInputSender inputSender, KeyboardMonitor keyboardMonitor)
        {
            pressedKeys = new HashSet<Keys>();
            this.inputSender = inputSender;

            this.keyboardMonitor = keyboardMonitor;
            this.keyboardMonitor.KeyDown += PressKey;
            this.keyboardMonitor.KeyUp += ReleaseKey;
        }

        public async void PressKey(Keys key)
        {
            KeyboardInput keyboardInput = new KeyboardInput(key, true);
            int bytesSent = await this.inputSender.SendKeyboardInputAsync(keyboardInput);
        }

        public async void ReleaseKey(Keys key)
        {
            KeyboardInput keyboardInput = new KeyboardInput(key, false);
            int bytesSent = await this.inputSender.SendKeyboardInputAsync(keyboardInput);
        }

        public void Connect(IPAddress hostIp, int portNumber)
        {
            this.inputSender.Connect(hostIp, portNumber);
            Application.Run();
        }

        public void Disconnect()
        {
            this.inputSender.Close();
        }
    }
}