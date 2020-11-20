using System.Linq;
using BioMad_backend.Areas.Api.V1.Helpers;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using BioMad_backend.Services;

namespace BioMad_backend.Areas.Api.V1.Controllers
{
    public class CategoryController : GetControllerBase<Category>
    {
        public CategoryController(ApplicationContext db, UserService userService) : base(db, userService)
        {
        }
        protected override IQueryable<Category> Queryable => _db.Categories;
        protected override Category LocalizationStrategy(Category m) => m.Localize(_userService.Culture);
    }
}