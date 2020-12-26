using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KhemicalKoder.Data;
using KhemicalKoder.Extensions;
using KhemicalKoder.Models;
using KhemicalKoder.Services;

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
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null) return NotFound();

            var article = await _context.Article.FindAsync(id);
                
            if (article == null) return NotFound();

            return View(article);
        }

        // GET: ManageArticles/Create
        public IActionResult Create()
        {
            return View();
        }

        private void UpdateCache()
        {
            var articles = from Article _article in _context.Article orderby _article.Date descending select _article;
            _cache.Set(CacheKeys.Article, articles);
        }

        // POST: ManageArticles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Story")] Article article)
        {
            if (ModelState.IsValid)
            {

                article.id = Guid.NewGuid().ToString();
                article.Date = DateTime.Now;
                _context.Article.Add(article);
                await _context.SaveChangesAsync();

                UpdateCache();

                return RedirectToAction(nameof(Index));
            }

            return View(article);
        }

        // GET: ManageArticles/Edit/5
        public async Task<IActionResult> Edit(string? id)
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
        public async Task<IActionResult> Edit(string id, [Bind("id,Date,Title,Story")] Article article)
        {
            if (id != article.id) return NotFound();

            if (ModelState.IsValid)
            {
                IEnumerable<Article> articles = null;
                try
                {
                    var dbArticle = await _context.Article.FindAsync(id);
                    dbArticle.Title = article.Title;
                    dbArticle.Story = article.Story;
                    _context.Update(dbArticle);

                    UpdateCache();

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                    if (articles == null || !articles.Any(e => e.id == id))
                        return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(article);
        }

        // GET: ManageArticles/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null) return NotFound();

            var article = await _context.Article.FindAsync(id);
                
            if (article == null) return NotFound();

            return View(article);
        }

        // POST: ManageArticles/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var article = await _context.Article.FindAsync(id);
            if (article != null)
            {
                _context.Article.Remove(article);
                await _context.SaveChangesAsync();

                UpdateCache();
            }
            
            return RedirectToAction(nameof(Index));
        }

       
    }
}