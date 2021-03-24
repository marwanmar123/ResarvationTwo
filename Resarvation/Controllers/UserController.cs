using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Resarvation.Data;
using Resarvation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resarvation.Controllers
{
    public class UserController : Controller
    {
        ApplicationDbContext _db;
        UserManager<IdentityUser> _userManager;
        RoleManager<IdentityRole> _roleManager;

        public UserController(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var viewModel = new List<ReservApprenantViewModel>();


            foreach (IdentityUser user in users)
            {
                var Vmodel = new ReservApprenantViewModel();
                Vmodel.Email = user.Email;
                Vmodel.UserName = user.UserName;
                Vmodel.Roles = await roleUser(user);
                viewModel.Add(Vmodel);
            }




            //var res = _db.Apprenants.ToList();
            //ViewBag.Data = res;
            //return View();
            return View(viewModel);
        }
        public async Task<List<string>> roleUser(IdentityUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Apprenant apprenant)
        {

            if (ModelState.IsValid)
            {

                _db.Add(apprenant);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(apprenant);
        }


        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _db.Apprenants.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, Apprenant apprenant)
        {

            if (id != apprenant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(apprenant);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApprenantExists(apprenant.Id))
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
            return View(apprenant);
        }
        private bool ApprenantExists(string id)
        {
            return _db.Apprenants.Any(e => e.Id == id);
        }


        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _db.Apprenants
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: AspNetUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var aspNetUser = await _db.Apprenants.FindAsync(id);
            _db.Apprenants.Remove(aspNetUser);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _db.Apprenants
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


    }
}
