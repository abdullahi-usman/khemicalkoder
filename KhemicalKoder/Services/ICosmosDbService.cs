using KhemicalKoder.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KhemicalKoder.Services
{
    public interface ICosmosDbService
    {
        Task<IEnumerable<Article>> GetItemsAsync(string query);
        Task<Article> GetItemAsync(string id);
        Task AddItemAsync(Article item);
        Task UpdateItemAsync(string id, Article item);
        Task DeleteItemAsync(string id);
    }
}
