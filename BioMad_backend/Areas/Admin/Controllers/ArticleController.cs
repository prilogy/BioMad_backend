using System.Linq;
using System.Threading.Tasks;
using BioMad_backend.Areas.Admin.Helpers;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BioMad_backend.Areas.Admin.Controllers
{
    public class ArticleController : LocalizedEntityController<Article, ArticleTranslation>
    {
        public ArticleController(ApplicationContext context) : base(context)
        {
        }
        

        protected override IQueryable<Article> Queryable => _context.Articles;
    }

    public class ArticleTranslationController : NoViewTranslationController<Article, ArticleTranslation>
    {
        public ArticleTranslationController(ApplicationContext context) : base(context)
        {
        }

        protected override IQueryable<Article> Queryable => _context.Articles;
        public override IActionResult RedirectToBaseEntity(int id) => RedirectToAction("Edit", "Article", new {Area="Admin", id});
        
        
        public async Task<ActionResult<string>> GetText(int id)
        {
            var tr = await _context.Set<ArticleTranslation>().FirstOrDefaultAsync(x => x.Id == id);
            if (tr == null)
                return NotFound();

            return Ok(tr.Text);
        }
    }
}