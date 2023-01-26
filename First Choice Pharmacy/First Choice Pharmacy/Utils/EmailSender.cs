using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using Assignment2.Models;

namespace FIT5032_Week08A.Utils
{
    public class EmailSender
    {
        // Please use your API KEY here.
        private const String API_KEY = "YOUR_SENDGRID_ACCESS_TOKEN";
        public void Notification(String toEmail)
        {
            var client = new SendGridClient(API_KEY);
            var from = new EmailAddress("lyl13974846792@gmail.com", "First Choice Pharmacy");
            var to = new EmailAddress(toEmail, "");
            var plainTextContent = "Welcome to First Choice Pharmacy";
            var htmlContent = "<p> Welcome to First Choice Pharmacy </p>";
            var msg = MailHelper.CreateSingleEmail(from, to, "no reply", plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg);
        }

        public void ApplyBooking(String toEmail, String toName)
        {
            var client = new SendGridClient(API_KEY);
            var from = new EmailAddress("lyl13974846792@gmail.com", "First Choice Pharmacy");
            var to = new EmailAddress(toEmail, toName);
            var plainTextContent = "Your have apply for you booking";
            var htmlContent = "<p> Hi " + toName + ", The appointment application has been sent to the " +
                "Pharmacists, please check your bookings or email and wait for the confirmation. If your appointment has been confirmed, " +
                "it will be added into your calendar. </p>";
            var msg = MailHelper.CreateSingleEmail(from, to, "no reply", plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg);
        }
        public void ConfirmBooking(String toEmail, String toName)
        {
            var client = new SendGridClient(API_KEY);
            var from = new EmailAddress("lyl13974846792@gmail.com", "First Choice Pharmacy");
            var to = new EmailAddress(toEmail, toName);
            var plainTextContent = "Your Appointment has been confirmed";
            var htmlContent = "<p> Hi " + toName + ", your appointment has been confirmed! </p>";
            var msg = MailHelper.CreateSingleEmail(from, to, "no reply", plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg);
        }

        public void Send(String toEmail, String subject, String contents)
        {
            var client = new SendGridClient(API_KEY);
            var from = new EmailAddress("lyl13974846792@gmail.com", "First Choice Pharmacy");
            var to = new EmailAddress(toEmail, "");
            var plainTextContent = contents;
            var htmlContent = "<p> Thanks for your enquiry, we'll send you feedback within the next few days! </p>";
            var msg = MailHelper.CreateSingleEmail(from, to, "no reply", plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg);
        }

        public void SendWithAttachment(String toEmail, String subject, String contents, String attachment)
        {
            var client = new SendGridClient(API_KEY);
            var from = new EmailAddress("lyl13974846792@gmail.com", "First Choice Pharmacy");
            var to = new EmailAddress(toEmail, "");
            var plainTextContent = contents;
            var htmlContent = "<p> Thanks for your enquiry, we'll send you feedback within the next few days! </p>" + plainTextContent;

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            string Path = HttpContext.Current.Server.MapPath("\\Uploads\\");
            var bytes = File.ReadAllBytes(Path + attachment);
            var file = Convert.ToBase64String(bytes);
            msg.AddAttachment(attachment, file);




            var response = client.SendEmailAsync(msg);
        }

        public void SendBulkEmail(List<EmailAddress> toEmail, String subject, String contents, String attachment = null)
        {
            var client = new SendGridClient(API_KEY);
            var from = new EmailAddress("lyl13974846792@gmail.com", "First Choice Pharmacy");
            var plainTextContent = contents;
            var htmlContent = "<p> Thanks for your enquiry, we'll send you feedback within the next few days! </p>" + plainTextContent;

            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, toEmail, subject, plainTextContent, htmlContent);

            if (attachment != null)
            {
                string Path = HttpContext.Current.Server.MapPath("\\Uploads\\");
                var bytes = File.ReadAllBytes(Path + attachment);
                var file = Convert.ToBase64String(bytes);
                msg.AddAttachment(attachment, file);
            }


            var response = client.SendEmailAsync(msg);
        }

    }
}