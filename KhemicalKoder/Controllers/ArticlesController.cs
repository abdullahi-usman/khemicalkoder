using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KhemicalKoder.Data;
using KhemicalKoder.Extensions;
using KhemicalKoder.Models;
using KhemicalKoder.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

using Newtonsoft.Json;

namespace KhemicalKoder.Controllers
{
   
    public class ArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        public ArticlesController(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        private async Task<List<Article>> GetArticlesAsync()
        {
            
            return (List<Article>)await _cache.GetOrCreateAsync(CacheKeys.Article, async entry =>
            {
                var articles = await _context.Article.ToListAsync();
                
                return articles;
            });
        }

        // GET: Articles
        public async Task<ActionResult> Index(string? id)
        {
            
            if (id == null)return View("ArticlesList", await GetArticlesAsync());

            var articles = await GetArticlesAsync();

            if (!articles.Any())
                return View("Error", "No Article");

            var article = articles.Find(predict => predict.id == id);

            if (article == null) return View("Error", "No Such Article");

            var articlePosition = articles.FindIndex(predict => predict.id == article.id);

            if (articles.Count > articlePosition + 1)
                ViewData["article-next"] = articles.ElementAt(articlePosition + 1).id;

            if (articlePosition > 0)
                ViewData["article-prev"] = articles.ElementAt(articlePosition - 1).id;

            return View(article);
        }


       
        public async Task<string> Search(string searchString)
        {
            List<Article> articles = await GetArticlesMatchingName(searchString);

            return JsonConvert.SerializeObject(articles);
        }

        private async Task<List<Article>> GetArticlesMatchingName(string searchString)
        {
            var articles = new List<Article>();
            
            if (searchString != null)
            {

                foreach (var article in await GetArticlesAsync())
                    if (article.Title.ToLowerInvariant().Contains(searchString.ToLowerInvariant(),
                        StringComparison.InvariantCultureIgnoreCase))
                        articles.Add(article);
            }

            return articles;
        }
        
       
        [HttpGet]
        public async Task<IActionResult> SearchOnView(string searchString)
        {
            List<Article> articles = await GetArticlesMatchingName(searchString);

            return View("ArticlesList", articles);
        }

        [HttpPost]
        public async Task<IActionResult> SearchOnView()
        {
            var form = this.Request.Form;
            StringValues searchString;
            form.TryGetValue("searchString", out searchString);
            
            List<Article> articles = await GetArticlesMatchingName(searchString[0]);

            return View("ArticlesList", articles);
        }
    }
}