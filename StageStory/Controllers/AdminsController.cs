using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using StageStory.Data;
using StageStory.Models;
using StageStory.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StageStory.Controllers
{
    public class AdminsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Admins.ToListAsync());
        }

        // GET: Admins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admin = await _context.Admins
                .FirstOrDefaultAsync(m => m.AdminId == id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        // GET: Admins/Create
        public IActionResult Create()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<ActionResult<AdminLoginResponse>> Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return BadRequest("Identifiants manquants.");
            var admin = new Admin();
            try
            {
                admin = await _context.Admins.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERREUR EF ===> " + ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }

            if (admin == null)
                return Unauthorized("Admin introuvable ou inactif.");

            bool validPassword = BCrypt.Net.BCrypt.Verify(password, admin.Password);
            if (!validPassword)
                return Unauthorized("Mot de passe incorrect.");

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userRead.Login!),
                new Claim(ClaimTypes.NameIdentifier, userRead.Id.ToString()),
                new Claim(ClaimTypes.Role, userRead.Profile!.ToLower()),
                new Claim(JwtRegisteredClaimNames.GivenName,$"{userRead.Prenom} {userRead.Nom}"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var res = _tokenService.GenerateAccessToken(claims);
            string accessToken = res.Item1;
            DateTime expire = res.Item2;

            var refreshToken = _tokenService.GenerateRefreshToken();
            _ = int.TryParse(_config["Jwt:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

            var userRefreshToken = _context.RefreshTokens.FirstOrDefault(rt => rt.UserName == user.Login);
            if (userRefreshToken == null)
            {
                userRefreshToken = new RefreshToken { UserName = user.Login };
                _context.RefreshTokens.Add(userRefreshToken);
            }

            userRefreshToken.Refresh_Token = refreshToken;
            userRefreshToken.Created = DateTime.Now;
            userRefreshToken.Expires = DateTime.Now.AddDays(refreshTokenValidityInDays);

            await _context.SaveChangesAsync();

            return Ok(new UserLoginResponse
            {
                Token = accessToken,
                RefreshToken = refreshToken,
                TokenExpireAt = expire,
                User = userRead
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdminId,Name,Email,Password")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(admin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(admin);
        }

        // GET: Admins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admin = await _context.Admins.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }
            return View(admin);
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AdminId,Name,Email,Password")] Admin admin)
        {
            if (id != admin.AdminId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(admin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminExists(admin.AdminId))
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
            return View(admin);
        }

        // GET: Admins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admin = await _context.Admins
                .FirstOrDefaultAsync(m => m.AdminId == id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin != null)
            {
                _context.Admins.Remove(admin);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminExists(int id)
        {
            return _context.Admins.Any(e => e.AdminId == id);
        }
    }
}
