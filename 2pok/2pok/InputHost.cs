using System;
using System.Net;
using System.Net.Sockets;

using _2pok.interfaces;

namespace _2pok
{
    public class InputHost : IInputHost
    {
        private UdpState udpState;

        public InputHost(int portNumber)
        {
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, portNumber);
            UdpClient udpClient = new UdpClient(ipEndPoint);

            this.udpState = new UdpState(udpClient, ipEndPoint);
        }

        public void Close()
        {
            this.udpState.client.Close();
        }

        public void BeginReceivingInput(AsyncCallback inputHandlerCallback)
        {
            this.udpState.client.BeginReceive(inputHandlerCallback, udpState);
        }

        public Input EndReceivingInput(IAsyncResult inputResult, IPEndPoint endpoint)
        {
            return (Input)Utils.ByteArrayToObject(this.udpState.client.EndReceive(inputResult, ref endpoint));
        }
    }
}
