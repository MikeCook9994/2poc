using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

using _2pok.interfaces;

namespace _2pok
{
    /// <summary>
    /// Connects to a host specified by an IP address over a specified port number and sends <see cref="Input"/>
    /// event data over the network.
    /// </summary>
    public class InputClient : IInputClient
    {
        /// <summary>
        /// The <see cref="UdpClient"/> that establishes the connection between the client and host and is
        /// used to send and receive datagrams.
        /// </summary>
        UdpClient udpClient;

        /// <summary>
        /// Initializes a new InputClient object that is prepared to connect with an <see cref="InputHost"/>
        /// object to send input events to.
        /// </summary>
        public InputClient()
        {
            this.udpClient = new UdpClient();
        }

        /// <summary>
        /// Attempts to Establish a connection with an <see cref="InputHost"/>
        /// </summary>
        /// <param name="hostIp">The host IP to attempt a connection with.</param>
        /// <param name="portNumber">The port to attempt making the connection over.</param>
        public void Connect(IPAddress hostIp, int portNumber)
        {
            this.udpClient.Connect(hostIp, portNumber);
        }

        /// <summary>
        /// Closes an established connection between the input host and the input client.
        /// </summary>
        public void Close()
        {
            this.udpClient.Close();
        }

        /// <summary>
        /// Sends input event data to the input host over the established connection.
        /// </summary>
        /// <param name="input">The input data to send.</param>
        /// <returns>The number of bytes sent.</returns>
        public async Task<int> SendInputAsync(Input input)
        {
            byte[] datagram = Utils.ObjectToByteArray(input);
            return await this.udpClient.SendAsync(datagram, datagram.Length);
        }
    }
}
