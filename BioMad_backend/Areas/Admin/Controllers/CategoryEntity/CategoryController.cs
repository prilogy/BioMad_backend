using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioMad_backend.Areas.Admin.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using BioMad_backend.Entities.ManyToMany;
using BioMad_backend.Extensions;
using X.PagedList;

namespace BioMad_backend.Areas.Admin.Controllers.CategoryEntity
{
    public class CategoryController : AdminController
    {
        private readonly ApplicationContext _context;

        public CategoryController(ApplicationContext context)
        {
            _context = context;
        }

        private IActionResult ReturnToId(int id) => RedirectToAction("Edit", new { id });

        public async Task<IActionResult> RemoveBiomarker(int biomarkerId, int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
                return RedirectToAction("Index");

            var toRemove = category.CategoryBiomarkers.FirstOrDefault(x => x.BiomarkerId == biomarkerId);
            if (toRemove == null)
                return ReturnToId(category.Id);

            _context.Remove(toRemove);
            await _context.SaveChangesAsync();
            
            return ReturnToId(category.Id);
        }
        
        public async Task<IActionResult> AddBiomarker(int biomarkerId, int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
                return RedirectToAction("Index");

            
            if(!category.Biomarkers.Any(x => x.Id == biomarkerId) && await _context.Biomarkers.AnyAsync(x => x.Id == biomarkerId))
                category.CategoryBiomarkers.Add(new CategoryBiomarker
                {
                    BiomarkerId = biomarkerId
                });
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Edit", new { id = category.Id });
        }

        // GET: Category
        public async Task<IActionResult> Index(int page = 1, string searchString = default)
        {
            var q = _context.Categories.AsQueryable();
            if (searchString != default)
            {
                q = q.SearchWithQuery<Category, CategoryTranslation>(searchString);
                ViewData["searchString"] = searchString;
            }

            var result = await q.ToPagedListAsync(page, 10);
            var lst = await q.ToListAsync();
            return View(result);
        }

        // GET: Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
