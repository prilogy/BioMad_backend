using System.Linq;
using BioMad_backend.Areas.Api.V1.Helpers;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using BioMad_backend.Services;

namespace BioMad_backend.Areas.Api.V1.Controllers
{
    public class CategoryController : GetControllerBase<Category>
    {
        private readonly MonitoringService _monitoringService;
        public CategoryController(ApplicationContext db, UserService userService, MonitoringService monitoringService) : base(db, userService)
        {
            _monitoringService = monitoringService;
        }
        protected override IQueryable<Category> Queryable => _db.Categories;

        protected override Category ProcessStrategy(Category m)
        {
            m = m.Localize(_userService.Culture);
            m.State = _monitoringService.CategoryStates.FirstOrDefault(x => x.CategoryId == m.Id);
            return m;
        }
    }
}