using System;
using System.Net;

namespace _2pok.interfaces
{
    public interface IInputReceiver
    {
        void BeginReceivingInput(AsyncCallback inputHandlerCallback);

        KeyboardInput EndReceivingKeyboardInput(IAsyncResult inputResult, IPEndPoint endpoint);

        MouseInput EndReceivingMouseInput(IAsyncResult inputResult, IPEndPoint endpoint);

        void Close();
    }
}
