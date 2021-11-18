using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ThePie.Data;
using ThePie.Models;

namespace ThePie.Controllers
{
    public class PostersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public PostersController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Poster.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var poster = await _context.Poster
                .FirstOrDefaultAsync(m => m.id == id);
            if (poster == null)
            {
                return NotFound();
            }

            return View(poster);
        }

        [HttpGet]
        [Authorize]
        [Authorize(Roles = "Member")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [Authorize(Roles = "Member")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,Photo")] Poster poster)
        {
            if (ModelState.IsValid)
            {
                //saves the uploaded image in wwwRoot folder
                string wwwRootPath = webHostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(poster.Photo.FileName);
                string extension = Path.GetExtension(poster.Photo.FileName);
                poster.ImageName = fileName = fileName + DateTime.Now.ToString("yyymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/images/", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await poster.Photo.CopyToAsync(fileStream);
                }


                    _context.Add(poster);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(poster);
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

            var poster = await _context.Poster.FindAsync(id);
            if (poster == null)
            {
                return NotFound();
            }
            return View(poster);
        }

        [HttpPost]
        [Authorize]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,ImageName")] Poster poster)
        {
            if (id != poster.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(poster);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PosterExists(poster.id))
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
            return View(poster);
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

            var poster = await _context.Poster
                .FirstOrDefaultAsync(m => m.id == id);
            if (poster == null)
            {
                return NotFound();
            }

            return View(poster);
        }

        [Authorize]
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var poster = await _context.Poster.FindAsync(id);

            //deleting the image from the images folder
            var imagePath = Path.Combine(webHostEnvironment.WebRootPath, "images", poster.ImageName);

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }


            _context.Poster.Remove(poster);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PosterExists(int id)
        {
            return _context.Poster.Any(e => e.id == id);
        }
    }
}
