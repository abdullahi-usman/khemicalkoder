using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KhemicalKoder.Data;
using KhemicalKoder.Extensions;
using KhemicalKoder.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace KhemicalKoder.Controllers
{
    [Authorize("IsAdmin")]
    public class ManageArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        public ManageArticlesController(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: ManageArticles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Article.ToListAsync());
        }

        // GET: ManageArticles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var article = await _context.Article
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null) return NotFound();

            return View(article);
        }

        // GET: ManageArticles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ManageArticles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Story")] Article article)
        {
            if (ModelState.IsValid)
            {
                article.Date = DateTime.Now;
                _context.Add(article);

                if ((await _context.SaveChangesAsync()) > 0)
                {
                    var articles = _context.Article.ToList();
                    articles.Reverse();
                    _cache.Set(CacheKeys.Article, articles);   
                };


                return RedirectToAction(nameof(Index));
            }

            return View(article);
        }

        // GET: ManageArticles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var article = await _context.Article.FindAsync(id);
            if (article == null) return NotFound();
            return View(article);
        }

        // POST: ManageArticles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Title,Story")] Article article)
        {
            if (id != article.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var dbArticle = await _context.Article.FindAsync(id);
                    dbArticle.Title = article.Title;
                    dbArticle.Story = article.Story;
                    _context.Update(dbArticle);

                    if ((await _context.SaveChangesAsync()) > 0)
                    {
                        var articles = _context.Article.ToList();
                        articles.Reverse();
                        _cache.Set(CacheKeys.Article, articles);
                    };

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.Id))
                        return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(article);
        }

        // GET: ManageArticles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var article = await _context.Article
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null) return NotFound();

            return View(article);
        }

        // POST: ManageArticles/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _context.Article.FindAsync(id);
            _context.Article.Remove(article);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(int id)
        {
            return _context.Article.Any(e => e.Id == id);
        }
    }
}