using System;
using System.Collections.Generic;
using System.Linq;
using BioMad_backend.Areas.Admin.Models;
using BioMad_backend.Entities;
using BioMad_backend.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BioMad_backend.Areas.Admin.Helpers
{
    public static class SelectListHelpers
    {
        public static SelectList CreateSelectList<T>(IEnumerable<T> list, Func<T,SelectListModel> process, int selectId = default)
        {
            var items = list.Select(process).ToList();
            return new SelectList(items, "Id", "Title", selectId);
        }
    }
}