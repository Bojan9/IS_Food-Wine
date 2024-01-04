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
    public class TutorialsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TutorialsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tutorials
        public async Task<IActionResult> Index(string searchString, string filterType)
        {
            var tutorials = from t in _context.Tutorial
                          select t;

            if (!string.IsNullOrEmpty(searchString))
            {
                tutorials = tutorials.Where(t => t.TutorialName.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(filterType))
            {
                Enum.TryParse(filterType, out TutorialCategory type);
                tutorials = tutorials.Where(t => t.Category == type);
            }

            return View(await tutorials.ToListAsync());
        }

        // GET: Tutorials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tutorial == null)
            {
                return NotFound();
            }

            var tutorial = await _context.Tutorial
                .FirstOrDefaultAsync(m => m.TutorialId == id);
            if (tutorial == null)
            {
                return NotFound();
            }

            return View(tutorial);
        }

        // GET: Tutorials/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewBag.TutorialCategoryList = new SelectList(Enum.GetValues(typeof(TutorialCategory)).Cast<TutorialCategory>());
            return View();
        }

        // POST: Tutorials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("TutorialId,TutorialName,TutorialDescription,TutorialVideoUrl,Category,CreatedBy")] Tutorial tutorial)
        {
            if (ModelState.IsValid)
            {
                tutorial.CreatedBy = User.Identity.Name;

                _context.Add(tutorial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tutorial);
        }

        // GET: Tutorials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tutorial == null)
            {
                return NotFound();
            }

            var tutorial = await _context.Tutorial.FindAsync(id);
            if (tutorial == null)
            {
                return NotFound();
            }
            ViewBag.TutorialCategoryList = new SelectList(Enum.GetValues(typeof(TutorialCategory)).Cast<TutorialCategory>());

            if (tutorial == null || !UserIsAuthorized(tutorial.CreatedBy))
            {
                return Forbid();
            }

            return View(tutorial);
        }

        // POST: Tutorials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TutorialId,TutorialName,TutorialDescription,TutorialVideoUrl,Category,CreatedBy")] Tutorial tutorial)
        {
            if (id != tutorial.TutorialId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tutorial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TutorialExists(tutorial.TutorialId))
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
            return View(tutorial);
        }

        // GET: Tutorials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tutorial == null)
            {
                return NotFound();
            }

            var tutorial = await _context.Tutorial
                .FirstOrDefaultAsync(m => m.TutorialId == id);
            if (tutorial == null)
            {
                return NotFound();
            }

            if (tutorial == null || !UserIsAuthorized(tutorial.CreatedBy))
            {
                return Forbid();
            }

            return View(tutorial);
        }

        // POST: Tutorials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tutorial == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Tutorial'  is null.");
            }
            var tutorial = await _context.Tutorial.FindAsync(id);
            if (tutorial != null)
            {
                _context.Tutorial.Remove(tutorial);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TutorialExists(int id)
        {
          return (_context.Tutorial?.Any(e => e.TutorialId == id)).GetValueOrDefault();
        }
        private bool UserIsAuthorized(string createdBy)
        {
            // Check if the current logged-in user is the creator of the recipe
            return User.Identity.Name == createdBy || User.Identity.Name == "admin@mail.com";
        }
    }
}
