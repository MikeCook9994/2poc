﻿using System;
using System.Net;

namespace _2poc
{
    interface IInputClient
    {
        void SendInputAsync(string input);

        void BeginReceiveInput(AsyncCallback inputHandlerCallback);

        byte[] EndReceiveInput(IAsyncResult inputResult, IPEndPoint endpoint);

        void Close();
    }
}
