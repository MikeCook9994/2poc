using System;

using WindowsInput;

using _2pok.interfaces;
using System.Net;

namespace _2pok
{
    public class VirtualMouse : IMouse
    {
        MainWindow mainWindow;
        AsyncCallback inputHandlerCallback;
        IInputSimulator inputSimulator;
        IInputReceiver inputReceiver;

        public VirtualMouse(IInputReceiver inputReceiver, IInputSimulator inputSimulator, MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            this.inputHandlerCallback = new AsyncCallback(handleInput);
            this.inputSimulator = inputSimulator;

            this.inputReceiver = inputReceiver;
            this.inputReceiver.BeginReceivingInput(this.inputHandlerCallback);
        }

        public void MoveMouse(Utils.MSLLHOOKSTRUCT hookStruct)
        {
            Utils.POINT point = hookStruct.pt;
            this.inputSimulator.Mouse.MoveMouseTo(point.x, point.y);
        }

        public void PressLeftMouseButton(Utils.MSLLHOOKSTRUCT hookStruct)
        {
            this.inputSimulator.Mouse.LeftButtonDown();
        }

        public void PressMiddleMouseButton(Utils.MSLLHOOKSTRUCT hookStruct)
        {
            this.inputSimulator.Mouse.MiddleButtonDown();
        }

        public void PressRightMouseButton(Utils.MSLLHOOKSTRUCT hookStruct)
        {
            this.inputSimulator.Mouse.RightButtonDown();
        }

        public void ReleaseLeftMouseButton(Utils.MSLLHOOKSTRUCT hookStruct)
        {
            this.inputSimulator.Mouse.LeftButtonUp();
        }

        public void ReleaseMiddleMouseButton(Utils.MSLLHOOKSTRUCT hookStruct)
        {
            this.inputSimulator.Mouse.MiddleButtonUp();
        }

        public void ReleaseRightMouseButton(Utils.MSLLHOOKSTRUCT hookStruct)
        {
            this.inputSimulator.Mouse.RightButtonUp();
        }

        public void ScrollMouseWheel(Utils.MSLLHOOKSTRUCT hookStruct)
        {
            this.inputSimulator.Mouse.VerticalScroll(hookStruct.mouseData);
        }

        private void handleInput(IAsyncResult inputResult)
        {
            IPEndPoint endpoint = ((UdpState)(inputResult.AsyncState)).endpoint;
            MouseInput mouseInput = this.inputReceiver.EndReceivingMouseInput(inputResult, endpoint);
            Utils.MSLLHOOKSTRUCT mouseHookStruct = mouseInput.mouseHookStruct;

            this.inputReceiver.BeginReceivingInput(this.inputHandlerCallback);

            switch(mouseInput.eventType)
            {
                case Utils.MouseMessages.WM_LBUTTONDOWN:
                    PressLeftMouseButton(mouseHookStruct);
                    PostMouseEventMessageToGUI("Left Mouse Button was pressed" + '\n');
                    break;
                case Utils.MouseMessages.WM_LBUTTONUP:
                    ReleaseLeftMouseButton(mouseHookStruct);
                    PostMouseEventMessageToGUI("Left Mouse Button was released" + '\n');
                    break;
                case Utils.MouseMessages.WM_RBUTTONDOWN:
                    PressRightMouseButton(mouseHookStruct);
                    PostMouseEventMessageToGUI("Right Mouse Button was pressed" + '\n');
                    break;
                case Utils.MouseMessages.WM_RBUTTONUP:
                    ReleaseRightMouseButton(mouseHookStruct);
                    PostMouseEventMessageToGUI("RIght Mouse Button was released" + '\n');
                    break;
                case Utils.MouseMessages.WM_MBUTTONDOWN:
                    PressMiddleMouseButton(mouseHookStruct);
                    PostMouseEventMessageToGUI("Middle Mouse Button was pressed" + '\n');
                    break;
                case Utils.MouseMessages.WM_MBUTTONUP:
                    ReleaseRightMouseButton(mouseHookStruct);
                    PostMouseEventMessageToGUI("Middle Mouse Button was released" + '\n');
                    break;
                case Utils.MouseMessages.WM_MOUSEWHEEL:
                    ScrollMouseWheel(mouseHookStruct);
                    PostMouseEventMessageToGUI($"scroll wheel was scrolled {((mouseHookStruct.mouseData > 0) ? ("up") : ("down"))}{Environment.NewLine}");
                    break;
                case Utils.MouseMessages.WM_MOUSEMOVE:
                    MoveMouse(mouseHookStruct);
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
