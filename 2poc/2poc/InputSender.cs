using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace _2poc
{
    public class InputSender : IInputClient
    {
        UdpClient udpClient;

        public InputSender(IPAddress hostIp, int portNumber)
        {
            this.udpClient = new UdpClient();
            this.udpClient.Connect(hostIp, portNumber);
        }

        public void Close()
        {
            this.udpClient.Close();
        }

        public async void SendInputAsync(string input)
        {
            byte[] datagram = Encoding.ASCII.GetBytes(input);
            await this.udpClient.SendAsync(datagram, datagram.Length);
        }

        public void ReceiveInputAsync(InputHandler inputHandler)
        {
            throw new NotImplementedException();
        }
    }
}
