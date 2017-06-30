using System;
using System.Net;
using System.Windows.Forms;

namespace _2pok.interfaces
{
    interface IInputReceiver
    {
        void BeginReceivingInput(AsyncCallback inputHandlerCallback);

        KeyboardInput EndReceivingKeyboardInput(IAsyncResult inputResult, IPEndPoint endpoint);

        byte[] EndReceivingMouseInput(IAsyncResult inputResult, IPEndPoint endpoint);

        void Close();
    }
}
