using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using OmniChannel.VideoChatConnector.APIs.Models.VideoChat;

namespace OmniChannel.VideoChatConnector.APIs.Hubs
{
    public class VideoChatHub : Hub
    {

        static List<CallerData> ConnectedCustomers = new List<CallerData>();
        static List<AgentData> ConnectedAgents = new List<AgentData>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Send(string message)
        {
            Clients.All.newMessage(message
                //   Context.User.Identity.Name+ " says " + message
                );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="webrtcDiallerKey"></param>
        /// <param name="pageContext"></param>
        public void ConnectCustomer(string userName, string webrtcDiallerKey, string pageContext)
        {
                var id = Context.ConnectionId;
            var registerCaller = new CallerData()
            {
                CustomerDiallerKey = webrtcDiallerKey,
                CustomerDiallerKeyValid = true,
                CustomerHubId = id,
                CustomerName = userName,
                PageContext = pageContext

            };
            Groups.Add(Context.ConnectionId, pageContext);
            Clients.Group(Constants.Agent).addCustomerToAgents(registerCaller.CustomerName, registerCaller.CustomerDiallerKey);
            ConnectedCustomers.Add(registerCaller);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="webrtcDiallerKey"></param>
        /// <param name="agentExpertise"></param>
        public void ConnectAgent(string userName, string webrtcDiallerKey,string agentExpertise)
        {
            var id = Context.ConnectionId;
            var registerCaller = new AgentData()
            {
                AgentDiallerKey = webrtcDiallerKey,
                AgentDiallerKeyValid = true,
                AgentHubId = id,
                AgentName = userName,
                AgentExpertise = agentExpertise
            };
            // JoinGroup("agent");

            Groups.Add(Context.ConnectionId, Constants.Agent);


            ConnectedAgents.Add(registerCaller);
        }

        //public async Task JoinGroup(string groupName)
        //{
        //    await Groups.Add(Context.ConnectionId, groupName);
        //    Clients.Group(groupname).addContosoChatMessageToPage(Context.ConnectionId + " added to group");
        //}


        public override Task OnConnected()
        {
            // Add your own code here.
            // For example: in a chat application, record the association between
            // the current connection ID and user name, and mark the user as online.
            // After the code in this method completes, the client is informed that
            // the connection is established; for example, in a JavaScript client,
            // the start().done callback is executed.
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            var id = Context.ConnectionId;
            if (ConnectedCustomers.Count(x => x.CustomerHubId == id) > 0)
            {
                ConnectedCustomers.RemoveAll(item => item.CustomerHubId == id);
            }
            if (ConnectedAgents.Count(x => x.AgentHubId == id) > 0)
            {
                ConnectedAgents.RemoveAll(item => item.AgentHubId == id);
            }
            // Add your own code here.
            // For example: in a chat application, mark the user as offline, 
            // delete the association between the current connection id and user name.
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            // Add your own code here.
            // For example: in a chat application, you might have marked the
            // user as offline after a period of inactivity; in that case 
            // mark the user as online again.
            return base.OnReconnected();
        }
    }


}