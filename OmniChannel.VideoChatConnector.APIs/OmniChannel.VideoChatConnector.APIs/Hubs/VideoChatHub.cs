using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OmniChannel.VideoChatConnector.APIs.Models.VideoChat;

namespace OmniChannel.VideoChatConnector.APIs.Hubs
{
    public class VideoChatHub : Hub
    {

        static List<CallerData> ConnectedCustomers = new List<CallerData>();
        static List<AgentData> ConnectedAgents = new List<AgentData>();
        private static string Order = "Order";
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
        /// <param name="pageContext"></param>
        public void GetAllConnectedCustomers(string pageContext)
        {
            if (ConnectedCustomers != null && ConnectedCustomers.Count > 0)
            {
                var hubId = Context.ConnectionId;
                var customers = ConnectedCustomers.All(x => x.CustomerDiallerKeyValid == true);
                var customersJson = JsonConvert.SerializeObject(customers);
                Clients.Client(hubId).getAllConnectedCustomers(customersJson);
            }
        }

        private void BroadCastCustomers(string agentExpertise)
        {
            //if (ConnectedCustomers.Count > 0)
            //{
            if (ConnectedAgents.Count(x => x.AgentExpertise == agentExpertise) == 1)
            {
                foreach (var customer in ConnectedCustomers)
                {
                    if (Convert.ToString(customer.PageContext) == Convert.ToString(agentExpertise))
                    {
                        Clients.Client(customer.CustomerHubId).checkForAgents(true);
                    }
                }
            }

            //    Clients.Group(Constants.Customer).checkForAgents(true);
            //}
        }
        /// <summary>
        /// 
        /// </summary>
        public void CheckForAgents(string enrollCustomer = "")
        {
            var hubId = Context.ConnectionId;
            string customerPageContext = string.Empty;
            if (ConnectedCustomers.FirstOrDefault(x => x.CustomerHubId == hubId) != null)
            {
                customerPageContext = ConnectedCustomers.Find(x => x.CustomerHubId == hubId).PageContext;
            }

            if (ConnectedAgents != null && ConnectedAgents.FirstOrDefault(x => x.AgentExpertise == customerPageContext) != null)
            {
                Clients.Client(hubId).checkForAgents(true);
            }
            else
            {
                Clients.Client(hubId).checkForAgents(false);
            }

            //if (ConnectedAgents!= null && ConnectedAgents.Count > 0)
            //{

            //}
            //else
            //{

            //}

            if (!string.IsNullOrEmpty(enrollCustomer) && enrollCustomer == "newCustomer")
            {
                Groups.Add(Context.ConnectionId, Constants.Customer);
            }

        }

        public void BroadcastCustomerToAgent(string pageContext)
        {
            var id = Context.ConnectionId;
            var customer = ConnectedCustomers.FirstOrDefault(x => x.CustomerHubId == id);
            if (customer != null)
            {
                customer.CustomerDiallerKeyValid = true;
                foreach (var agent in ConnectedAgents)
                {
                    if (Convert.ToString(agent.AgentExpertise) == Convert.ToString(pageContext))
                    {
                        Clients.Client(agent.AgentHubId).addCustomerToAgents(customer.CustomerName, customer.CustomerDiallerKey, Constants.ActionAdd);
                    }
                }
            }
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
                CustomerHubId = id,
                CustomerName = userName,
                PageContext = pageContext
                

            };
            //Groups.Add(Context.ConnectionId, pageContext);
            //foreach (var agent in ConnectedAgents)
            //{
            //    if (Convert.ToString(agent.AgentExpertise) == Convert.ToString(pageContext))
            //    {
            //        Clients.Client(agent.AgentHubId).addCustomerToAgents(registerCaller.CustomerName, registerCaller.CustomerDiallerKey, Constants.ActionAdd);
            //    }
            //}

            ConnectedCustomers.Add(registerCaller);
            CheckForAgents("newCustomer");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="webrtcDiallerKey"></param>
        /// <param name="agentExpertise"></param>
        public void ConnectAgent(string userName, string webrtcDiallerKey, string agentExpertise)
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

            GetAllConnectedCustomers(agentExpertise);
            ConnectedAgents.Add(registerCaller);
            //if (ConnectedAgents.Count == 1)
            //{
            BroadCastCustomers(agentExpertise);
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveCustomer()
        {
            var id = Context.ConnectionId;
            if (ConnectedCustomers.Count(x => x.CustomerHubId == id) > 0)
            {
                var customer = ConnectedCustomers.Find(x => x.CustomerHubId == id);

                Clients.Group(Constants.Agent).addCustomerToAgents(customer.CustomerName, customer.CustomerDiallerKey, Constants.ActionRemove);
                Groups.Remove(id, customer.PageContext);

                ConnectedCustomers.RemoveAll(item => item.CustomerHubId == id);
            }
        }

        public void DisableCustomer()
        {
            var id = Context.ConnectionId;
            if (ConnectedCustomers.Count(x => x.CustomerHubId == id) > 0)
            {
                var customer = ConnectedCustomers.Find(x => x.CustomerHubId == id);
                customer.CustomerDiallerKeyValid = false;
            //    BroadcastCustomerToAgent(customer.PageContext);
                 Clients.Group(Constants.Agent).addCustomerToAgents(customer.CustomerName, customer.CustomerDiallerKey, Constants.ActionRemove);
                //   Groups.Remove(id, customer.PageContext);

                //    ConnectedCustomers.RemoveAll(item => item.CustomerHubId == id);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveAgent()
        {
            var id = Context.ConnectionId;
            var agentContext = string.Empty;
            if (ConnectedAgents.Count(x => x.AgentHubId == id) > 0)
            {
                agentContext = ConnectedAgents.FirstOrDefault(x => x.AgentHubId == id).AgentExpertise;


                //   var agent = ConnectedAgents.Find(x => x.AgentHubId == id);
                //    Clients.Group(Constants.Agent).addCustomerToAgents(agent.CustomerName, agent.AgentDiallerKey, Constants.ActionRemove);
                Groups.Remove(id, Constants.Agent);

                ConnectedAgents.RemoveAll(item => item.AgentHubId == id);


                if (ConnectedAgents.Count(x=>x.AgentExpertise==agentContext) > 0)
                {
                    //Clients.Group(Constants.Customer).checkForAgents(true);
                    foreach (var customer in ConnectedCustomers)
                    {
                        if (Convert.ToString(customer.PageContext) == Convert.ToString(agentContext))
                        {
                            Clients.Client(customer.CustomerHubId).checkForAgents(true);
                        }
                    }
                }
                else
                {
                    foreach (var customer in ConnectedCustomers)
                    {
                        if (Convert.ToString(customer.PageContext) == Convert.ToString(agentContext))
                        {
                            Clients.Client(customer.CustomerHubId).checkForAgents(false);
                        }
                    }
                    //Clients.Group(Constants.Customer).checkForAgents(false);
                }
            }

            //remove the agent availability if the agents in customer context not available
            //if (ConnectedAgents.Count(x => x.AgentExpertise == agentContext) == 0)
            //{
            //    foreach (var customer in ConnectedCustomers)
            //    {
            //        if (Convert.ToString(customer.PageContext) == Convert.ToString(agentContext))
            //        {
            //            Clients.Client(customer.CustomerHubId).checkForAgents(false);
            //        }
            //    }
            //}






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
            //var id = Context.ConnectionId;
            //if (ConnectedCustomers.Count(x => x.CustomerHubId == id) > 0)
            //{
            //    var customer = ConnectedCustomers.Find(x => x.CustomerHubId == id);
            //    Clients.Group(Constants.Agent).addCustomerToAgents(customer.CustomerName, customer.CustomerDiallerKey, Constants.ActionRemove);
            //    Groups.Remove(id, customer.PageContext);

            //    ConnectedCustomers.RemoveAll(item => item.CustomerHubId == id);




            //}
            //if (ConnectedAgents.Count(x => x.AgentHubId == id) > 0)
            //{
            //    ConnectedAgents.RemoveAll(item => item.AgentHubId == id);
            //}
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