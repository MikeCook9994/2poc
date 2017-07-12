﻿using System.Net;
using System.Threading.Tasks;

namespace _2pok.interfaces
{
    public interface IInputSender
    {
        Task<int> SendKeyboardInputAsync(KeyboardInput keyboardInput);

        Task<int> SendMouseInputAsync(MouseInput mouseInput);

        void Connect(IPAddress hostIp, int portNumber);

        void Close();
    }
}
