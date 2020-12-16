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

namespace BioMad_backend.Areas.Admin.Controllers.BiomarkerTypeEntity
{
    public class BiomarkerTypeTranslationController : TranslationController<BiomarkerType, BiomarkerTypeTranslation>
    {
        public BiomarkerTypeTranslationController(ApplicationContext context) : base(context)
        {
        }

        protected override IQueryable<BiomarkerType> Queryable => _context.BiomarkerTypes.AsQueryable();

        public override IActionResult RedirectToBaseEntity(int id)
            => RedirectToAction("Edit", "BiomarkerType", new {id});
    }
}
