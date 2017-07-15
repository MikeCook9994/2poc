using System;
using System.Collections.Generic;
using System.Net;

using WindowsInput;
using WindowsInput.Native;

using _2pok.interfaces;

namespace _2pok
{
    class VirtualKeyboard : IKeyboard
    {
        HashSet<VirtualKeyCode> pressedKeys;
        IInputHost inputReceiver;
        IInputSimulator inputSimulator;
        AsyncCallback inputHandlerCallback;
        MainWindow mainWindow;

        public VirtualKeyboard(IInputHost inputReceiver, IInputSimulator inputSimulator, MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            this.pressedKeys = new HashSet<VirtualKeyCode>();
            this.inputHandlerCallback = new AsyncCallback(handleInput);
            this.inputSimulator = inputSimulator;

            this.inputReceiver = inputReceiver;
            this.inputReceiver.BeginReceivingInput(this.inputHandlerCallback);
        }

        public void PressKey(VirtualKeyCode key)
        {
            this.pressedKeys.Add(key);
            this.inputSimulator.Keyboard.KeyDown(key);
        }

        public void ReleaseKey(VirtualKeyCode key)
        {
            this.pressedKeys.Remove(key);
            this.inputSimulator.Keyboard.KeyUp(key);
        }

        private void handleInput(IAsyncResult inputResult)
        {
            IPEndPoint endpoint = ((UdpState)(inputResult.AsyncState)).endpoint;
            Input keyboardInput = this.inputReceiver.EndReceivingInput(inputResult, endpoint);

            this.inputReceiver.BeginReceivingInput(this.inputHandlerCallback);

            if ((keyboardInput.keyEvent == Utils.KeyEvents.KeyUp) && !this.pressedKeys.Contains(keyboardInput.key))
            {
                PressKey(keyboardInput.key);
                PostKeyboardEventMessageToGUI(keyboardInput);
            }
            else if ((keyboardInput.keyEvent == Utils.KeyEvents.KeyDown) && this.pressedKeys.Contains(keyboardInput.key))
            {
                ReleaseKey(keyboardInput.key);
                PostKeyboardEventMessageToGUI(keyboardInput);
            }
        }

        private void PostKeyboardEventMessageToGUI(Input keyboardInput)
        {
            this.mainWindow.Dispatcher.Invoke(() =>
            {
                this.mainWindow.Host_Input_Textbox.Text += $"{keyboardInput.key.ToString()} was {((keyboardInput.keyEvent == Utils.KeyEvents.KeyDown) ? ("pressed") : ("released"))}{Environment.NewLine}";
            });
        }
    }
}
