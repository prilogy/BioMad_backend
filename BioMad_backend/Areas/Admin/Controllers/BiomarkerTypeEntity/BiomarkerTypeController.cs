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
using BioMad_backend.Extensions;
using X.PagedList;

namespace BioMad_backend.Areas.Admin.Controllers.BiomarkerTypeEntity
{
    public class BiomarkerTypeController : AdminController
    {
        private readonly ApplicationContext _context;

        public BiomarkerTypeController(ApplicationContext context)
        {
            _context = context;
        }
        
        // GET: BiomarkerType
        public async Task<IActionResult> Index(int page = 1, string searchString = default)
        {
            var q = _context.BiomarkerTypes.AsQueryable();
            if (searchString != default)
            {
                q = q.SearchWithQuery<BiomarkerType, BiomarkerTypeTranslation>(searchString);
                ViewData["searchString"] = searchString;
            }

            var result = await q.ToPagedListAsync(page, 10);
            var lst = await q.ToListAsync();
            return View(result);
        }

        // GET: BiomarkerType/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var biomarkerType = await _context.BiomarkerTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (biomarkerType == null)
            {
                return NotFound();
            }

            return View(biomarkerType);
        }

        // GET: BiomarkerType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BiomarkerType/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] BiomarkerType biomarkerType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(biomarkerType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(biomarkerType);
        }

        // GET: BiomarkerType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var biomarkerType = await _context.BiomarkerTypes.FindAsync(id);
            if (biomarkerType == null)
            {
                return NotFound();
            }
            return View(biomarkerType);
        }

        // POST: BiomarkerType/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] BiomarkerType biomarkerType)
        {
            if (id != biomarkerType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(biomarkerType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BiomarkerTypeExists(biomarkerType.Id))
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
            return View(biomarkerType);
        }

        // GET: BiomarkerType/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var biomarkerType = await _context.BiomarkerTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (biomarkerType == null)
            {
                return NotFound();
            }

            return View(biomarkerType);
        }

        // POST: BiomarkerType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var biomarkerType = await _context.BiomarkerTypes.FindAsync(id);
            _context.BiomarkerTypes.Remove(biomarkerType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BiomarkerTypeExists(int id)
        {
            return _context.BiomarkerTypes.Any(e => e.Id == id);
        }
    }
}
