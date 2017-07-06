namespace _2pok.interfaces
{
    interface IMouse
    {
        void PressLeftMouseButton(MouseMonitor.MOUSEHOOKSTRUCT hookStruct);

        void ReleaseLeftMouseButton(MouseMonitor.MOUSEHOOKSTRUCT hookStruct);

        void PressRightMouseButton(MouseMonitor.MOUSEHOOKSTRUCT hookStruct);

        void ReleaseRightMouseButton(MouseMonitor.MOUSEHOOKSTRUCT hookStruct);

        void PressMiddleMouseButton(MouseMonitor.MOUSEHOOKSTRUCT hookStruct);

        void ReleaseMiddleMouseButton(MouseMonitor.MOUSEHOOKSTRUCT hookStruct);

        void ScrollMouseWheelUp(MouseMonitor.MOUSEHOOKSTRUCT hookStruct);

        void ScrollMouseWheelDown(MouseMonitor.MOUSEHOOKSTRUCT hookStruct);

        void MoveMouseTo(MouseMonitor.MOUSEHOOKSTRUCT hookStruct);
    }
}
