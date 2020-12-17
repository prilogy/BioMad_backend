using System;
using System.Threading.Tasks;
using BioMad_backend.Data;
using BioMad_backend.Entities.ManyToMany;
using BioMad_backend.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BioMad_backend.Areas.Admin.Helpers
{
    public static class NavigationPropertyHelpers
    {
        public static async Task<bool> RemoveAsync<T, TNav>(ApplicationContext context, int entityId, int containerId, Func<T, TNav> getNavProperty)
            where T: class, IWithId, new()
        {
            var container = await context.Set<T>().FindAsync(containerId);
            if (container == null)
                return false;

            var navPropertyToRemove = getNavProperty(container);
            if (navPropertyToRemove == null)
                return false;

            context.Remove(navPropertyToRemove);
            await context.SaveChangesAsync();
            
            return true;
        }

        public static async Task<bool> AddAsync<T>(ApplicationContext context, int entityId, int containerId, Func<T, Task<bool>> addCondition, Action<T> add)
        where T: class, IWithId, new() 
        {
            var container = await context.Set<T>().FindAsync(containerId);
            if (container == null)
                return false;

            if (await addCondition(container))
                add(container);
            
            await context.SaveChangesAsync();
            return true;
        }
    }
}