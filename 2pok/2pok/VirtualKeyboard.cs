using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net;
using _2pok.interfaces;

namespace _2pok
{
    class VirtualKeyboard : IVirtualKeyboard
    {
        HashSet<Keys> pressedKeys;
        IInputReceiver inputReceiver;
        AsyncCallback inputHandlerCallback;
        MainWindow mainWindow;

        public VirtualKeyboard(IInputReceiver inputReceiver, MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            this.pressedKeys = new HashSet<Keys>();
            this.inputHandlerCallback = new AsyncCallback(handleInput);
            this.inputReceiver = inputReceiver;
        }

        public async void PressKey(Keys key)
        {
            this.pressedKeys.Add(key);
        }

        public async void ReleaseKey(Keys key)
        {
            this.pressedKeys.Remove(key);
        }

        private void handleInput(IAsyncResult inputResult)
        {
            IPEndPoint endpoint = ((UdpState)(inputResult.AsyncState)).endpoint;
            KeyboardInput keyboardInput = this.inputReceiver.EndReceivingKeyboardInput(inputResult, endpoint);

            this.inputReceiver.BeginReceivingInput(this.inputHandlerCallback);

            if (keyboardInput.isBeingPressed && !this.pressedKeys.Contains(keyboardInput.key))
            {
                PressKey(keyboardInput.key);
                this.mainWindow.Dispatcher.Invoke(() =>
                {
                    this.mainWindow.Host_Input_Textbox.Text += keyboardInput.key.ToString() + " was pressed." + '\n';
                });
            }
            else if(!keyboardInput.isBeingPressed && this.pressedKeys.Contains(keyboardInput.key))
            {
                ReleaseKey(keyboardInput.key);
                this.mainWindow.Dispatcher.Invoke(() =>
                {
                    this.mainWindow.Host_Input_Textbox.Text += keyboardInput.key.ToString() + " was released." + '\n';
                });
            }
        }

        public void Connect()
        {
            this.inputReceiver.BeginReceivingInput(this.inputHandlerCallback);
        }

        public void Disconnect()
        {
            this.inputReceiver.Close();
        }
    }
}
