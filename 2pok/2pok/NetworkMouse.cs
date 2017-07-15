using System.Threading.Tasks;
using System.Timers;

using _2pok.interfaces;

namespace _2pok
{
    class NetworkMouse : IMouse
    {
        IInputClient inputSender;
        MouseMonitor mouseMonitor;
        Timer movementTimer;

        Utils.POINT mostRecentMousePosition;

        public NetworkMouse(IInputClient inputSender, MouseMonitor mouseMonitor)
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
                await SendMouseEventAsync(Utils.MouseMessages.WM_MOUSEMOVE, this.mostRecentMousePosition);
            });
            this.movementTimer.Enabled = true;
        }

        public void MoveMouse(Utils.POINT point)
        {
            this.mostRecentMousePosition = point;
        }

        public async void PressLeftMouseButton()
        {
            await SendMouseEventAsync(Utils.MouseMessages.WM_LBUTTONDOWN);
        }

        public async void PressMiddleMouseButton()
        {
            await SendMouseEventAsync(Utils.MouseMessages.WM_LBUTTONUP);
        }

        public async void PressRightMouseButton()
        {
            await SendMouseEventAsync(Utils.MouseMessages.WM_RBUTTONDOWN);
        }

        public async void ReleaseLeftMouseButton()
        {
            await SendMouseEventAsync(Utils.MouseMessages.WM_RBUTTONUP);
        }

        public async void ReleaseMiddleMouseButton()
        {
            await SendMouseEventAsync(Utils.MouseMessages.WM_MBUTTONDOWN);
        }

        public async void ReleaseRightMouseButton()
        {
            await SendMouseEventAsync(Utils.MouseMessages.WM_MBUTTONUP);
        }

        public async void ScrollMouseWheel(int wheelDelta)
        {
            await SendMouseEventAsync(Utils.MouseMessages.WM_MOUSEWHEEL, wheelDelta);
        }
        
        private async Task SendMouseEventAsync(Utils.MouseMessages eventType)
        {
            await this.inputSender.SendInputAsync(new Input(eventType));
        }

        private async Task SendMouseEventAsync(Utils.MouseMessages eventType, Utils.POINT point)
        { 
            await this.inputSender.SendInputAsync(new Input(eventType, point));
        }

        private async Task SendMouseEventAsync(Utils.MouseMessages eventType, int wheelDelta)
        {
            await this.inputSender.SendInputAsync(new Input(eventType, wheelDelta));
        }
    }
}
