using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Assignment2.Models;
using Microsoft.AspNet.Identity;

namespace Assignment2.Controllers
{
    public class RatingsController : Controller
    {
        private RatingViewModel db = new RatingViewModel();

        // GET: Ratings?productid = 5
        public ActionResult Index()
        {
            string url = System.Web.HttpContext.Current.Request.Url.ToString();
            Uri uri = new Uri(url);
            var productId = HttpUtility.ParseQueryString(uri.Query).Get("productid");
            if(productId == null)
            {
                return RedirectToAction("Index", "Products");
            }
            var ratings = db.Ratings.AsEnumerable().Where(s => s.ProductId == int.Parse(productId)).ToList();
            return View(ratings);
        }

        // GET: Ratings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // GET: Ratings/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Ratings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,Rate,Comment")] Rating rating)
        {
            string url = System.Web.HttpContext.Current.Request.Url.ToString();
            Uri uri = new Uri(url);
            var productId = HttpUtility.ParseQueryString(uri.Query).Get("productid");
            rating.ProductId = int.Parse(productId);
            rating.UserId = User.Identity.GetUserId();
            rating.Time = DateTime.Now;
            if(productId == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                db.Ratings.Add(rating);
                db.SaveChanges();
                ViewBag.Result = "Comment Successful!";
                return Redirect("/Ratings?productid=" + productId);
            }

            return View(rating);
        }

        // GET: Ratings/Edit/5
        [Authorize(Roles ="Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // POST: Ratings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Rate,Comment,Time,ProductId,UserId")] Rating rating)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rating).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rating);
        }

        // GET: Ratings/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // POST: Ratings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Rating rating = db.Ratings.Find(id);
            db.Ratings.Remove(rating);
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
