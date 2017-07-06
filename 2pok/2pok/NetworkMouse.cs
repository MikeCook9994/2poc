using System;
using _2pok.interfaces;

namespace _2pok
{
    class NetworkMouse : IMouse
    {
        IInputSender inputSender;
        MouseMonitor mouseMonitor;

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
        }
        
        public void MoveMouseTo(MouseMonitor.MOUSEHOOKSTRUCT hookStruct)
        {
            PrintHookStructContents(hookStruct);
        }

        public void PressLeftMouseButton(MouseMonitor.MOUSEHOOKSTRUCT hookStruct)
        {
            PrintHookStructContents(hookStruct);
        }

        public void PressMiddleMouseButton(MouseMonitor.MOUSEHOOKSTRUCT hookStruct)
        {
            PrintHookStructContents(hookStruct);
        }

        public void PressRightMouseButton(MouseMonitor.MOUSEHOOKSTRUCT hookStruct)
        {
            PrintHookStructContents(hookStruct);
        }

        public void ReleaseLeftMouseButton(MouseMonitor.MOUSEHOOKSTRUCT hookStruct)
        {
            PrintHookStructContents(hookStruct);
        }

        public void ReleaseMiddleMouseButton(MouseMonitor.MOUSEHOOKSTRUCT hookStruct)
        {
            PrintHookStructContents(hookStruct);
        }

        public void ReleaseRightMouseButton(MouseMonitor.MOUSEHOOKSTRUCT hookStruct)
        {
            PrintHookStructContents(hookStruct);
        }

        public void ScrollMouseWheelDown(MouseMonitor.MOUSEHOOKSTRUCT hookStruct)
        {
            PrintHookStructContents(hookStruct);
        }

        public void ScrollMouseWheelUp(MouseMonitor.MOUSEHOOKSTRUCT hookStruct)
        {
            PrintHookStructContents(hookStruct);
        }

        private void PrintHookStructContents(MouseMonitor.MOUSEHOOKSTRUCT hookStruct)
        {
            Console.WriteLine("Mouse location is (" + hookStruct.pt.x + ", " + hookStruct.pt.y + ")");
            Console.WriteLine("Mouse HWND: " + hookStruct.hwnd);
            Console.WriteLine("Mouse wHitTestCode: " + hookStruct.wHitTestCode);
            Console.WriteLine("Mouse dwExtraInfo: " + hookStruct.dwExtraInfo);
        }
    }
}
