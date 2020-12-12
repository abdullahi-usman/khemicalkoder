using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KhemicalKoder.Data;
using KhemicalKoder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

using Newtonsoft.Json;

namespace KhemicalKoder.Controllers
{
   
    public class ArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArticlesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Articles
        public async Task<ActionResult> Index(int? id)
        {
            if (!_context.Article.Any())
                return View("Error", "No Article");

            Article article;
            var articles = _context.Article.ToList();
            articles.Reverse();

            if (id == null) {
                //article = articles.First();
                return View("ArticlesList", articles);
            }
            else
                article = await _context.Article.FindAsync(id);

            if (article == null) return View("Error", "No Such Article");

            
            var articlePosition = articles.FindIndex(predict => predict.Id == article.Id);

            if (articles.Count > articlePosition + 1)
                ViewData["article-next"] = articles.ElementAt(articlePosition + 1).Id;

            if (articlePosition > 0)
                ViewData["article-prev"] = articles.ElementAt(articlePosition - 1).Id;

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

                await foreach (var article in _context.Article.AsAsyncEnumerable())
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