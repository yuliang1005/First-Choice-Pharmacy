using Assignment2.Models;
using FIT5032_Week08A.Utils;
using Microsoft.AspNet.Identity;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Assignment2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            return View(new SendEmailViewModel());
        }

        //POST: Home/Contact
        //contact with website and send ads
        [HttpPost]
        public ActionResult Contact(SendEmailViewModel model, HttpPostedFileBase postedFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    String toEmail = model.ToEmail;
                    String subject = model.Subject;
                    String contents = model.Contents;

                    EmailSender es = new EmailSender();

                    var myUniqueFileName = string.Format(@"{0}", Guid.NewGuid());
                    model.Upload = myUniqueFileName;
                    
                        if (toEmail.EndsWith("."))
                        {
                            ViewBag.EmailError = "Please input valid email addresses!";
                            return View(model);
                        }
                        try
                        {
                            var addr = new System.Net.Mail.MailAddress(toEmail);
                        }
                        catch
                        {
                            ViewBag.EmailError = "Please input valid email addresses!";
                            return View(model);
                        }
                        if (postedFile != null)
                        {
                            string serverPath = Server.MapPath("~/Uploads/");
                            string fileExtension = Path.GetExtension(postedFile.FileName);
                            string filePath = model.Upload + fileExtension;
                            model.Upload = filePath;
                            postedFile.SaveAs(serverPath + model.Upload);
                            es.SendWithAttachment(toEmail, subject, contents, model.Upload);
                        }
                        else
                        {
                            es.Send(toEmail, subject, contents);
                        }
                    
                    

                    ViewBag.Result = "Thanks for your enquiry, we'll send you an email for the reply time!";

                    ModelState.Clear();

                    return View(new SendEmailViewModel());
                }
                catch
                {
                    return View();
                }
            }

            return View();
        }

        [Authorize(Roles = "Admin, Pharmacist")]
        public ActionResult SendAdTo()
        {
            BookingModel db = new BookingModel();
            var userId = User.Identity.GetUserId();
            var userList = db.AspNetUsers.Where(s => s.Id !=
            userId).ToList();
            return View(userList);
        }

        [Authorize(Roles = "Admin, Pharmacist")]
        public ActionResult SendAds()
        {
            return View(new SendEmailViewModel());
        }

        [HttpPost]
        [Authorize(Roles ="Admin, Pharmacist")]
        public ActionResult SendAds(SendEmailViewModel model, HttpPostedFileBase postedFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    String toEmail = model.ToEmail.Remove(model.ToEmail.Length -1);
                    String subject = model.Subject;
                    String contents = model.Contents;

                    EmailSender es = new EmailSender();

                    var myUniqueFileName = string.Format(@"{0}", Guid.NewGuid());
                    model.Upload = myUniqueFileName;
                        List<String> list = toEmail.Trim().Split(',').ToList();
                        List<EmailAddress> email_list = new List<EmailAddress>();
                        for (int i = 0; i < list.Count; i++)
                        {
                            EmailAddress email = new EmailAddress(list[i], "");
                            email_list.Add(email);
                        }
                        if (postedFile != null)
                        {
                            string serverPath = Server.MapPath("~/Uploads/");
                            string fileExtension = Path.GetExtension(postedFile.FileName);
                            string filePath = model.Upload + fileExtension;
                            model.Upload = filePath;
                            postedFile.SaveAs(serverPath + model.Upload);
                            es.SendBulkEmail(email_list, subject, contents, model.Upload);
                        }
                        else
                        {
                            es.SendBulkEmail(email_list, subject, contents);
                        }

                    ModelState.Clear();

                    return RedirectToAction("SendAds", "Home");
                }
                catch
                {
                    return View();
                }
            }

            return View();
        }

    }
}