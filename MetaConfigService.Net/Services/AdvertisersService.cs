using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;

namespace MetaConfigService.Net.Services
{
    public class AdvertisersService : IAdvertisersService
    {
        public async Task<List<BsonDocument>> GetAdvertisers()
        {
            var mongoClient = new MongoClient(ConfigurationManager.ConnectionStrings["mongoDbConnection"].ConnectionString);
            var mongoDb = mongoClient.GetDatabase(ConfigurationManager.AppSettings["mongoDbName"]);
            var collection = await mongoDb.GetCollection<BsonDocument>("advertisers").Find(new BsonDocument()).ToListAsync();

            return collection;
        }
    }

    public interface IAdvertisersService
    {
        Task<List<BsonDocument>> GetAdvertisers();
    }
}