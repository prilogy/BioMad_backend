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
    public class BiomarkerTypeController : EntityController<BiomarkerType, BiomarkerTypeTranslation>
    {
        public BiomarkerTypeController(ApplicationContext context) : base(context)
        {
        }

        protected override IQueryable<BiomarkerType> Queryable => _context.BiomarkerTypes;
    }
}
