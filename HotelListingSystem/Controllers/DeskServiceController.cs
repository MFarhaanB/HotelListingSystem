using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HotelListingSystem.Models;
using static HotelListingSystem.Controllers.ReservationsController;

namespace HotelListingSystem.Controllers
{
    [Authorize]
    public class DeskServiceController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: DeskService
        public ActionResult Index()
        {
            var user = AppHelper.CurrentHotelUser().Id;
            var reservations = db.Reservations
                .Include(c => c.Hotel)
                .Include(c => c.Room)
                .Include(c => c.HotelUser)
                .Where(a => a.Hotel.ReceptionistId == user).ToList();
            return View(reservations);
        }
        public ActionResult Update(int Id)
        {
            var results = db.Reservations
                .Include(c => c.Hotel)
                .Include(c => c.Room)
                .Include(c => c.HotelUser)
                .Include(c => c.CheckInRoom)
                .FirstOrDefault(a => a.Id == Id);
            ViewBag.ThisHotelRooms = new SelectList(db.Rooms.Where(a => a.HotelId == results.HotelId).ToList(), "Id", "Name");
            return View(results);
        }

        public ActionResult DeskCheckIns(int Id)
        {
            var results = db.Reservations
                .Include(c => c.Hotel)
                .Include(c => c.Room)
                .Include(c => c.HotelUser)
                .Include(c => c.CheckInRoom)
                .FirstOrDefault(a => a.Id == Id);
            ViewBag.ThisHotelRooms = new SelectList(db.Rooms.Where(a => a.HotelId == results.HotelId).ToList(), "Id", "Name");
            return View(results);
        }
    }

    public class ResevationViewModel
    {
        public Reservation Reservation { get; set; }
        public List<AddOnsR>  AddOnsR { get; set; }
        public DiningReservation DiningReservation { get; set; }

    }
}