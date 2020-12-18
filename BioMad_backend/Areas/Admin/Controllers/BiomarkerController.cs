using System.Linq;
using System.Threading.Tasks;
using BioMad_backend.Areas.Admin.Helpers;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using BioMad_backend.Entities.ManyToMany;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BioMad_backend.Areas.Admin.Controllers
{
  public class BiomarkerController : LocalizedEntityController<Biomarker, BiomarkerTranslation>
  {
    public BiomarkerController(ApplicationContext context) : base(context)
    {
    }

    protected override IQueryable<Biomarker> Queryable => _context.Biomarkers;
    
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