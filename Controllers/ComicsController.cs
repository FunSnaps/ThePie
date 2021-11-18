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
    public class ComicsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComicsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Comic.Include(c => c.Author).Include(c => c.Publisher).Include(c => c.Poster);
            return View(await applicationDbContext.ToListAsync());
        }

        //[HttpGet]
        //public async Task<IActionResult> Index(String comicSearch)
        //{
        //    ViewData["GetComics"] = comicSearch;
        //    var comicQuery = from x in _context.Comic select x ;
        //    if (!String.IsNullOrEmpty(comicSearch))
        //    {
        //        comicQuery = comicQuery.Where(
        //        x => x.Title.Contains(comicSearch) ||
        //        x.Author.Name.Contains(comicSearch) ||
        //        x.Publisher.Name.Contains(comicSearch) ||
        //        x.Poster.Name.Contains(comicSearch) ||
        //        x.ReleaseDate.Equals(comicSearch) ||
        //        x.AgeRating.Equals(comicSearch));
        //    }
        //    return View(await comicQuery.AsNoTracking().ToListAsync());
        //}

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comic = await _context.Comic
                .Include(c => c.Author)
                .Include(c => c.Poster)
                .Include(c => c.Publisher)
                .FirstOrDefaultAsync(m => m.id == id);
            if (comic == null)
            {
                return NotFound();
            }

            return View(comic);
        }

        [HttpGet]
        [Authorize]
        [Authorize(Roles = "Member")]
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Author, "id", "Name");
            ViewData["PublisherId"] = new SelectList(_context.Publisher, "id", "Name");
            ViewData["PosterId"] = new SelectList(_context.Poster, "id", "Name");
            return View();
        }

        [HttpPost]
        [Authorize]
        [Authorize(Roles = "Member")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Title,AuthorId,PublisherId,PosterId,ReleaseDate,ComicImage,AgeRating,Price")] Comic comic)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Author, "id", "Name", comic.AuthorId);
            ViewData["PublisherId"] = new SelectList(_context.Publisher, "id", "Name", comic.PublisherId);
            ViewData["PosterId"] = new SelectList(_context.Poster, "id", "Name", comic.PosterId);
            return View(comic);
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

            var comic = await _context.Comic.FindAsync(id);
            if (comic == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Author, "id", "Name", comic.AuthorId);
            ViewData["PublisherId"] = new SelectList(_context.Publisher, "id", "Name", comic.PublisherId);
            ViewData["PosterId"] = new SelectList(_context.Poster, "id", "Name", comic.PosterId);
            return View(comic);
        }

        [HttpPost]
        [Authorize]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Title,AuthorId,PublisherId,PosterId,ReleaseDate,ComicImage,AgeRating,Price")] Comic comic)
        {
            if (id != comic.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComicExists(comic.id))
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
            ViewData["AuthorId"] = new SelectList(_context.Author, "id", "Name", comic.AuthorId);
            ViewData["PublisherId"] = new SelectList(_context.Publisher, "id", "Name", comic.PublisherId);
            ViewData["PosterId"] = new SelectList(_context.Poster, "id", "Name", comic.PosterId);
            return View(comic);
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

            var comic = await _context.Comic
                .Include(c => c.Author)
                .Include(c => c.Poster)
                .Include(c => c.Publisher)
                .FirstOrDefaultAsync(m => m.id == id);
            if (comic == null)
            {
                return NotFound();
            }

            return View(comic);
        }

        [Authorize]
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comic = await _context.Comic.FindAsync(id);
            _context.Comic.Remove(comic);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ShowSearchResults(String SearchTitle, String SearchAuthor, String SearchPublisher)
        {
            if (SearchTitle != null)
            {
                return View("Index", await _context.Comic.Include(c => c.Author).Include(c => c.Publisher).Include(c => c.Poster)
                    .Where(comic => comic.Title.Contains(SearchTitle)).ToListAsync());
            }
            else if (SearchAuthor != null)
            {
                return View("Index", await _context.Comic.Include(c => c.Author).Include(c => c.Publisher).Include(c => c.Poster)
                    .Where(comic => comic.Author.Name.Contains(SearchAuthor)).ToListAsync());
            }
            else 
            {
                return View("Index", await _context.Comic.Include(c => c.Author).Include(c => c.Publisher).Include(c => c.Poster)
                    .Where(comic => comic.Publisher.Name.Contains(SearchPublisher)).ToListAsync());
            }
        }

        private bool ComicExists(int id)
        {
            return _context.Comic.Any(e => e.id == id);
        }
    }
}
