using HotelListingSystem.Models;
using HotelListingSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace HotelListingSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string message)
        {
            ViewBag.Message = message;
            //new Email().SendEmail("farhaanhotd1@gmail.com", "Hotel Validation", "Iqsaan", "Verified");

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult SendMessage(string email, string name, string body, string subject)
        {
            string b = "Your enquiry, " + body + " has been submitted and will be attended to by an agent soon.";
            new Email().SendEmail(email, "Hotel enquiry: " + subject, name, b, false);
            return Json(new { success = true, message = "Hotel updated successfully" });

        }

    }
}