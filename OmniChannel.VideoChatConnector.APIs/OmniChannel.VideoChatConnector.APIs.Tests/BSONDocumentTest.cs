using MongoDB.Bson;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;

namespace OmniChannel.VideoChatConnector.APIs.Tests
{
    
    class BSONDocumentTest
    {
        public BSONDocumentTest()
        {
            JsonWriterSettings.Defaults.Indent = true;
        }
        [Test]  
        public void EmptyBson()
        {
            var document = new BsonDocument();
            Console.WriteLine(document);
        }

        public class Person
        {
            public string Name;
            public int Age;
            public  List<string> AddressList = new List<string>();
            public  Contact ContactDetails = new Contact();
        }
        public class  Contact
        {
             
            public string Email;
            public string Phone;
        }
        [Test]
        public void Automatic()
        {
            var person = new Person()
            {
                Age = 54,
                Name = "Anuj"
            };
            person.AddressList.Add("1081 ff");

            person.AddressList.Add("23 main 11 cross");

            person.AddressList.Add("Sector 1 HSR Layout");

            person.ContactDetails.Email = "anuj.sethi@cgi.com";
            person.ContactDetails.Phone = "9740148598";

            Console.WriteLine(person.ToJson());
        }
        [Test]
        public void BsonValueTest()
        {
            var person = new BsonDocument
            {
                { "age", 53}
            };
            var bson = person.ToBson();
            var desBsonDoc = BsonSerializer.Deserialize<BsonDocument>(bson);
            Console.WriteLine(desBsonDoc);
            Console.WriteLine(person["age"].AsInt32+10);
            Console.WriteLine(person["age"].IsInt32);
            Console.WriteLine(person["age"].IsString);
        }
    }
}
