

using KhemicalKoder.Models;

using Microsoft.Azure.Cosmos;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KhemicalKoder.Services
{
    public class CosmosDbService : ICosmosDbService
    {
        private Container _container;
        public CosmosDbService(CosmosClient cosmosClient, string database, string containerName)
        {
            
            this._container = cosmosClient.GetContainer(database, containerName);
        }
        public Task AddItemAsync(Article article)
        {
          
            return this._container.CreateItemAsync<Article>(article, new PartitionKey(article.id));
        }

        public Task DeleteItemAsync(string id)
        {
            return this._container.DeleteItemAsync<Article>(id, new PartitionKey(id));
        }

        public async Task<Article> GetItemAsync(string id)
        {
            try
            {
                ItemResponse<Article> itemResponse = await this._container.ReadItemAsync<Article>(id, new PartitionKey(id));
                return itemResponse.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

        }

        public async Task<IEnumerable<Article>> GetItemsAsync(string query)
        {
            var queryIterator = this._container.GetItemQueryIterator<Article>(new QueryDefinition(query));
          
            List<Article> items = new List<Article>();
            while (queryIterator.HasMoreResults)
            {
               
                var response = await queryIterator.ReadNextAsync();

                items.AddRange(response.ToList());
            }

            return items;
        }

        public async Task UpdateItemAsync(string id, Article article)
        {
            await this._container.UpsertItemAsync<Article>(article, new PartitionKey(id));
        }
    }
}
