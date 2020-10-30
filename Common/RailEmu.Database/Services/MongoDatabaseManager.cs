using MongoDB.Driver;

namespace RailEmu.Database.Services
{
    public class MongoDatabaseManager
    {
        private readonly string connectionString = "mongodb://localhost:27017";
        private readonly MongoClient mongoClient;

        public MongoDatabaseManager()
        {
            mongoClient = new MongoClient(connectionString);
        }

        public IMongoDatabase GetDatabase()
            => mongoClient.GetDatabase("auth");
    }
}
