﻿using System.Net;
using System.Net.Sockets;

namespace _2poc
{
    class UdpState
    {
        public UdpClient client;
        public IPEndPoint endpoint;

        public UdpState(UdpClient client, IPEndPoint endpoint)
        {
            this.client = client;
            this.endpoint = endpoint;
        }
    }
}
