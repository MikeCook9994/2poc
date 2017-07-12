namespace _2pok.interfaces
{
    public interface IMouse
    {
        void PressLeftMouseButton(Utils.MSLLHOOKSTRUCT hookStruct);

        void ReleaseLeftMouseButton(Utils.MSLLHOOKSTRUCT hookStruct);

        void PressRightMouseButton(Utils.MSLLHOOKSTRUCT hookStruct);

        void ReleaseRightMouseButton(Utils.MSLLHOOKSTRUCT hookStruct);

        void PressMiddleMouseButton(Utils.MSLLHOOKSTRUCT hookStruct);

        void ReleaseMiddleMouseButton(Utils.MSLLHOOKSTRUCT hookStruct);

        void ScrollMouseWheel(Utils.MSLLHOOKSTRUCT hookStruct);

        void MoveMouse(Utils.MSLLHOOKSTRUCT hookStruct);
    }
}
