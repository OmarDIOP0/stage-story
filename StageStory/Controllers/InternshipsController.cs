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
    public class InternshipsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InternshipsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Internships
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Internships.Include(i => i.Enterprise).Include(i => i.Student);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Internships/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var internship = await _context.Internships
                .Include(i => i.Enterprise)
                .Include(i => i.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (internship == null)
            {
                return NotFound();
            }

            return View(internship);
        }

        // GET: Internships/Create
        public IActionResult Create()
        {
            ViewData["Id"] = new SelectList(_context.Enterprises, "Id", "Id");
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Rating,Evaluation,SalaryAmount,Status,SignatureType,StartDate,EndDate,CreatedDate,StudentId,EntrepriseId")] Internship internship)
        {
            if (ModelState.IsValid)
            {
                internship.CreatedDate = DateTime.Now;
                internship.Status = Models.Enum.StatusEnum.Pending;
                if(internship.StudentId == 0)
                {
                    internship.Profile = Models.Enum.ProfileEnum.Anonymous;
                    internship.StudentId = null;
                }
                else
                {
                    internship.Profile = Models.Enum.ProfileEnum.Student;

                }
                _context.Add(internship);
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
                ViewData["Id"] = new SelectList(_context.Enterprises, "Id", "Name", internship.Id);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId", internship.StudentId);
            return View(internship);
        }

        // GET: Internships/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var internship = await _context.Internships.FindAsync(id);
            if (internship == null)
            {
                return NotFound();
            }
            ViewData["Id"] = new SelectList(_context.Enterprises, "Id", "Id", internship.Id);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId", internship.StudentId);
            return View(internship);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Rating,Evaluation,SalaryAmount,Status,SignatureType,StartDate,EndDate,CreatedDate,StudentId,EntrepriseId")] Internship internship)
        {
            if (id != internship.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(internship);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InternshipExists(internship.Id))
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
            ViewData["Id"] = new SelectList(_context.Enterprises, "Id", "Id", internship.Id);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId", internship.StudentId);
            return View(internship);
        }

        // GET: Internships/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var internship = await _context.Internships
                .Include(i => i.Enterprise)
                .Include(i => i.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (internship == null)
            {
                return NotFound();
            }

            return View(internship);
        }

        // POST: Internships/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var internship = await _context.Internships.FindAsync(id);
            if (internship != null)
            {
                _context.Internships.Remove(internship);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InternshipExists(int id)
        {
            return _context.Internships.Any(e => e.Id == id);
        }
    }
}
