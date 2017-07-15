using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

using _2pok.interfaces;

namespace _2pok
{
    public class InputClient : IInputClient
    {
        UdpClient udpClient;

        public InputClient()
        {
            this.udpClient = new UdpClient();
        }

        public void Connect(IPAddress hostIp, int portNumber)
        {
            this.udpClient.Connect(hostIp, portNumber);
        }

        public void Close()
        {
            this.udpClient.Close();
        }

        public async Task<int> SendInputAsync(Input input)
        {
            byte[] datagram = Utils.ObjectToByteArray(input);
            return await this.udpClient.SendAsync(datagram, datagram.Length);
        }
    }
}
