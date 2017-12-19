using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WTracking.Hubs
{
    public class Chat : Hub
    {
        public async Task Send(string message)
        {
            await Clients.All.InvokeAsync("Send", message);
        }
    }
}
