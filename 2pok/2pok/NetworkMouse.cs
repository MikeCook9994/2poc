using System;
using System.Threading.Tasks;
using System.Timers;

using _2pok.interfaces;

namespace _2pok
{
    class NetworkMouse : IMouse
    {
        IInputSender inputSender;
        MouseMonitor mouseMonitor;
        Timer movementTimer;
        Utils.MSLLHOOKSTRUCT mostRecentMouseInput;

        public NetworkMouse(IInputSender inputSender, MouseMonitor mouseMonitor)
        {
            this.inputSender = inputSender;
            this.mouseMonitor = mouseMonitor;

            this.mouseMonitor.leftMouseButtonDownCallback += PressLeftMouseButton;
            this.mouseMonitor.leftMouseButtonUpCallback += ReleaseLeftMouseButton;

            this.mouseMonitor.rightMouseButtonDownCallback += PressRightMouseButton;
            this.mouseMonitor.rightMouseButtonUpCallback += ReleaseRightMouseButton;

            this.mouseMonitor.middleMouseButtonDownCallback += PressMiddleMouseButton;
            this.mouseMonitor.middleMouseButtonUpCallback += ReleaseMiddleMouseButton;

            this.mouseMonitor.scrollMouseWheel += ScrollMouseWheel;

            this.mouseMonitor.mouseMovedCallback += MoveMouse;

            this.movementTimer = new Timer();
            this.movementTimer.Interval = 100;
            this.movementTimer.AutoReset = true;
            this.movementTimer.Elapsed += (async (object sender, ElapsedEventArgs e) =>
            {
                await SendMouseEventAsync(Utils.MouseMessages.WM_MOUSEMOVE, this.mostRecentMouseInput);
            });
            this.movementTimer.Enabled = true;
        }

        public void MoveMouse(Utils.MSLLHOOKSTRUCT hookStruct)
        {
            this.mostRecentMouseInput = hookStruct;
        }

        public async void PressLeftMouseButton(Utils.MSLLHOOKSTRUCT hookStruct)
        {
            await SendMouseEventAsync(Utils.MouseMessages.WM_LBUTTONDOWN, hookStruct);
        }

        public async void PressMiddleMouseButton(Utils.MSLLHOOKSTRUCT hookStruct)
        {
            await SendMouseEventAsync(Utils.MouseMessages.WM_LBUTTONUP, hookStruct);
        }

        public async void PressRightMouseButton(Utils.MSLLHOOKSTRUCT hookStruct)
        {
            await SendMouseEventAsync(Utils.MouseMessages.WM_RBUTTONDOWN, hookStruct);
        }

        public async void ReleaseLeftMouseButton(Utils.MSLLHOOKSTRUCT hookStruct)
        {
            await SendMouseEventAsync(Utils.MouseMessages.WM_RBUTTONUP, hookStruct);
        }

        public async void ReleaseMiddleMouseButton(Utils.MSLLHOOKSTRUCT hookStruct)
        {
            await SendMouseEventAsync(Utils.MouseMessages.WM_MBUTTONDOWN, hookStruct);
        }

        public async void ReleaseRightMouseButton(Utils.MSLLHOOKSTRUCT hookStruct)
        {
            await SendMouseEventAsync(Utils.MouseMessages.WM_MBUTTONUP, hookStruct);
        }

        public async void ScrollMouseWheel(Utils.MSLLHOOKSTRUCT hookStruct)
        {
            await SendMouseEventAsync(Utils.MouseMessages.WM_MOUSEWHEEL, hookStruct);
        }
        
        private async Task SendMouseEventAsync(Utils.MouseMessages eventType, Utils.MSLLHOOKSTRUCT hookStruct)
        {
            MouseInput mouseInput = new MouseInput(eventType, hookStruct);
            await this.inputSender.SendMouseInputAsync(mouseInput);
        }
    }
}
