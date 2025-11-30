using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StageStory.Data;
using StageStory.Models;

namespace StageStory.Controllers
{
    public class EnterprisesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnterprisesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Enterprises.ToListAsync());
        }

        // GET: Enterprises/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enterprise = await _context.Enterprises
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enterprise == null)
            {
                return NotFound();
            }

            return View(enterprise);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,SectorActivity,Address")] Enterprise enterprise)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enterprise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors)
                                    .Select(e => e.ErrorMessage)
                                    .ToList();
                foreach (var error in allErrors)
                    {
                    Console.WriteLine(error);
                }
            }
                return View(enterprise);
        }

        // GET: Enterprises/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enterprise = await _context.Enterprises.FindAsync(id);
            if (enterprise == null)
            {
                return NotFound();
            }
            return View(enterprise);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,SectorActivity,Address")] Enterprise enterprise)
        {
            if (id != enterprise.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enterprise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnterpriseExists(enterprise.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(enterprise);
        }

        // GET: Enterprises/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enterprise = await _context.Enterprises
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enterprise == null)
            {
                return NotFound();
            }

            return View(enterprise);
        }

        // POST: Enterprises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enterprise = await _context.Enterprises.FindAsync(id);
            if (enterprise != null)
            {
                _context.Enterprises.Remove(enterprise);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnterpriseExists(int id)
        {
            return _context.Enterprises.Any(e => e.Id == id);
        }
    }
}
