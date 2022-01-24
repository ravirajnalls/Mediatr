using MediatrTutorial.Domain;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediatrTutorial.Data
{
    public interface IMongoDbContext
    {
        Task<BaseModelMetaData> Create(BaseModelMetaData data);
        Task<IList<BaseModelMetaData>> ListAll();
        Task<List<BaseModelMetaData>> FindAllVersionsByModelMetaDataId(string modelMetaDataId);
        Task Update(BaseModelMetaData data);
        //Task<BaseModelMetaData> Find(string id);
        //Task Update(BaseModelMetaData data);
        //Task Delete(string id);
    }

    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase db;
        private readonly IMongoCollection<BaseModelMetaData> _models;

        public MongoDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetSection("ConnectionString").Value);
            this.db = client.GetDatabase(configuration.GetSection("Database").Value);
            _models = db.GetCollection<BaseModelMetaData>(configuration.GetSection("Collection").Value);
        }

        public async Task<BaseModelMetaData> Create(BaseModelMetaData data)
        {
            await _models.InsertOneAsync(data);
            return data;
        }

        public async Task<IList<BaseModelMetaData>> ListAll() =>
            (await _models.FindAsync(model => true)).ToList();

        public async Task<List<BaseModelMetaData>> FindAllVersionsByModelMetaDataId(string modelMetaDataId)
        {
            return (await _models.FindAsync(model => model.ModelMetaDataId == modelMetaDataId)).ToList();
        }

        public async Task Update(BaseModelMetaData data)
        {
            await _models.ReplaceOneAsync(model => model.Id == data.Id, data);
        }

        //public async Task Delete(string id) =>
        //   await _models.DeleteOneAsync(model => model.Id == id);
    }
}
