using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Assignment2.Models;
using FIT5032_Week08A.Utils;
using Microsoft.AspNet.Identity;

namespace Assignment2.Controllers
{
    public class BookingsController : Controller
    {
        private BookingModel db = new BookingModel();

        // GET: Bookings
        [Authorize]
        public ActionResult Index()
        {
            if (User.IsInRole("Pharmacist")) {
                var userId = User.Identity.GetUserId();
                var events = db.Bookings.Where(s => s.PharmacistId ==
                userId).ToList();
                return View(events);
            }
            else
            {
                var userId = User.Identity.GetUserId();
                var events = db.Bookings.Where(s => s.PatientId ==
                userId).ToList();
                for(int i = 0; i < events.Count; i++)
                {
                    if (events[i].PharmacistId == null)
                    {
                        events.Remove(events[i]);
                        i--;
                    }
                }
                return View(events);
            }
        }

        //GET: Bookings/ViewAllBookings
        [Authorize(Roles = "Admin")]
        public ActionResult ViewAllBookings()
        {
            return View(db.Bookings.ToList());
        }

        // GET: Bookings/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            if(booking.PharmacistId != User.Identity.GetUserId() && booking.PatientId != User.Identity.GetUserId())
            {
                if (User.IsInRole("Admin"))
                {
                    return View(booking);
                }
                else {
                    return HttpNotFound();
                }
            }
            return View(booking);
        }

        [Authorize]
        // GET: Bookings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,Start,Title")] Booking booking)
        {
            if (User.IsInRole("Pharmacist"))
            {
                ViewBag.Result = "Sorry, Pharmacists cannot allocate appointments.";
                return View(booking);
            }
            booking.PatientId = User.Identity.GetUserId();
            booking.End = booking.Start.AddMinutes(15);
            ModelState.Clear();
            TryValidateModel(booking);
            var userId = User.Identity.GetUserId();
            var events = db.Bookings.Where(s => s.PatientId ==
            userId).ToList();
            for (int i = 0; i < events.Count; i++)
            {
                if (booking.Start >= events[i].Start.AddMinutes(-15) && booking.Start < events[i].End)
                {
                    ViewBag.Error = "Sorry, this time has been allocated.";
                    return View(booking);
                }
            }
            if(booking.Start < DateTime.Now)
            {
                ViewBag.Error = "Sorry, you can only allocate time after now.";
                return View(booking);
            }
            if (ModelState.IsValid)
            {
                db.Bookings.Add(booking);
                db.SaveChanges();
                ViewBag.Success = "The appointment application has been sent to the Pharmacists, please check your bookings or email" +
                    "and wait for the confirmation. If your appointment has been confirmed, it will be added into your calendar.";
                EmailSender es = new EmailSender();
                AspNetUsers users = db.AspNetUsers.Find(booking.PatientId);
                string email = users.Email;
                string name = users.FirstName + " " + users.LastName;
                es.ApplyBooking(email, name);
                return View(booking);
            }

            return View(booking);
        }

        //GET: Bookings/Applications
        [Authorize(Roles ="Pharmacist")]
        public ActionResult Applications()
        {

            var userId = User.Identity.GetUserId();
            var events = db.Bookings.Where(s => s.PharmacistId ==
            null).ToList();
            return View(events);
        }

        //GET: Bookings/Modify
        [Authorize]
        public ActionResult Modify()
        {
            if (User.IsInRole("Pharmacist"))
            {
                var userId = User.Identity.GetUserId();
                var events = db.Bookings.Where(s => s.PharmacistId ==
                userId).ToList();
                return View(events);
            }
            else
            {
                var userId = User.Identity.GetUserId();
                var events = db.Bookings.Where(s => s.PatientId ==
                userId).ToList();
                return View(events);
            }
        }

        // GET: Bookings/Edit/5
        [Authorize(Roles ="Pharmacist")]
        public ActionResult Confirm(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Pharmacist")]
        public ActionResult Confirm([Bind(Include = "Id,Start,PatientId,End,Title")] Booking booking)
        {
            if (User.IsInRole("Pharmacist"))
            {
                booking.PharmacistId = User.Identity.GetUserId();
            }
            var userId = User.Identity.GetUserId();
            var events = db.Bookings.Where(s => s.PharmacistId ==
            userId).ToList();
            for (int i = 0; i < events.Count; i++)
            {
                if (booking.Start >= events[i].Start.AddMinutes(-15) && booking.Start < events[i].End)
                {
                    ViewBag.Result = "Sorry, this time has been allocated.";
                    return View(booking);
                }
            }
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                EmailSender es = new EmailSender();
                AspNetUsers users = db.AspNetUsers.Find(booking.PatientId);
                string email = users.Email;
                string name = users.FirstName + " " + users.LastName;
                es.ConfirmBooking(email, name);
                return RedirectToAction("Index");
            }
            return View(booking);
        }

        // GET: Bookings/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            if (booking.PharmacistId != User.Identity.GetUserId() && booking.PatientId != User.Identity.GetUserId())
            {
                if (User.IsInRole("Admin"))
                {
                    return View(booking);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Bookings.Find(id);
            db.Bookings.Remove(booking);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
