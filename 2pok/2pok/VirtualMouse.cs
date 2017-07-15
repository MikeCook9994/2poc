using System;
using System.Net;

using WindowsInput;

using _2pok.interfaces;

namespace _2pok
{
    public class VirtualMouse : IMouse
    {
        MainWindow mainWindow;
        AsyncCallback inputHandlerCallback;
        IInputSimulator inputSimulator;
        IInputHost inputReceiver;

        public VirtualMouse(IInputHost inputReceiver, IInputSimulator inputSimulator, MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            this.inputHandlerCallback = new AsyncCallback(handleInput);
            this.inputSimulator = inputSimulator;

            this.inputReceiver = inputReceiver;
            this.inputReceiver.BeginReceivingInput(this.inputHandlerCallback);
        }

        public void MoveMouse(Utils.POINT point)
        {
            this.inputSimulator.Mouse.MoveMouseTo(point.x, point.y);
        }

        public void PressLeftMouseButton()
        {
            this.inputSimulator.Mouse.LeftButtonDown();
        }

        public void PressMiddleMouseButton()
        {
            this.inputSimulator.Mouse.MiddleButtonDown();
        }

        public void PressRightMouseButton()
        {
            this.inputSimulator.Mouse.RightButtonDown();
        }

        public void ReleaseLeftMouseButton()
        {
            this.inputSimulator.Mouse.LeftButtonUp();
        }

        public void ReleaseMiddleMouseButton()
        {
            this.inputSimulator.Mouse.MiddleButtonUp();
        }

        public void ReleaseRightMouseButton()
        {
            this.inputSimulator.Mouse.RightButtonUp();
        }

        public void ScrollMouseWheel(int wheelDelta)
        {
            this.inputSimulator.Mouse.VerticalScroll(wheelDelta);
        }

        private void handleInput(IAsyncResult inputResult)
        {
            IPEndPoint endpoint = ((UdpState)(inputResult.AsyncState)).endpoint;
            Input mouseInput = this.inputReceiver.EndReceivingInput(inputResult, endpoint);

            this.inputReceiver.BeginReceivingInput(this.inputHandlerCallback);

            switch(mouseInput.eventType)
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

        private void PostMouseEventMessageToGUI(string eventMessage)
        {
            this.mainWindow.Dispatcher.Invoke(() => {
                this.mainWindow.Host_Input_Textbox.Text += eventMessage;
            });
        }
    }
}
