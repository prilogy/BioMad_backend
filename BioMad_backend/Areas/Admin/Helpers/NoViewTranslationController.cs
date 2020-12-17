﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 using BioMad_backend.Data;
 using BioMad_backend.Entities;
 using BioMad_backend.Infrastructure.AbstractClasses;
 using BioMad_backend.Infrastructure.Interfaces;
 using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace BioMad_backend.Areas.Admin.Helpers
{
  public abstract class NoViewTranslationController<T, T2> : AdminController
    where T : class, ILocalizedEntity<T2>, IWithId, new()
    where T2 : Translation<T2>, ITranslationEntity<T>, new()
  {
    protected readonly ApplicationContext _context;

    protected abstract IQueryable<T> Queryable { get; }

    public NoViewTranslationController(ApplicationContext context)
    {
      _context = context;
    }

    public abstract IActionResult RedirectToBaseEntity(int id);
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(T2 translation)
    {
      if (ModelState.IsValid)
      {
        try
        {
          _context.Add(translation);
          await _context.SaveChangesAsync();
          return RedirectToBaseEntity(translation.BaseEntityId);
        }
        catch
        {
          return View("Ошибка: такое сочетание ключей уже существует");
        }
      }

      return View(translation);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, T2 translation)
    {
      if (id != translation.Id)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(translation);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!TranslationExists(translation.Id))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }

        return RedirectToBaseEntity(translation.BaseEntityId);
      }

      return View(translation);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
      var translation = await _context.Set<T2>().FindAsync(id);
      var baseId = translation.BaseEntityId;
      _context.Set<T2>().Remove(translation);
      await _context.SaveChangesAsync();
      return RedirectToBaseEntity(baseId);
    }

    private bool TranslationExists(int id)
    {
      return _context.Set<T2>().Any(e => e.Id == id);
    }
  }
}