using Hisabwala.Core.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Hisabwala.Infrastructure
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration config)
        {
            var client = new MongoClient(config["Mongo:ConnectionString"]);
            _database = client.GetDatabase(config["Mongo:DatabaseName"]);
        }

        public IMongoCollection<Party> Parties =>
            _database.GetCollection<Party>("Parties");
    }

}
