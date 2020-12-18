using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BioMad_backend.Areas.Admin.Helpers;
using BioMad_backend.Areas.Admin.Models;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BioMad_backend.Areas.Admin.Controllers
{
    public class BiomarkerController : LocalizedEntityController<Biomarker, BiomarkerTranslation>
    {
        public BiomarkerController(ApplicationContext context) : base(context)
        {
            CultureInfo.CurrentCulture = Culture.En.Info;
            CultureInfo.CurrentUICulture = Culture.En.Info;
        }

        protected override IQueryable<Biomarker> Queryable => _context.Biomarkers;

        [HttpPost]
        public async Task<IActionResult> CreateReference(BiomarkerReferenceModel model)
        {
            var result = await AddOrEditReferenceFromModelAsync(model);

            if (result == null)
                return RedirectToAction("Index");

            return RedirectToEditById(model.BiomarkerId);
        }

        [HttpPost]
        public async Task<IActionResult> EditReference(BiomarkerReferenceModel model)
        {
            var result = await AddOrEditReferenceFromModelAsync(model);

            if (result == null)
                return RedirectToAction("Index");

            return RedirectToEditById(model.BiomarkerId);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteReference(int id)
        {
            var r = await _context.BiomarkerReferences.FindAsync(id);
            var biomarkerId = r?.BiomarkerId;
            if (r == null) return RedirectToAction("Index");

            _context.Remove(r);
            await _context.SaveChangesAsync();

            return RedirectToEditById(biomarkerId ?? 0);
        }

        public async Task<BiomarkerReferenceConfigRange> FindOrAddRangeAsync(double a, double b)
        {
            var r = await _context.BiomarkerReferenceConfigRanges.FirstOrDefaultAsync(x =>
                Math.Abs(x.Lower - a) < 0.001 && Math.Abs(x.Upper - b) < 0.001);

            if (r != null)
                return r;

            r = new BiomarkerReferenceConfigRange
            {
                Lower = a, Upper = b
            };

            await _context.BiomarkerReferenceConfigRanges.AddAsync(r);
            await _context.SaveChangesAsync();

            return r;
        }

        public async Task<BiomarkerReference> AddOrEditReferenceFromModelAsync(BiomarkerReferenceModel model)
        {
            var biomarker = _context.Biomarkers.FirstOrDefault(x => x.Id == model.BiomarkerId);
            if (biomarker == null)
                return null;

            var isNew = model.Id == default;

            var r = isNew
                ? new BiomarkerReference()
                : await _context.BiomarkerReferences.FindAsync(model.Id);

            if (r == null)
                return null;

            r.BiomarkerId = model.BiomarkerId;
            r.UnitId = model.UnitId;
            r.ValueA = model.ValueA;
            r.ValueB = model.ValueB;

            var range = await FindOrAddRangeAsync(model.AgeLower, model.AgeUpper);

            if (isNew)
                r.Config = new BiomarkerReferenceConfig();

            r.Config.GenderId = model.GenderId;
            r.Config.AgeRangeId = range.Id;

            if (isNew)
                await _context.BiomarkerReferences.AddAsync(r);

            await _context.SaveChangesAsync();
            return r;
        }
    }

    public class BiomarkerTranslationController : TranslationController<Biomarker, BiomarkerTranslation>
    {
        public BiomarkerTranslationController(ApplicationContext context) : base(context)
        {
        }

        protected override IQueryable<Biomarker> Queryable => _context.Biomarkers;

        public override IActionResult RedirectToBaseEntity(int id) =>
            RedirectToAction("Edit", "Biomarker", new { id });
    }
}