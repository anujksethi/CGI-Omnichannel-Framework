using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OmniChannel.VideoChatConnector.APIs.Models.VideoChat
{
    public class AgentData
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string AgentName { get; set; }
        public string AgentExpertise { get; set; }
        public string AgentAvailability { get; set; }
        public string AgentDiallerKey { get; set; }
        public string AgentHubId { get; set; }
        public bool AgentDiallerKeyValid { get; set; }
        private bool _agentStatus = true;

        public bool AgentStatus
        {
            get { return _agentStatus; }
            set { _agentStatus = value; }
        }
    }
}