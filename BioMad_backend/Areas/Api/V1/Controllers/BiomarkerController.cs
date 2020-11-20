using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BioMad_backend.Areas.Api.V1.Helpers;
using BioMad_backend.Areas.Api.V1.Models;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using BioMad_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BioMad_backend.Areas.Api.V1.Controllers
{
    public class BiomarkerController : GetControllerBase<Biomarker>
    {
        public BiomarkerController(ApplicationContext db, UserService userService) : base(db, userService)
        {
        }

        protected override IQueryable<Biomarker> Queryable => _db.Biomarkers;
        protected override Biomarker LocalizationStrategy(Biomarker m) => m.Localize(_userService.Culture);
    }
}