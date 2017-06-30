using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using _2pok.interfaces;

namespace _2pok
{
    public class InputSender : IInputSender
    {
        UdpClient udpClient;

        public InputSender()
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

        public async Task<int> SendKeyboardInputAsync(KeyboardInput keyboardInput)
        {
            byte[] datagram = Utils.ObjectToByteArray(keyboardInput);
            return await this.udpClient.SendAsync(datagram, datagram.Length);
        }

        public async Task<int> SendMouseInputAsync()
        {
            throw new NotImplementedException();
        }
    }
}
