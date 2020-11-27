using System.Linq; 
using BioMad_backend.Areas.Api.V1.Helpers;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using BioMad_backend.Services;

namespace BioMad_backend.Areas.Api.V1.Controllers
{
    public class ArticleController : GetControllerBase<Article>
    {
        public ArticleController(ApplicationContext db, UserService userService) : base(db, userService)
        {
        }

        protected override IQueryable<Article> Queryable => _db.Articles;
        protected override Article ProcessStrategy(Article m) => m.Localize(_userService.Culture);
    }
}