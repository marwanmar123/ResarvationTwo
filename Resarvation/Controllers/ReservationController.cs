using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Resarvation.Data;
using Resarvation.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using X.PagedList;

namespace Resarvation.Controllers
{
    public class ReservationController : Controller
    {
        ApplicationDbContext _db;
        UserManager<IdentityUser> _userManager;

        public ReservationController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [Authorize(Roles = "admin")]
        // GET: ReservationController
        public ActionResult Index(int? page)
        {


            var pageNumber = page ?? 1;
            int pageSize = 5;
            var Result = (from r in _db.Reservations
                          join a in _db.Apprenants
                          on r.Apprenant.Id equals a.Id
                          join tr in _db.TypeReservations
                          on r.TypeReservation.Id equals tr.Id
                          where r.Date == DateTime.Today
                          select new ReservApprenantViewModel
                          {
                              Id = r.Id,
                              UserName = a.UserName,
                              Email = a.Email,
                              Date = r.Date,
                              Cause = r.Cause,
                              Status = r.Status,
                              TypeReservationId = tr.Id,
                              Name = tr.Name,
                              ResCount = a.ResCount
                          }).OrderBy(a => a.ResCount).ToList().ToPagedList(pageNumber, pageSize); ;


            return View("Index", Result);
        }



        public ActionResult GetAll(int? page)
        {


            var pageNumber = page ?? 1;
            int pageSize = 30;
            var Result = (from r in _db.Reservations
                          join a in _db.Apprenants
                          on r.Apprenant.Id equals a.Id
                          join tr in _db.TypeReservations
                          on r.TypeReservation.Id equals tr.Id
                          select new ReservApprenantViewModel
                          {
                              Id = r.Id,
                              UserName = a.UserName,
                              Email = a.Email,
                              Date = r.Date,
                              Cause = r.Cause,
                              Status = r.Status,
                              TypeReservationId = tr.Id,
                              Name = tr.Name,
                              ResCount = a.ResCount
                          }).OrderBy(a => a.ResCount).ToList().ToPagedList(pageNumber, pageSize); ;


            return View("Index", Result);
        }





        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult postdata(bool status, string id)
        //{

        //    var resId = _db.Reservations.FirstOrDefault(a => a.Id == id);
        //    resId.Status = status;
        //    _db.SaveChanges();
        //    return RedirectToAction(nameof(Index));

        //}


        public ActionResult Filter(DateTime filtr, ReservApprenantViewModel viewModel)
        {
            if (filtr.Year == 0001)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var Result = (from r in _db.Reservations
                              join a in _db.Apprenants
                              on r.Apprenant.Id equals a.Id
                              join tr in _db.TypeReservations
                              on r.TypeReservation.Id equals tr.Id
                              where r.Date == filtr
                              select new ReservApprenantViewModel
                              {
                                  Id = r.Id,
                                  UserName = a.UserName,
                                  Email = a.Email,
                                  Date = r.Date,
                                  Cause = r.Cause,
                                  Status = r.Status,
                                  TypeReservationId = tr.Id,
                                  Name = tr.Name,
                                  ResCount = a.ResCount
                              }).ToList();
                return View("Index", Result);
            }
        }




        public ActionResult History()
        {

            //var user = User.Identity.Name;
            var us = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var Result = (from r in _db.Reservations
                          join a in _db.Apprenants
                          on r.Apprenant.Id equals a.Id
                          join tr in _db.TypeReservations
                          on r.TypeReservation.Id equals tr.Id
                          where a.Id == us
                          select new ReservApprenantViewModel
                          {
                              Id = r.Id,
                              UserName = a.UserName,
                              Email = a.Email,
                              Date = r.Date,
                              Cause = r.Cause,
                              Status = r.Status,
                              TypeReservationId = tr.Id,
                              Name = tr.Name,
                              ResCount = a.ResCount
                          }).ToList();
            var usr = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cnt = _db.Reservations.Where(x => x.Status.Value == true).Where(x => x.ApprenantId == usr);
            int count = cnt.Count();



            ViewBag.hsb = count;

            return View(Result);
        }


        public IActionResult Create()
        {
            ViewData["type"] = new SelectList(_db.TypeReservations, "Id", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservApprenantViewModel viewModel, Apprenant apprenant)
        {

            Reservation resarvation = new Reservation()
            {
                Date = viewModel.Date,
                //Status = viewModel.Status,
                Cause = viewModel.Cause

            };



            var usId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var type = _db.TypeReservations.Single(t => t.Id == viewModel.TypeReservationId);

            resarvation.ApprenantId = usId;
            resarvation.TypeReservation = type;

            _db.Reservations.Add(resarvation);



            await _db.SaveChangesAsync();

            ViewData["type"] = new SelectList(_db.TypeReservations, "Id", "Name", viewModel.TypeReservationId);


            if (User.IsInRole("admin"))
            {

                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(History));

        }


        public IActionResult Edit(string id)
        {


            var find = _db.Reservations.Find(id);


            return View(find);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, Reservation reservation)
        {

            if (ModelState.IsValid)
            {

                try
                {

                    var res = _db.Reservations.Find(reservation.Id);

                    res.Status = reservation.Status;
                    res.Cause = reservation.Cause;
                    res.Date = reservation.Date;


                    _db.Reservations.Update(res);

                    var ListResAppr = _db.Reservations

                        .Include(r => r.Apprenant)

                        .Where(r => r.ApprenantId == reservation.ApprenantId)
                        .ToList();

                    var count = ListResAppr.Count();
                    var appr = _db.Find<Apprenant>(reservation.ApprenantId);

                    if (reservation.Status == true)
                    {
                        appr.ResCount = count;
                        _db.Apprenants.Update(appr);
                    }


                    //apprenant.ResCount = count;


                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }

            return View(reservation);
        }



    }
}
