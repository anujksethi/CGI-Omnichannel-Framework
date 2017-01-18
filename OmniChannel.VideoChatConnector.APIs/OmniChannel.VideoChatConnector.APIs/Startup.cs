using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Owin;

namespace OmniChannel.VideoChatConnector.APIs
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HubConfiguration()
            {
                EnableJSONP = true
            };
            app.MapSignalR(config);
        }
    }
}