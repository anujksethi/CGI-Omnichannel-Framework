using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OmniChannel.VideoChatConnector.APIs.Properties;
using MongoDB.Driver;
using OmniChannel.VideoChatConnector.APIs.Rentals;

namespace OmniChannel.VideoChatConnector.APIs.App_Start
{
    public class VideoConnectorContext
    {
        public MongoDatabase Database { get; set; }
        public VideoConnectorContext()
        {
            var client = new MongoClient(Settings.Default.OmniChannelConnectionString);
            var server = client.GetServer();
            Database = server.GetDatabase(Settings.Default.OmniChannelDatabaseName);
        }

        public MongoCollection<Rental> Rentals
        {
            get
            {
                return Database.GetCollection<Rental>("rentals");
            }
        }
    }
}