namespace _2pok.interfaces
{
    interface IMouse
    {
        void PressLeftMouseButton(Utils.MSLLHOOKSTRUCT hookStruct);

        void ReleaseLeftMouseButton(Utils.MSLLHOOKSTRUCT hookStruct);

        void PressRightMouseButton(Utils.MSLLHOOKSTRUCT hookStruct);

        void ReleaseRightMouseButton(Utils.MSLLHOOKSTRUCT hookStruct);

        void PressMiddleMouseButton(Utils.MSLLHOOKSTRUCT hookStruct);

        void ReleaseMiddleMouseButton(Utils.MSLLHOOKSTRUCT hookStruct);

        void ScrollMouseWheelUp(Utils.MSLLHOOKSTRUCT hookStruct);

        void ScrollMouseWheelDown(Utils.MSLLHOOKSTRUCT hookStruct);

        void MoveMouseTo(Utils.MSLLHOOKSTRUCT hookStruct);
    }
}
