using System;
using System.Collections.Generic;
using System.Net;

using WindowsInput;
using WindowsInput.Native;

using _2pok.interfaces;

namespace _2pok
{
    /// <summary>
    /// Provides and implementation for simulating keyboard input events via the <see cref="IInputSimulator"/>
    /// interface.
    /// </summary>
    class VirtualKeyboard : IKeyboard
    {
        /// <summary>
        /// Maintains the state of which keys on our keyboard are pressed. Helps us reduce 
        /// network traffic by not sending a key press event for a key that is already pressed.
        /// </summary>
        private HashSet<VirtualKeyCode> pressedKeys;

        /// <summary>
        /// The network interface for receiving <see cref="Input"/>.
        /// </summary>
        private IInputHost inputReceiver;

        /// <summary>
        /// The interface used for simulating input events on the host machine.
        /// </summary>
        private IInputSimulator inputSimulator;

        /// <summary>
        /// The callback that handles keyboard input events when they are received over the network.
        /// </summary>
        private AsyncCallback inputHandlerCallback;

        /// <summary>
        /// The main window of the GUI so elements of the interface can be modified. This is needed because we
        /// have no other way of calling back to the main thread that the GUI runs on to modify the interface.
        /// </summary>
        private MainWindow mainWindow;

        /// <summary>
        /// Constructs a new instance that can receive keyboard <see cref="Input"/> events over the network, simulate them 
        /// on the host machine and notify the user of those events via the GUI.
        /// </summary>
        /// <param name="inputReceiver">The interface used for receiving <see cref="Input"/> over the network.</param>
        /// <param name="inputSimulator">The interface used for simulating keyboard input on the host's computer.</param>
        /// <param name="mainWindow">The main window of the GUI.</param>
        public VirtualKeyboard(IInputHost inputReceiver, IInputSimulator inputSimulator, MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            this.pressedKeys = new HashSet<VirtualKeyCode>();
            this.inputHandlerCallback = new AsyncCallback(handleInput);
            this.inputSimulator = inputSimulator;

            this.inputReceiver = inputReceiver;
            
            // This call is necessary to begin receiving input asynchonously.
            this.inputReceiver.BeginReceivingInput(this.inputHandlerCallback);
        }

        /// <summary>
        /// Simulates a key press event on the host machine.
        /// </summary>
        /// <param name="key">The key to be pressed.</param>
        public void PressKey(VirtualKeyCode key)
        {
            this.pressedKeys.Add(key);
            this.inputSimulator.Keyboard.KeyDown(key);
        }

        /// <summary>
        /// Simulates a key release event on the hose machine.
        /// </summary>
        /// <param name="key">The key to be released.</param>
        public void ReleaseKey(VirtualKeyCode key)
        {
            this.pressedKeys.Remove(key);
            this.inputSimulator.Keyboard.KeyUp(key);
        }

        /// <summary>
        /// The callback function that is called when input is received from the client. It receives the event by
        /// calling <see cref="IInputHost.EndReceivingInput(IAsyncResult, IPEndPoint)"/> and then resumes waiting for
        /// <see cref="Input"/> asynchronously by again calling <see cref="IInputHost.BeginReceivingInput(AsyncCallback)"/>.
        /// Upon receiving the <see cref="Input"/> event data, the key press or release is simualted and the GUI is updated.
        /// </summary>
        /// <param name="inputResult">Contains state information about the connection and data received.</param>
        private void handleInput(IAsyncResult inputResult)
        {
            IPEndPoint endpoint = ((UdpState)(inputResult.AsyncState)).endpoint;
            Input keyboardInput = this.inputReceiver.EndReceivingInput(inputResult, endpoint);

            this.inputReceiver.BeginReceivingInput(this.inputHandlerCallback);

            Console.WriteLine($"{keyboardInput.key} {keyboardInput.keyEvent}{Environment.NewLine}");
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

        /// <summary>
        /// Updates the GUI with information about the <see cref="Input"/> event received.
        /// </summary>
        /// <param name="keyboardInput">The <see cref="Input"/> event data received.</param>
        private void PostKeyboardEventMessageToGUI(Input keyboardInput)
        {
            this.mainWindow.Dispatcher.Invoke(() =>
            {
                this.mainWindow.Host_Input_Textbox.Text += $"{keyboardInput.key.ToString()} was {((keyboardInput.keyEvent == Utils.KeyEvents.KeyDown) ? ("pressed") : ("released"))}{Environment.NewLine}";
            });
        }
    }
}
