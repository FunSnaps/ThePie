using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using ThePie.Data;
using ThePie.Models;

namespace ThePie.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Author.Include(a => a.Poster);
            return View(await applicationDbContext.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Author
                .FirstOrDefaultAsync(m => m.id == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        [HttpGet]
        [Authorize]
        [Authorize(Roles = "Member")]
        public IActionResult Create()
        {
            ViewData["PosterId"] = new SelectList(_context.Poster, "id", "Name");
            return View();
        }

        [HttpPost]
        [Authorize]
        [Authorize(Roles = "Member")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,PosterId,AuthorSex,Active")] Author author)
        {
            if (ModelState.IsValid)
            {
                _context.Add(author);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PosterId"] = new SelectList(_context.Poster, "id", "Name", author.PosterId);
            return View(author);
        }

        [HttpGet]
        [Authorize]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Author.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            ViewData["PosterId"] = new SelectList(_context.Poster, "id", "Name", author.PosterId);
            return View(author);
        }

        [HttpPost]
        [Authorize]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,PosterId,AuthorSex,Active")] Author author)
        {
            if (id != author.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(author);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PosterId"] = new SelectList(_context.Poster, "id", "Name", author.PosterId);
            return View(author);
        }

        [HttpGet]
        [Authorize]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Author
                .FirstOrDefaultAsync(m => m.id == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        [Authorize]
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var author = await _context.Author.FindAsync(id);
            _context.Author.Remove(author);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ShowSearchResults(String SearchName, int SearchSex, String SearchStatus)
        {
            if (SearchName != null)
            {
                return View("Index", await _context.Author.Include(c => c.Poster)
                    .Where(author => author.Name.Contains(SearchName)).ToListAsync());
            }
            else 
            {
                //return View("Index", await _context.Author.Where(author => author.AuthorSex.Equals(SearchSex)).ToListAsync());
                return View("Index", await _context.Author.Include(c => c.Poster)
                    .Where(author => ((int)author.AuthorSex).Equals(SearchSex)).ToListAsync());
            }           
        }

        private bool AuthorExists(int id)
        {
            return _context.Author.Any(e => e.id == id);
        }
    }
}
