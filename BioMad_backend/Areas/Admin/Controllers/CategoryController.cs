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

namespace BioMad_backend.Areas.Admin.Controllers
{
    public class CategoryController : LocalizedEntityController<Category, CategoryTranslation>
    {
        protected override IQueryable<Category> Queryable => _context.Categories;

        public CategoryController(ApplicationContext context) : base(context)
        {
        }

        public async Task<IActionResult> RemoveBiomarker(int entityId, int containerId)
        {
            if (await NavigationPropertyHelpers.RemoveAsync<Category, CategoryBiomarker>(_context, entityId,
                containerId,
                x => x.CategoryBiomarkers.FirstOrDefault(y => y.BiomarkerId == entityId)))
                return RedirectToEditById(containerId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddBiomarker(int entityId, int containerId)
        {
            if (await NavigationPropertyHelpers.AddAsync<Category>(_context, entityId, containerId,
                async x => !x.Biomarkers.Any(y => y.Id == entityId)
                           && await _context.Biomarkers.AnyAsync(y => y.Id == entityId),
                x => x.CategoryBiomarkers.Add(new CategoryBiomarker
                {
                    BiomarkerId = entityId
                })))
                return RedirectToAction("Edit", new { id = containerId });
            return RedirectToAction("Index");
        }
    }

    public class CategoryTranslationController : NoViewTranslationController<Category, CategoryTranslation>
    {
        public CategoryTranslationController(ApplicationContext context) : base(context)
        {
        }

        protected override IQueryable<Category> Queryable => _context.Categories.AsQueryable();

        public override IActionResult RedirectToBaseEntity(int id)
            => RedirectToAction("Edit", "Category", new { id });
    }
}