using System.Windows.Forms;

namespace _2pok.interfaces
{
    interface IKeyboard
    {
        void PressKey(Keys key);

        void ReleaseKey(Keys key);

        void Disconnect();
    }
}
