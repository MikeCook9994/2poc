using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2pok.interfaces
{
    interface IInputSender
    {
        Task<int> SendKeyboardInputAsync(KeyboardInput keyboardInput);

        Task<int> SendMouseInputAsync();

        void Connect(IPAddress hostIp, int portNumber);

        void Close();
    }
}
