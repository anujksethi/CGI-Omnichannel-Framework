using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OmniChannel.VideoChatConnector.APIs.Models.VideoChat
{
    public class CallerData
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerDiallerKey { get; set; }
        public string PageContext { get; set; }
        public string CustomerHubId { get; set; }
        public bool CustomerDiallerKeyValid { get; set; }
    }
}