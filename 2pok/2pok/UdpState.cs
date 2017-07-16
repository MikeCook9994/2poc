using System.Net;
using System.Net.Sockets;

namespace _2pok
{
    /// <summary>
    /// The state of the UDP connection including the <see cref="UdpClient"/> containing information about the 
    /// connection and the <see cref="IPEndPoint"/> detailing acceptable clients.
    /// </summary>
    class UdpState
    {
        /// <summary>
        /// The client used to establish the udp connection between the client and the host.
        /// </summary>
        public UdpClient client;

        /// <summary>
        /// The IPEndPoint describing acceptable clients of some IP Address (or any) connecting on some port.
        /// </summary>
        public IPEndPoint endpoint;

        /// <summary>
        /// Packages the component objects of the UDP state.
        /// </summary>
        /// <param name="client">The client used to establish the udp connection between the client and the 
        /// host.</param>
        /// <param name="endpoint">The IPEndPoint describing acceptable clients of some IP Address (or any) connecting 
        /// on some port.</param>
        public UdpState(UdpClient client, IPEndPoint endpoint)
        {
            this.client = client;
            this.endpoint = endpoint;
        }
    }
}
