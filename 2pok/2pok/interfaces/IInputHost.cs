using System;
using System.Net;

namespace _2pok.interfaces
{
    /// <summary>
    /// Provides an interface for receiving keyboard and mouse input over a network. 
    /// </summary>
    public interface IInputHost
    {
        /// <summary>
        /// Begins the procedure to receive input asynchonrously.
        /// </summary>
        /// <param name="inputHandlerCallback">the asynchronous callback called when data is received</param>
        void BeginReceivingInput(AsyncCallback inputHandlerCallback);

        /// <summary>
        /// Retrieves the data send by the host. Called within the callback passed to 
        /// <see cref="BeginReceivingInput(AsyncCallback)"/>
        /// </summary>
        /// <param name="inputResult">the result of the asynchronous receive that is provided as a parameter to the 
        /// callback passed to <see cref="BeginReceivingInput(AsyncCallback)"/></param>
        /// <param name="endpoint">the endpoint of the client containing IP adress and the port number specified by the host</param>
        /// <returns>the input data send from the client</returns>
        Input EndReceivingInput(IAsyncResult inputResult, IPEndPoint endpoint);

        /// <summary>
        /// Closes the connection between the host and the client.
        /// </summary>
        void Close();
    }
}
