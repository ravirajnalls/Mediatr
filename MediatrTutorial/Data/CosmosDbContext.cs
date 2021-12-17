using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using MediatrTutorial.Domain;

namespace MediatrTutorial.Data
{
    public interface ICosmosDbContext
    {
        Task AddItemAsync(BaseModelMetaData data);
        Task DeleteItemAsync(string projectId, string partitionKey);
        Task<BaseModelMetaData> GetItemAsync(string projectId, string partitionKey);
        Task<IEnumerable<BaseModelMetaData>> GetItemsAsync(string queryString);
        Task UpdateItemAsync(BaseModelMetaData data, string partitionKey);

    }

    public class CosmosDbContext : ICosmosDbContext
    {
        private Container _container;

        public CosmosDbContext(IConfiguration configurationSection)
        {
            string databaseName = configurationSection.GetSection("DatabaseName").Value;
            string containerName = configurationSection.GetSection("ContainerName").Value;
            string account = configurationSection.GetSection("Account").Value;
            string key = configurationSection.GetSection("Key").Value;
            CosmosClient dbClient = new CosmosClient(account, key);
            this._container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task AddItemAsync(BaseModelMetaData data)
        {
            await this._container.CreateItemAsync<BaseModelMetaData>(data, new PartitionKey(data.Tags));
        }

        public async Task DeleteItemAsync(string projectId, string partitionKey)
        {
            await this._container.DeleteItemAsync<BaseModelMetaData>(projectId, new PartitionKey(partitionKey));
        }

        public async Task<BaseModelMetaData> GetItemAsync(string projectId, string partitionKey)
        {
            try
            {
                ItemResponse<BaseModelMetaData> response = await this._container.ReadItemAsync<BaseModelMetaData>(projectId, new PartitionKey(partitionKey));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<IEnumerable<BaseModelMetaData>> GetItemsAsync(string queryString)
        {
            var query = this._container.GetItemQueryIterator<BaseModelMetaData>(new QueryDefinition(queryString));
            List<BaseModelMetaData> results = new List<BaseModelMetaData>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateItemAsync(BaseModelMetaData data, string partitionKey)
        {
            await this._container.UpsertItemAsync<BaseModelMetaData>(data, new PartitionKey(partitionKey));
        }
    }
}
