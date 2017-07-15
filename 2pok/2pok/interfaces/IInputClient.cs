using System.Net;
using System.Threading.Tasks;

namespace _2pok.interfaces
{
    /// <summary>
    /// Provides an interface for sending keyboard and mouse input over a network. 
    /// </summary>
    public interface IInputClient
    {
        /// <summary>
        /// Sends the input data to the host over the network.
        /// </summary>
        /// <param name="input">the input data</param>
        /// <returns>the number of bytes sent</returns>
        Task<int> SendInputAsync(Input input);

        /// <summary>
        /// Establishes the connection between the client and the host for sending and receiving input data.
        /// </summary>
        /// <param name="hostIp">the IP address of the host</param>
        /// <param name="portNumber">the port number to establish the connection on</param>
        void Connect(IPAddress hostIp, int portNumber);

        /// <summary>
        /// Closes the connection between the client and the host.
        /// </summary>
        void Close();
    }
}
