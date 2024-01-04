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
    public class RecipesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecipesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Recipes
        public async Task<IActionResult> Index(string searchString, string filterType)
        {
            var recipes = from r in _context.Recipe
                          select r;

            if (!string.IsNullOrEmpty(searchString))
            {
                recipes = recipes.Where(r => r.Name.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(filterType))
            {
                Enum.TryParse(filterType, out RecipeType type);
                recipes = recipes.Where(r => r.Type == type);
            }

            return View(await recipes.ToListAsync());
        }

        // GET: Recipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Recipe == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipe
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // GET: Recipes/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewBag.TypeList = new SelectList(Enum.GetValues(typeof(RecipeType)).Cast<RecipeType>());
            ViewBag.FoodTypeList = new SelectList(Enum.GetValues(typeof(FoodType)).Cast<FoodType>());
            return View();
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Preparation,Ingredients,ImageUrl,CaloryCount,Type,FoodType,IsAlcoholic,CreatedBy")] Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                recipe.CreatedBy = User.Identity.Name;

                _context.Add(recipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recipe);
        }

        // GET: Recipes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Recipe == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipe.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            if (recipe == null || !UserIsAuthorized(recipe.CreatedBy))
            {
                return Forbid();
            }

            // Retrieve options for Type and FoodType fields
            ViewBag.TypeList = new SelectList(Enum.GetValues(typeof(RecipeType)).Cast<RecipeType>());
            ViewBag.FoodTypeList = new SelectList(Enum.GetValues(typeof(FoodType)).Cast<FoodType>());

            return View(recipe);
        }


        // POST: Recipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Preparation,Ingredients,ImageUrl,CaloryCount,Type,FoodType,IsAlcoholic,CreatedBy")] Recipe recipe)
        {
            if (id != recipe.Id)
            {
                return NotFound();
            }

            if (!UserIsAuthorized(recipe.CreatedBy))
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.Id))
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
            return View(recipe);
        }

        // GET: Recipes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Recipe == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipe
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            if (recipe == null || !UserIsAuthorized(recipe.CreatedBy))
            {
                return Forbid();
            }

            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Recipe == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Recipe'  is null.");
            }
            var recipe = await _context.Recipe.FindAsync(id);
            if (recipe != null)
            {
                _context.Recipe.Remove(recipe);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeExists(int id)
        {
          return (_context.Recipe?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private bool UserIsAuthorized(string createdBy)
        {
            // Check if the current logged-in user is the creator of the recipe
            return User.Identity.Name == createdBy || User.Identity.Name == "admin@mail.com";
        }
    }
}
