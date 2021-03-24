using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Resarvation.Data;
using Resarvation.Models;

namespace Resarvation.Controllers
{
    [Authorize(Roles = "admin")]
    public class TypeReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TypeReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TypeReservations
        public async Task<IActionResult> Index()
        {
            return View(await _context.TypeReservations.ToListAsync());
        }


        // GET: TypeReservations/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,AccessNumber")] TypeReservation typeReservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(typeReservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(typeReservation);
        }

        // GET: TypeReservations/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeReservation = await _context.TypeReservations.FindAsync(id);
            if (typeReservation == null)
            {
                return NotFound();
            }
            return View(typeReservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,AccessNumber")] TypeReservation typeReservation)
        {
            if (id != typeReservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(typeReservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeReservationExists(typeReservation.Id))
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
            return View(typeReservation);
        }




        private bool TypeReservationExists(string id)
        {
            return _context.TypeReservations.Any(e => e.Id == id);
        }
    }
}
