using System;
using System.Net;

using WindowsInput;

using _2pok.interfaces;

namespace _2pok
{
    /// <summary>
    /// Provides and implementation for simulating mouse input events via the <see cref="IInputSimulator"/>
    /// interface.
    /// </summary>
    public class VirtualMouse : IMouse
    {
        /// <summary>
        /// The main window of the GUI so elements of the interface can be modified. This is needed because we
        /// have no other way of calling back to the main thread that the GUI runs on to modify the interface.
        /// </summary>
        MainWindow mainWindow;

        /// <summary>
        /// The callback that handles keyboard input events when they are received over the network.
        /// </summary>
        AsyncCallback inputHandlerCallback;

        /// <summary>
        /// The interface used for simulating input events on the host machine.
        /// </summary>
        IInputSimulator inputSimulator;

        /// <summary>
        /// The network interface for receiving <see cref="Input"/>.
        /// </summary>
        IInputHost inputReceiver;

        /// <summary>
        /// Constructs a new instance that can receive mouse <see cref="Input"/> events over the network, simulate them 
        /// on the host machine and notify the user of those events via the GUI.
        /// </summary>
        /// <param name="inputReceiver">The interface used for receiving <see cref="Input"/> over the network.</param>
        /// <param name="inputSimulator">The interface used for simulating keyboard input on the host's computer.</param>
        /// <param name="mainWindow">The main window of the GUI.</param>
        public VirtualMouse(IInputHost inputReceiver, IInputSimulator inputSimulator, MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            this.inputHandlerCallback = new AsyncCallback(handleInput);
            this.inputSimulator = inputSimulator;

            this.inputReceiver = inputReceiver;
            
            // This call is necessary to begin receiving input asynchonously.
            this.inputReceiver.BeginReceivingInput(this.inputHandlerCallback);
        }

        /// <summary>
        /// Simulates a mouse movement event on the host machine.
        /// </summary>
        /// <param name="point">The location to move the mouse to. Stored as an xy-coordiante 
        /// <see cref="Utils.POINT"/>.</param>
        public void MoveMouse(Utils.POINT point)
        {
            this.inputSimulator.Mouse.MoveMouseTo(point.x, point.y);
        }

        /// <summary>
        /// Simulates a left mouse button press event on the host machine.
        /// </summary>
        public void PressLeftMouseButton()
        {
            this.inputSimulator.Mouse.LeftButtonDown();
        }

        /// <summary>
        /// Simulates a middle mouse button press event on the host machine.
        /// </summary>
        public void PressMiddleMouseButton()
        {
            this.inputSimulator.Mouse.MiddleButtonDown();
        }

        /// <summary>
        /// Simulates a right mouse button press event on the host machine.
        /// </summary>
        public void PressRightMouseButton()
        {
            this.inputSimulator.Mouse.RightButtonDown();
        }

        /// <summary>
        /// Simulates a left mouse button release event on the host machine.
        /// </summary>
        public void ReleaseLeftMouseButton()
        {
            this.inputSimulator.Mouse.LeftButtonUp();
        }

        /// <summary>
        /// Simulates a middle mouse button release event on the host machine.
        /// </summary>
        public void ReleaseMiddleMouseButton()
        {
            this.inputSimulator.Mouse.MiddleButtonUp();
        }
        /// <summary>
        /// Simulates a right mouse button release event on the host machine.
        /// </summary>
        public void ReleaseRightMouseButton()
        {
            this.inputSimulator.Mouse.RightButtonUp();
        }
        /// <summary>
        /// Simulates a mouse wheel scroll event on the host machine.
        /// </summary>
        /// <param name="wheelDelta">Represents the amount to scroll in clicks as a multiple of 120. Positive values 
        /// represent that the mouse wheel has been scrolled up, away from the user. Negative wheels represent that the
        /// wheel has been scrolled down, toward the user.</param>
        public void ScrollMouseWheel(int wheelDelta)
        {
            this.inputSimulator.Mouse.VerticalScroll(wheelDelta);
        }

        /// <summary>
        /// The callback function that is called when input is received from the client. It receives the event by
        /// calling <see cref="IInputHost.EndReceivingInput(IAsyncResult, IPEndPoint)"/> and then resumes waiting for
        /// <see cref="Input"/> asynchronously by again calling <see cref="IInputHost.BeginReceivingInput(AsyncCallback)"/>.
        /// Upon receiving the <see cref="Input"/> event data, the mouse event is simualted and the GUI is updated.
        /// </summary>
        /// <param name="inputResult">Contains state information about the connection and data received.</param>
        private void handleInput(IAsyncResult inputResult)
        {
            IPEndPoint endpoint = ((UdpState)(inputResult.AsyncState)).endpoint;
            Input mouseInput = this.inputReceiver.EndReceivingInput(inputResult, endpoint);

            this.inputReceiver.BeginReceivingInput(this.inputHandlerCallback);

            Console.WriteLine($"x: {mouseInput.point.x} y: {mouseInput.point.y}; {mouseInput.mouseEvent}");
            switch(mouseInput.mouseEvent)
            {
                case Utils.MouseMessages.WM_LBUTTONDOWN:
                    PressLeftMouseButton();
                    PostMouseEventMessageToGUI("Left Mouse Button was pressed" + '\n');
                    break;
                case Utils.MouseMessages.WM_LBUTTONUP:
                    ReleaseLeftMouseButton();
                    PostMouseEventMessageToGUI("Left Mouse Button was released" + '\n');
                    break;
                case Utils.MouseMessages.WM_RBUTTONDOWN:
                    PressRightMouseButton();
                    PostMouseEventMessageToGUI("Right Mouse Button was pressed" + '\n');
                    break;
                case Utils.MouseMessages.WM_RBUTTONUP:
                    ReleaseRightMouseButton();
                    PostMouseEventMessageToGUI("RIght Mouse Button was released" + '\n');
                    break;
                case Utils.MouseMessages.WM_MBUTTONDOWN:
                    PressMiddleMouseButton();
                    PostMouseEventMessageToGUI("Middle Mouse Button was pressed" + '\n');
                    break;
                case Utils.MouseMessages.WM_MBUTTONUP:
                    ReleaseRightMouseButton();
                    PostMouseEventMessageToGUI("Middle Mouse Button was released" + '\n');
                    break;
                case Utils.MouseMessages.WM_MOUSEWHEEL:
                    ScrollMouseWheel(mouseInput.wheelDelta);
                    PostMouseEventMessageToGUI($"scroll wheel was scrolled {((mouseInput.wheelDelta > 0) ? ("up") : ("down"))}{Environment.NewLine}");
                    break;
                case Utils.MouseMessages.WM_MOUSEMOVE:
                    MoveMouse(mouseInput.point);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Updates the GUI with information about the <see cref="Input"/> event received.
        /// </summary>
        /// <param name="keyboardInput">The <see cref="Input"/> event data received.</param>
        private void PostMouseEventMessageToGUI(string eventMessage)
        {
            this.mainWindow.Dispatcher.Invoke(() => {
                this.mainWindow.Host_Input_Textbox.Text += eventMessage;
            });
        }
    }
}
