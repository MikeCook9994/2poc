using System.Threading.Tasks;
using System.Timers;

using _2pok.interfaces;

namespace _2pok
{
    /// <summary>
    /// Provides a simple interface for sending mouse input events over a network.
    /// </summary>
    class NetworkMouse : IMouse
    {
        /// <summary>
        /// The network interface for sending <see cref="Input"/> event data.
        /// </summary>
        private IInputClient inputSender;

        /// <summary>
        /// The mouse monitor that sets up os level hooks for listening for mouse events.
        /// </summary>
        private MouseMonitor mouseMonitor;

        /// <summary>
        /// As opposed to sending every mouse event, a timer is used to send only the most recent mouse movement 
        /// every .016 seconds (just above 60 inputs per second). This minimizes the amount of data that is sent
        /// over the network but should still provide a somewhat smooth experience.
        /// </summary>
        private Timer movementTimer;

        /// <summary>
        /// The most recent location of the mouse of the mouse that will be sent over the network when the next
        /// movement even is to be sent. Stored as an xy-coordinate <see cref="Utils.POINT"/>
        /// </summary>
        private Utils.POINT mostRecentMousePosition;

        /// <summary>
        /// Constructs a new instance that monitors the client computer's physical mouse for input events and
        /// can send them over the network as <see cref="Input"/>
        /// </summary>
        /// <param name="inputSender">The interface used for sending <see cref="Input"/> over the network.</param>
        /// <param name="keyboardMonitor">Sets up os level hooks for monitoring the mouse.</param>
        public NetworkMouse(IInputClient inputSender, MouseMonitor mouseMonitor)
        {
            this.inputSender = inputSender;
            this.mouseMonitor = mouseMonitor;

            // Sets up the callbacks from the mouse monitor.
            this.mouseMonitor.leftMouseButtonDownCallback += PressLeftMouseButton;
            this.mouseMonitor.leftMouseButtonUpCallback += ReleaseLeftMouseButton;

            this.mouseMonitor.rightMouseButtonDownCallback += PressRightMouseButton;
            this.mouseMonitor.rightMouseButtonUpCallback += ReleaseRightMouseButton;

            this.mouseMonitor.middleMouseButtonDownCallback += PressMiddleMouseButton;
            this.mouseMonitor.middleMouseButtonUpCallback += ReleaseMiddleMouseButton;

            this.mouseMonitor.scrollMouseWheelCallback += ScrollMouseWheel;

            this.mouseMonitor.moveMouseCallback += MoveMouse;

            // Sets up the timer for sending mouse movement events periodically.
            this.movementTimer = new Timer();
            this.movementTimer.Interval = 16;
            this.movementTimer.AutoReset = true;
            this.movementTimer.Elapsed += (async (object sender, ElapsedEventArgs e) =>
            {
                await SendMouseEventAsync(Utils.MouseMessages.WM_MOUSEMOVE, this.mostRecentMousePosition);
            });
            this.movementTimer.Enabled = true;
        }

        /// <summary>
        /// Updates the most recent position of the mouse to be sent over the network when the next event is to be 
        /// sent.
        /// </summary>
        /// <param name="point">The new position of the mouse. Stored as an xy-coordinate <see cref="Utils.POINT"/>
        /// </param>
        public void MoveMouse(Utils.POINT point)
        {
            this.mostRecentMousePosition = point;
        }

        /// <summary>
        /// Sends a left mouse button press event over the network.
        /// </summary>
        public async void PressLeftMouseButton()
        {
            await SendMouseEventAsync(Utils.MouseMessages.WM_LBUTTONDOWN);
        }

        /// <summary>
        /// Sends a middle mouse button press event over the network.
        /// </summary>
        public async void PressMiddleMouseButton()
        {
            await SendMouseEventAsync(Utils.MouseMessages.WM_LBUTTONUP);
        }

        /// <summary>
        /// Sends a right mouse button press event over the network.
        /// </summary>
        public async void PressRightMouseButton()
        {
            await SendMouseEventAsync(Utils.MouseMessages.WM_RBUTTONDOWN);
        }

        /// <summary>
        /// Sends a left mouse button release event over the network.
        /// </summary>
        public async void ReleaseLeftMouseButton()
        {
            await SendMouseEventAsync(Utils.MouseMessages.WM_RBUTTONUP);
        }

        /// <summary>
        /// Sends a middle mouse button release event over the network.
        /// </summary>
        public async void ReleaseMiddleMouseButton()
        {
            await SendMouseEventAsync(Utils.MouseMessages.WM_MBUTTONDOWN);
        }

        /// <summary>
        /// Sends a right mouse button release event over the network.
        /// </summary>
        public async void ReleaseRightMouseButton()
        {
            await SendMouseEventAsync(Utils.MouseMessages.WM_MBUTTONUP);
        }

        /// <summary>
        /// Sends a mouse scroll event over the network.
        /// </summary>
        /// <param name="wheelDelta">Represents the amount to scroll in clicks as a multiple of 120. Positive values 
        /// represent that the mouse wheel has been scrolled up, away from the user. Negative wheels represent that the
        /// wheel has been scrolled down, toward the user.</param>
        public async void ScrollMouseWheel(int wheelDelta)
        {
            await SendMouseEventAsync(Utils.MouseMessages.WM_MOUSEWHEEL, wheelDelta);
        }

        /// <summary>
        /// Constructs a new <see cref="Input"/> instance for button press and release events and sends it over the 
        /// network.
        /// </summary>
        /// <param name="eventType">The type of mouse event that ocurred.</param>
        /// <returns>The asynchronous task to be awaited.</returns>
        private async Task SendMouseEventAsync(Utils.MouseMessages eventType)
        {
            await this.inputSender.SendInputAsync(new Input(eventType));
        }

        /// <summary>
        /// Constructs a new <see cref="Input"/> instance for mouse movement events and sends it over the network.
        /// </summary>
        /// <param name="eventType">The type of mouse event that ocurred.</param>
        /// <param name="point">The location to move the mouse to represented as an xy-coordinate 
        /// <see cref="Utils.POINT"/></param>
        /// <returns>The asynchronous task to be awaited.</returns>
        private async Task SendMouseEventAsync(Utils.MouseMessages eventType, Utils.POINT point)
        { 
            await this.inputSender.SendInputAsync(new Input(eventType, point));
        }

        /// <summary>
        /// Constructs a new <see cref="Input"/> instance for mouse wheel scroll events and sends it over the network.
        /// </summary>
        /// <param name="eventType">The type of mouse event that ocurred.</param>
        /// <param name="wheelDelta">Represents the amount to scroll in clicks as a multiple of 120. Positive values 
        /// represent that the mouse wheel has been scrolled up, away from the user. Negative wheels represent that the
        /// wheel has been scrolled down, toward the user.</param>
        /// <returns>The asynchronous task to be awaited.</returns>
        private async Task SendMouseEventAsync(Utils.MouseMessages eventType, int wheelDelta)
        {
            await this.inputSender.SendInputAsync(new Input(eventType, wheelDelta));
        }
    }
}
