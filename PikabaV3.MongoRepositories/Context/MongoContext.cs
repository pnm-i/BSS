using System.Configuration;
using MongoDB.Driver;
using PikabaV3.Models.Entities;

namespace PikabaV3.MongoRepositories.Context
{
    public class MongoContext
    {
        private MongoClient client;
        private MongoServer server;
        private MongoDatabase database;

        public MongoContext()
        {
            var cnStr = new MongoConnectionStringBuilder
                (ConfigurationManager.ConnectionStrings["PikabaV3DB"].ConnectionString);
            client = new MongoClient(cnStr.ConnectionString);
            server = client.GetServer();
            database = server.GetDatabase(cnStr.DatabaseName);
        }

        public MongoCollection<Product> Products
        {
            get
            {
                return database.GetCollection<Product>("Products");
            }
        }

        public MongoCollection<User> Users
        {
            get
            {
                return database.GetCollection<User>("Users");
            }
        }

        public MongoCollection<Seller> Sellers
        {
            get
            {
                return database.GetCollection<Seller>("Users");
            }
        }

        public MongoCollection<Category> Categories
        {
            get
            {
                return database.GetCollection<Category>("Categories");
            }
        }

        public MongoCollection<UserSession> Sessions
        {
            get
            {
                return database.GetCollection<UserSession>("Sessions");
            }
        }
        
        public void DropDatabase()
        {
            database.Drop();
        }

        public void DropCollection(string collName)
        {
            database.DropCollection(collName);
        }
    }
}