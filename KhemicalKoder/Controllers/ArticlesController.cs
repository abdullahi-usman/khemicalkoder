using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KhemicalKoder.Data;
using KhemicalKoder.Models;
using Microsoft.AspNetCore.Mvc;
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

            if (id == null) article = _context.Article.FirstOrDefault();
            else
                article = await _context.Article.FindAsync(id);

            if (article == null) return View("Error", "No Such Article");

            var articles = _context.Article.ToList();
            var articlePosition = articles.FindIndex(predict => predict.Id == article.Id);

            if (articles.Count > articlePosition + 1)
                ViewData["article-next"] = articles.ElementAt(articlePosition + 1).Id;

            if (articlePosition > 0)
                ViewData["article-prev"] = articles.ElementAt(articlePosition - 1).Id;

            return View(article);
        }

        public async Task<string> Search(string searchString)
        {
            var articles = new List<Article>();

            await foreach (var article in _context.Article.AsAsyncEnumerable())
                if (article.Title.ToLowerInvariant().Contains(searchString.ToLowerInvariant(),
                    StringComparison.InvariantCultureIgnoreCase))
                    articles.Add(article);


            return JsonConvert.SerializeObject(articles);
        }
    }
}