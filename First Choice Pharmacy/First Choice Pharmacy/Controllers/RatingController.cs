using Assignment2.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assignment2.Controllers
{
    public class RatingController : Controller
    {
        private RatingModel db = new RatingModel();
        // GET: Rating
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult setRating(int productId, decimal rate, string comment)
        {
            Rating rating = new Rating();
            rating.ProductId = productId;
            rating.Rate = rate;
            rating.Comment = comment;
            rating.UsersId = User.Identity.GetUserId();

            db.Ratings.Add(rating);
            db.SaveChanges();

            return RedirectToAction("Details", "Products", new { id = productId });
        }
    }
}