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
  public abstract class TranslationController<T, T2> : AdminController
    where T : class, ILocalizedEntity<T2>, IWithId, new()
    where T2 : Translation<T2>, ITranslationEntity<T>, new()
  {
    protected readonly ApplicationContext _context;

    protected abstract IQueryable<T> Queryable { get; }

    public TranslationController(ApplicationContext context)
    {
      _context = context;
    }

    public abstract IActionResult RedirectToBaseEntity(int id);
    // RedirectToAction("Edit", "Sport", new { Id = id });
    
    public async Task<IActionResult> Index()
    {
      var applicationContext = _context.Set<T2>();
      return View(await applicationContext.ToListAsync());
    }
    
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var translation = await _context.Set<T2>()
        .FirstOrDefaultAsync(m => m.Id == id);
      if (translation == null)
      {
        return NotFound();
      }

      return View(translation);
    }
    
    public IActionResult Create(int baseEntityId = default, int cultureId = default)
    {
      var list = new SelectList(Queryable, "Id", "Id");
      if (baseEntityId != default)
      {
        var item = list.FirstOrDefault(x => x.Value == baseEntityId.ToString());
        if (item != null)
        {
          item.Selected = true;
          ViewData["BaseEntityIsReadOnly"] = true;
        }
        else ModelState.AddModelError("", "Id базового представления не существует.");
      }

      var cultureList = new SelectList(_context.Cultures, "Id", "Key");
      if (cultureId != default)
      {
        var item = cultureList.FirstOrDefault(x => x.Value == cultureId.ToString());
        if (item != null)
        {
          item.Selected = true;
          ViewData["CultureIdIsReadOnly"] = true;
        }
        else ModelState.AddModelError("", "Id культуры представления не существует.");
      }

      ViewData["BaseEntityId"] = list;
      ViewData["CultureId"] = cultureList;
      return View();
    }
    
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
          ModelState.AddModelError("", "Такое сочетание ключей уже существует.");
          return Create();
        }
      }

      return View(translation);
    }
    
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var translation = await _context.Set<T2>().FindAsync(id);
      if (translation == null)
      {
        return NotFound();
      }

      ViewData["BaseEntityId"] = new SelectList(Queryable, "Id", "Id", translation.BaseEntityId);
      ViewData["CultureId"] = new SelectList(_context.Cultures, "Id", "Id", translation.CultureId);
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
    
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var translation = await _context.Set<T2>()
        .FirstOrDefaultAsync(m => m.Id == id);
      if (translation == null)
      {
        return NotFound();
      }

      return View(translation);
    }
    
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
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