using System;
using System.Timers;

using _2pok.interfaces;

namespace _2pok
{
    class NetworkMouse : IMouse
    {
        IInputSender inputSender;
        MouseMonitor mouseMonitor;
        Timer movementTimer;
        MouseInput mostRecentMouseInput;

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

            this.mouseMonitor.scrollWheelDownCallback += ScrollMouseWheelDown;
            this.mouseMonitor.scrollWheelUpCallback += ScrollMouseWheelUp;

            this.mouseMonitor.mouseMovedCallback += MoveMouseTo;

            this.movementTimer = new Timer();
            this.movementTimer.Interval = 100;
            this.movementTimer.AutoReset = true;
            this.movementTimer.Elapsed += (async (object sender, ElapsedEventArgs e) =>
            {
                await this.inputSender.SendMouseInputAsync(this.mostRecentMouseInput);
            });
            this.movementTimer.Enabled = true;
        }

        public void MoveMouseTo(Utils.MSLLHOOKSTRUCT hookStruct)
        {
            this.mostRecentMouseInput = new MouseInput(hookStruct.pt, hookStruct.mouseData);
            PrintHookStructContents(hookStruct);
        }

        public void PressLeftMouseButton(Utils.MSLLHOOKSTRUCT hookStruct)
        {
            PrintHookStructContents(hookStruct);
        }

        public void PressMiddleMouseButton(Utils.MSLLHOOKSTRUCT hookStruct)
        {
            PrintHookStructContents(hookStruct);
        }

        public void PressRightMouseButton(Utils.MSLLHOOKSTRUCT hookStruct)
        {
            PrintHookStructContents(hookStruct);
        }

        public void ReleaseLeftMouseButton(Utils.MSLLHOOKSTRUCT hookStruct)
        {
            PrintHookStructContents(hookStruct);
        }

        public void ReleaseMiddleMouseButton(Utils.MSLLHOOKSTRUCT hookStruct)
        {
            PrintHookStructContents(hookStruct);
        }

        public void ReleaseRightMouseButton(Utils.MSLLHOOKSTRUCT hookStruct)
        {
            PrintHookStructContents(hookStruct);
        }

        public void ScrollMouseWheelDown(Utils.MSLLHOOKSTRUCT hookStruct)
        {
            PrintHookStructContents(hookStruct);
        }

        public void ScrollMouseWheelUp(Utils.MSLLHOOKSTRUCT hookStruct)
        {
            PrintHookStructContents(hookStruct);
        }

        private void PrintHookStructContents(Utils.MSLLHOOKSTRUCT hookStruct)
        {
            Console.WriteLine($"POINT: ({hookStruct.pt.x}, {hookStruct.pt.y})");
            Console.WriteLine($"SCROLL DIRECTION: {hookStruct.mouseData}");
            Console.WriteLine($"FLAGS: {hookStruct.flags}");
            Console.WriteLine($"TIME: {hookStruct.time}");
            Console.WriteLine($"DWEXTRAINFO: {hookStruct.dwExtraInfo}");
            Console.WriteLine("");
        }
    }
}
