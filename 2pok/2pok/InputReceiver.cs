using System;
using System.Net;
using System.Net.Sockets;
using _2pok.interfaces;

namespace _2pok
{
    public class InputReceiver : IInputReceiver
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

        public void BeginReceivingInput(AsyncCallback inputHandlerCallback)
        {
            this.udpState.client.BeginReceive(inputHandlerCallback, udpState);
        }

        public KeyboardInput EndReceivingKeyboardInput(IAsyncResult inputResult, IPEndPoint endpoint)
        {
            return (KeyboardInput)Utils.ByteArrayToObject(this.udpState.client.EndReceive(inputResult, ref endpoint));
        }

        public MouseInput EndReceivingMouseInput(IAsyncResult inputResult, IPEndPoint endpoint)
        {
            return (MouseInput)Utils.ByteArrayToObject(this.udpState.client.EndReceive(inputResult, ref endpoint));
        }
    }
}
