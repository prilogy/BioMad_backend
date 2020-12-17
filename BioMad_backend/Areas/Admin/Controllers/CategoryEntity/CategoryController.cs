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
    public class CategoryController : EntityController<Category, CategoryTranslation>
    {
        protected override IQueryable<Category> Queryable => _context.Categories;

        public CategoryController(ApplicationContext context) : base(context)
        {
        }

        public async Task<IActionResult> RemoveBiomarker(int biomarkerId, int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
                return RedirectToAction("Index");

            var toRemove = category.CategoryBiomarkers.FirstOrDefault(x => x.BiomarkerId == biomarkerId);
            if (toRemove == null)
                return RedirectToEditById(category.Id);

            _context.Remove(toRemove);
            await _context.SaveChangesAsync();
            
            return RedirectToEditById(category.Id);
        }
        
        public async Task<IActionResult> AddBiomarker(int biomarkerId, int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
                return RedirectToAction("Index");

            
            if(category.Biomarkers.Any(x => x.Id != biomarkerId) 
               && await _context.Biomarkers.AnyAsync(x => x.Id == biomarkerId))
                category.CategoryBiomarkers.Add(new CategoryBiomarker
                {
                    BiomarkerId = biomarkerId
                });
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Edit", new { id = category.Id });
        }
    }
}
