﻿using System.Linq;
using System.Threading.Tasks;
using BioMad_backend.Areas.Api.V1.Helpers;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using BioMad_backend.Extensions;
using BioMad_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BioMad_backend.Areas.Api.V1.Controllers
{
    
    public class UnitController : GetControllerBase<Unit, UnitTranslation>
    {
        public UnitController(ApplicationContext db, UserService userService) : base(db, userService)
        {
        }

        protected override int PAGE_SIZE => 5;

        protected override IQueryable<Unit> Queryable => _db.Units;
    }
}