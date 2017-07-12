using System;

namespace _2pok
{
    [Serializable]
    public class MouseInput
    {
        Utils.POINT point;
        int mouseData;

        public MouseInput(Utils.POINT point, int mouseData)
        {
            this.point = point;
            this.mouseData = mouseData;
        }
    }
}
