using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using StageStory.Data;
using StageStory.Models;
using System.Diagnostics;

namespace StageStory.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }   
    public IActionResult Index()
    {
        ViewBag.EntrepriseList = new SelectList(_context.Enterprises, "Id", "Name");
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Description,Rating,Evaluation,SalaryAmount,Status,SignatureType,StartDate,EndDate,CreatedDate,StudentId,EntrepriseId")] Internship internship)
    {
        if (!ModelState.IsValid)
        {
            var allErrors = ModelState.Values.SelectMany(v => v.Errors)
                                             .Select(e => e.ErrorMessage)
                                             .ToList();
            return Json(new { success = false, errors = allErrors });
        }

        internship.CreatedDate = DateTime.Now;
        internship.Status = Models.Enum.StatusEnum.Pending;

        if (internship.StudentId == 0)
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

        return Json(new { success = true, message = "Stage créé avec succès !" });
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
