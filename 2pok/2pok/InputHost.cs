using System;
using System.Net;
using System.Net.Sockets;

using _2pok.interfaces;

namespace _2pok
{
    /// <summary>
    /// Opens the host machine up for connections over the specified port to receive <see cref="Input"/> event data
    /// from the client machine.
    /// </summary>
    public class InputHost : IInputHost
    {
        /// <summary>
        /// The state of the hosts UDP connection containing both the client with the
        /// established connection and the <see cref="IPEndPoint"/>s that are valid to
        /// make connections with.
        /// </summary>
        private UdpState udpState;

        /// <summary>
        /// Initializes the inputHose that is prepared to receive input from clients 
        /// over the provided port number.
        /// </summary>
        /// <param name="portNumber">The port number to accept connections over.</param>
        public InputHost(int portNumber)
        {
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, portNumber);
            UdpClient udpClient = new UdpClient(ipEndPoint);

            this.udpState = new UdpState(udpClient, ipEndPoint);
        }
        
        /// <summary>
        /// Closes the UDP connection between the client and the host.
        /// </summary>
        public void Close()
        {
            this.udpState.client.Close();
        }

        /// <summary>
        /// Begins receiving input asynchronously over a UDP connection.
        /// </summary>
        /// <param name="inputHandlerCallback">The callback that is used when input data is recieved over the 
        /// connection. The callback accepts a single parameter of the type <see cref="IAsyncResult"/></param>
        public void BeginReceivingInput(AsyncCallback inputHandlerCallback)
        {
            this.udpState.client.BeginReceive(inputHandlerCallback, udpState);
        }

        /// <summary>
        /// Returns the Input data that was sent by the host.
        /// </summary>
        /// <param name="inputResult">The result of the async transmission. This is the value passed to the 
        /// callback passed to <see cref="BeginReceivingInput(AsyncCallback)"/></param>
        /// <param name="endpoint">Defines which clients are acceptable to finish receiving input from.</param>
        /// <returns>The input data sent by the client.</returns>
        public Input EndReceivingInput(IAsyncResult inputResult, IPEndPoint endpoint)
        {
            return (Input)Utils.ByteArrayToObject(this.udpState.client.EndReceive(inputResult, ref endpoint));
        }
    }
}
