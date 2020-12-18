using System.Linq;
using System.Threading.Tasks;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using BioMad_backend.Entities.ManyToMany;
using BioMad_backend.Extensions;
using BioMad_backend.Infrastructure.AbstractClasses;
using BioMad_backend.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace BioMad_backend.Areas.Admin.Helpers
{
    public abstract class EntityController<T> : AdminController
        where T : class, IWithId, new()
    {
        protected readonly ApplicationContext _context;
        protected virtual int PageSize => 10;
        protected abstract IQueryable<T> Queryable { get; }

        public EntityController(ApplicationContext context)
        {
            _context = context;
        }

        protected IActionResult RedirectToEditById(int id) => RedirectToAction($"Edit", new { id });

        public async Task<IActionResult> Index(int page = 1, int searchString = default)
        {
            var q = Queryable;
            if (searchString != default)
            {
                q = q.Where(x => x.Id == searchString);
                ViewData["searchString"] = searchString;
            }
            
            q = q.OrderByDescending(x => x.Id);

            return View(await q.ToPagedListAsync(page, PageSize));
        }

        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(T entity)
        {
            if (ModelState.IsValid)
            {
                await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return RedirectToEditById(entity.Id);
            }
            return View(entity);
        }
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            

            var entity = await Queryable.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                return NotFound();

            return View(entity);
        }
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, T entity)
        {
            if (id != entity.Id)
                return NotFound();
            

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(entity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntityExists(entity.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction($"Index");
            }
            return View(entity);
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await Queryable
                .FirstOrDefaultAsync(m => m.Id == id);
            if (entity == null)
                return NotFound();
            

            return View(entity);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entity = await Queryable.FirstOrDefaultAsync(x => x.Id == id);
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            return RedirectToAction($"Index");
        }

        private bool EntityExists(int id)
        {
            return Queryable.Any(e => e.Id == id);
        }
    }
}