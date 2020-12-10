using System.Linq;
using System.Threading.Tasks;
using KhemicalKoder.Data;
using Microsoft.AspNetCore.Mvc;

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

            if (id == null) return View(_context.Article.FirstOrDefault());

            var article = await _context.Article.FindAsync(id);

            if (article == null) View("Error", "No Such Article");

            return View(article);
        }

        public IActionResult Error(string message)
        {
            return View(model: message);
        }
    }
}