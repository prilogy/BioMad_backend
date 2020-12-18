using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BioMad_backend.Areas.Admin.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using BioMad_backend.Extensions;
using X.PagedList;

namespace BioMad_backend.Areas.Admin.Controllers
{
    public class UnitController : LocalizedEntityController<Unit, UnitTranslation>
    {
        public UnitController(ApplicationContext context) : base(context)
        {
            CultureInfo.CurrentCulture = Culture.En.Info;
            CultureInfo.CurrentUICulture = Culture.En.Info;
        }

        [HttpGet, ActionName("Edit")]
        public async Task<IActionResult> EditNew(int? id, string translationAction, int translationId, int cultureId)
        {
            return await base.Edit(id, translationAction, translationId, cultureId);
        }

        protected override IQueryable<Unit> Queryable => _context.Units.AsQueryable();

        [HttpPost]
        public async Task<IActionResult> CreateTransfer(UnitTransfer unitTransfer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(unitTransfer);
                await _context.SaveChangesAsync();
                return RedirectToEditById(unitTransfer.UnitAId);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditTransfer(int id, UnitTransfer unitTransfer)
        {
            if (id != unitTransfer.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(unitTransfer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await _context.UnitTransfers.AnyAsync(x => x.Id == unitTransfer.Id)))
                        return NotFound();
                    throw;
                }

                return RedirectToEditById(unitTransfer.UnitAId);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteTransfer(int id)
        {
            var unitTransfer = await _context.UnitTransfers.FindAsync(id);
            var unitId = unitTransfer.UnitAId;
            _context.UnitTransfers.Remove(unitTransfer);
            await _context.SaveChangesAsync();
            return RedirectToEditById(unitId);
        }
    }

    public class UnitTranslationController : NoViewTranslationController<Unit, UnitTranslation>
    {
        public UnitTranslationController(ApplicationContext context) : base(context)
        {
        }

        protected override IQueryable<Unit> Queryable => _context.Units.AsQueryable();

        public override IActionResult RedirectToBaseEntity(int id)
            => RedirectToAction("Edit", "Unit", new { id });
    }
}