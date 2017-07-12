using System;

namespace _2pok
{
    [Serializable]
    public class MouseInput
    {
        public Utils.MouseMessages eventType { get; set; }
        public Utils.MSLLHOOKSTRUCT mouseHookStruct { get; set; }

        public MouseInput(Utils.MouseMessages eventType, Utils.MSLLHOOKSTRUCT mouseHookStruct)
        {
            this.eventType = eventType;
            this.mouseHookStruct = mouseHookStruct;
        }
    }
}
