using System.Linq;
using System.Threading.Tasks;
using BioMad_backend.Areas.Admin.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using BioMad_backend.Extensions;
using X.PagedList;

namespace BioMad_backend.Areas.Admin.Controllers.GenderEntity
{
    public class GenderController : EntityController<Gender, GenderTranslation>
    {
        public GenderController(ApplicationContext context) : base(context)
        {
        }

        protected override IQueryable<Gender> Queryable => _context.Genders;
    }
}
