using System;
using System.Collections.Generic;
using System.Net;
using _2pok.interfaces;
using WindowsInput;
using WindowsInput.Native;

namespace _2pok
{
    class VirtualKeyboard : IKeyboard
    {
        HashSet<VirtualKeyCode> pressedKeys;
        IInputReceiver inputReceiver;
        IInputSimulator inputSimulator;
        AsyncCallback inputHandlerCallback;
        MainWindow mainWindow;

        public VirtualKeyboard(IInputReceiver inputReceiver, IInputSimulator inputSimulator, MainWindow mainWindow)
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
            KeyboardInput keyboardInput = this.inputReceiver.EndReceivingKeyboardInput(inputResult, endpoint);

            this.inputReceiver.BeginReceivingInput(this.inputHandlerCallback);

            if (keyboardInput.isBeingPressed && !this.pressedKeys.Contains(keyboardInput.key))
            {
                PressKey(keyboardInput.key);
                PostKeyboardEventMessageToGUI(keyboardInput);
            }
            else if (!keyboardInput.isBeingPressed && this.pressedKeys.Contains(keyboardInput.key))
            {
                ReleaseKey(keyboardInput.key);
                PostKeyboardEventMessageToGUI(keyboardInput);
            }
        }

        private void PostKeyboardEventMessageToGUI(KeyboardInput keyboardInput)
        {
            this.mainWindow.Dispatcher.Invoke(() =>
            {
                this.mainWindow.Host_Input_Textbox.Text += $"{keyboardInput.key.ToString()} was {((keyboardInput.isBeingPressed) ? ("pressed") : ("released"))}{Environment.NewLine}";
            });
        }
    }
}
