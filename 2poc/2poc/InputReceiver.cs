using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace _2poc
{
    public class InputReceiver : IInputClient
    {
        UdpClient udpClient;
        int portNumber;

        public InputReceiver(int portNumber)
        {
            this.portNumber = portNumber;
            this.udpClient = new UdpClient(this.portNumber);
        }

        public void Close()
        {
            this.udpClient.Close();
        }

        public void ReceiveInputAsync(InputHandler inputHandler)
        {
            Task<UdpReceiveResult> inputTask = udpClient.ReceiveAsync();
            inputHandler(inputTask);
        }

        public void SendInputAsync(string input)
        {
            throw new NotImplementedException();
        }
    }
}
