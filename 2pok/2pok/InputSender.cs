using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace _2pok
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

        public byte[] EndReceiveInput(IAsyncResult inputResult, IPEndPoint endpoint)
        {
            throw new NotImplementedException();
        }

        public void BeginReceiveInput(AsyncCallback inputHandlerCallback)
        {
            throw new NotImplementedException();
        }
    }
}
