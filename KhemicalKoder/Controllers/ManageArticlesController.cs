using System;
using System.Linq;
using System.Threading.Tasks;
using KhemicalKoder.Data;
using KhemicalKoder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KhemicalKoder.Controllers
{
    public class ManageArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManageArticlesController(ApplicationDbContext context)
        {
            _context = context;
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
                await _context.SaveChangesAsync();
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Story")] Article article)
        {
            if (id != article.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(article);
                    await _context.SaveChangesAsync();
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