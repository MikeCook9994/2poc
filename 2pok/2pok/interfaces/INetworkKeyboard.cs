using System.Net;

namespace _2pok.interfaces
{
    interface INetworkKeyboard : IKeyboard
    {
        void Connect(IPAddress hostIp, int portNumber);
    }
}
