using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace OmniChannel.VideoChatConnector.APIs.Hubs
{
    public class PerfHub : Hub
    {
        public void Send(string message)
        {
            Clients.All.newMessage(message
             //   Context.User.Identity.Name+ " says " + message
                );
        }
    }
}