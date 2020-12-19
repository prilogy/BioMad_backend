using System.Linq;
using System.Threading.Tasks;
using BioMad_backend.Areas.Admin.Helpers;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using BioMad_backend.Entities.ManyToMany;
using BioMad_backend.Helpers;
using BioMad_backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BioMad_backend.Areas.Admin.Controllers
{
    public class ArticleController : LocalizedEntityController<Article, ArticleTranslation>
    {
        private readonly ImageService _imageService;
        
        public ArticleController(ApplicationContext context, ImageService imageService) : base(context)
        {
            _imageService = imageService;
        }
        

        protected override IQueryable<Article> Queryable => _context.Articles;

        [HttpPost]
        public async Task<IActionResult> AddImage(int id, [FromForm] IFormFile file)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
                return NotFound();
            
            var image = await _imageService.AddAsync(file);
            if (image == null)
                return BadRequest();

            article.ArticleImages.Add(new ArticleImage
            {
                ImageId = image.Id
            });

            await _context.SaveChangesAsync();
            return RedirectToEditById(article.Id);
        }

        [HttpGet]
        public async Task<IActionResult> RemoveImage(int articleId, int id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image == null)
                return NotFound();

            _context.Remove(image);
            await _context.SaveChangesAsync();

            return RedirectToEditById(articleId);
        }
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