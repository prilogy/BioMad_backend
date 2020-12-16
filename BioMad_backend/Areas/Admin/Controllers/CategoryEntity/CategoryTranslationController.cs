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

namespace BioMad_backend.Areas.Admin.Controllers.CategoryEntity
{
    public class CategoryTranslationController : TranslationController<Category, CategoryTranslation>
    {
        public CategoryTranslationController(ApplicationContext context) : base(context)
        {
        }

        protected override IQueryable<Category> Queryable => _context.Categories.AsQueryable();

        public override IActionResult RedirectToBaseEntity(int id)
            => RedirectToAction("Edit", "Category", new {id});
    }
}