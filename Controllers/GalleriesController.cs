using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bojan_recipe.Data;
using bojan_recipe.Models;
using Microsoft.AspNetCore.Authorization;

namespace bojan_recipe.Controllers
{
    public class GalleriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GalleriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Galleries
        public async Task<IActionResult> Index()
        {
              return _context.Gallery != null ? 
                          View(await _context.Gallery.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Gallery'  is null.");
        }

        // GET: Galleries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Gallery == null)
            {
                return NotFound();
            }

            var gallery = await _context.Gallery
                .FirstOrDefaultAsync(m => m.GalleryId == id);
            if (gallery == null)
            {
                return NotFound();
            }

            return View(gallery);
        }

        // GET: Galleries/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Galleries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("GalleryId,GalleryName,GalleryDescription,GalleryImageUrl,CreatedBy")] Gallery gallery)
        {
            if (ModelState.IsValid)
            {
                gallery.CreatedBy = User.Identity.Name;

                _context.Add(gallery);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gallery);
        }

        // GET: Galleries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Gallery == null)
            {
                return NotFound();
            }

            var gallery = await _context.Gallery.FindAsync(id);
            if (gallery == null)
            {
                return NotFound();
            }

            if (gallery == null || !UserIsAuthorized(gallery.CreatedBy))
            {
                return Forbid();
            }

            return View(gallery);
        }

        // POST: Galleries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GalleryId,GalleryName,GalleryDescription,GalleryImageUrl,CreatedBy")] Gallery gallery)
        {
            if (id != gallery.GalleryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gallery);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GalleryExists(gallery.GalleryId))
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
            return View(gallery);
        }

        // GET: Galleries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Gallery == null)
            {
                return NotFound();
            }

            var gallery = await _context.Gallery
                .FirstOrDefaultAsync(m => m.GalleryId == id);
            if (gallery == null)
            {
                return NotFound();
            }

            if (gallery == null || !UserIsAuthorized(gallery.CreatedBy))
            {
                return Forbid();
            }

            return View(gallery);
        }

        // POST: Galleries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Gallery == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Gallery'  is null.");
            }
            var gallery = await _context.Gallery.FindAsync(id);
            if (gallery != null)
            {
                _context.Gallery.Remove(gallery);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GalleryExists(int id)
        {
          return (_context.Gallery?.Any(e => e.GalleryId == id)).GetValueOrDefault();
        }

        private bool UserIsAuthorized(string createdBy)
        {
            // Check if the current logged-in user is the creator of the recipe
            return User.Identity.Name == createdBy;
        }
    }
}
