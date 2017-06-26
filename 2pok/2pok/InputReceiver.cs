using System;
using System.Net;
using System.Net.Sockets;

namespace _2pok
{
    public class InputReceiver : IInputClient
    {
        private UdpState udpState;

        public InputReceiver(int portNumber)
        {
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, portNumber);
            UdpClient udpClient = new UdpClient(ipEndPoint);

            this.udpState = new UdpState(udpClient, ipEndPoint);
        }

        public void Close()
        {
            this.udpState.client.Close();
        }

        public void BeginReceiveInput(AsyncCallback inputHandlerCallback)
        {
            this.udpState.client.BeginReceive(inputHandlerCallback, udpState);
        }

        public byte[] EndReceiveInput(IAsyncResult inputResult, IPEndPoint endpoint)
        {
            return this.udpState.client.EndReceive(inputResult, ref endpoint);
        }

        public void SendInputAsync(string input)
        {
            throw new NotImplementedException();
        }
    }
}
