using System.Collections.Generic;

using WindowsInput.Native;

using _2pok.interfaces;

namespace _2pok
{
    /// <summary>
    /// Provides a simple interface for sending keyboard key press and release events over a network.
    /// </summary>
    class NetworkKeyboard : IKeyboard
    {
        /// <summary>
        /// Maintains the state of which keys on our keyboard are pressed. Helps us reduce 
        /// network traffic by not sending a key press event for a key that is already pressed.
        /// </summary>
        private HashSet<VirtualKeyCode> pressedKeys;

        /// <summary>
        /// The network interface for sending <see cref="Input"/> event data.
        /// </summary>
        private IInputClient inputSender;

        /// <summary>
        /// The keyboard monitor that sets up os level hooks for listening for keyboard events.
        /// </summary>
        private KeyboardMonitor keyboardMonitor;

        /// <summary>
        /// Constructs a new instance that monitors the client computer's physical keyboard for input events and
        /// can send them over the network as <see cref="Input"/>
        /// </summary>
        /// <param name="inputSender">The interface used for sending <see cref="Input"/> over the network.</param>
        /// <param name="keyboardMonitor">Sets up os level hooks for monitoring the keyboard.</param>
        public NetworkKeyboard(IInputClient inputSender, KeyboardMonitor keyboardMonitor)
        {
            pressedKeys = new HashSet<VirtualKeyCode>();
            this.inputSender = inputSender;

            this.keyboardMonitor = keyboardMonitor;
            this.keyboardMonitor.KeyDown += PressKey;
            this.keyboardMonitor.KeyUp += ReleaseKey;
        }

        /// <summary>
        /// Sends a key press event after verifying that the key is not already being pressed.
        /// </summary>
        /// <param name="key">The key that has been pressed.</param>
        public async void PressKey(VirtualKeyCode key)
        {
            if (!this.pressedKeys.Contains(key))
            {
                this.pressedKeys.Add(key);
                await this.inputSender.SendInputAsync(new Input(Utils.KeyEvents.KeyDown, key));
            }
        }

        /// <summary>
        /// Sends a key release event after verifying that the key is already being pressed.
        /// </summary>
        /// <param name="key">The key that has been released.</param>
        public async void ReleaseKey(VirtualKeyCode key)
        {
            if (this.pressedKeys.Contains(key))
            {
                this.pressedKeys.Remove(key);
                await this.inputSender.SendInputAsync(new Input(Utils.KeyEvents.KeyUp, key));
            }
        }
    }
}