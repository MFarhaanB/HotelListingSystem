using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HotelListingSystem.Models;

namespace HotelListingSystem.Controllers
{
    public class ReservationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reservations
        public ActionResult Index()
        {
            var reservations = db.Reservations.Include(r => r.Hotel).Include(r => r.HotelUser).Include(r => r.Room);
            return View(reservations.ToList());
        }

        // GET: Reservations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // GET: Reservations/Create
        public ActionResult CreateReservation()
        {
            ViewBag.HotelId = new SelectList(db.Hotels, "Id", "Name");
            ViewBag.HotelUserId = new SelectList(db.HotelUsers, "Id", "FirstName");
            ViewBag.RoomId = new SelectList(db.Rooms, "Id", "Name");
            var reservations = db.Rooms.Include(x => x.Hotel).ToList();
            return View(reservations);
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReservation([Bind(Include = "Id,CheckInDate,CheckOutDate,HotelId,RoomId,HotelUserId")] Reservation reservation)
        {
            reservation.HotelUserId = AppHelper.CurrentHotelUser()?.Id;
            if (ModelState.IsValid)
            {
                db.Reservations.Add(reservation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HotelId = new SelectList(db.Hotels, "Id", "Name", reservation.HotelId);
            ViewBag.HotelUserId = new SelectList(db.HotelUsers, "Id", "FirstName", reservation.HotelUserId);
            ViewBag.RoomId = new SelectList(db.Rooms, "Id", "Name", reservation.RoomId);
            var reservations = db.Rooms.Include(x => x.Hotel).ToList();
            return View(reservations);
        }



        // GET: Reservations/Create
        public ActionResult Create()
        {
            ViewBag.HotelId = new SelectList(db.Hotels, "Id", "Name");
            ViewBag.HotelUserId = new SelectList(db.HotelUsers, "Id", "FirstName");
            ViewBag.RoomId = new SelectList(db.Rooms, "Id", "Name");
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CheckInDate,CheckOutDate,HotelId,RoomId,HotelUserId")] Reservation reservation)
        {
            reservation.HotelUserId = AppHelper.CurrentHotelUser()?.Id;
            reservation.CreatedOn = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Reservations.Add(reservation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HotelId = new SelectList(db.Hotels, "Id", "Name", reservation.HotelId);
            ViewBag.HotelUserId = new SelectList(db.HotelUsers, "Id", "FirstName", reservation.HotelUserId);
            ViewBag.RoomId = new SelectList(db.Rooms, "Id", "Name", reservation.RoomId);
            var reservations = db.Rooms.Include(x => x.Hotel).ToList();
            return View(reservations);
        }

        // GET: Reservations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            ViewBag.HotelId = new SelectList(db.Hotels, "Id", "Name", reservation.HotelId);
            ViewBag.HotelUserId = new SelectList(db.HotelUsers, "Id", "FirstName", reservation.HotelUserId);
            ViewBag.RoomId = new SelectList(db.Rooms, "Id", "Name", reservation.RoomId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CheckInDate,CheckOutDate,HotelId,RoomId,HotelUserId")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HotelId = new SelectList(db.Hotels, "Id", "Name", reservation.HotelId);
            ViewBag.HotelUserId = new SelectList(db.HotelUsers, "Id", "FirstName", reservation.HotelUserId);
            ViewBag.RoomId = new SelectList(db.Rooms, "Id", "Name", reservation.RoomId);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservation reservation = db.Reservations.Find(id);
            db.Reservations.Remove(reservation);
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

        [HttpPost]
        public ActionResult BookReservation(int hotelId, DateTime checkInDate, DateTime checkOutDate, int noOfRooms, int roomId)
        {

            int reservationCount = db.Reservations.Where(x => x.HotelId == hotelId).Count();
            var roomQty = db.Hotels.FirstOrDefault(x=>x.Id == hotelId)?.MaxOccupancy;

            string message = "";

            // Check if room quantity is available
            if (roomQty >= reservationCount)
            {
                // Return success status with empty message
                Reservation reservation = new Reservation();
                reservation.HotelId = hotelId;
                reservation.HotelUserId = AppHelper.CurrentHotelUser()?.Id;
                reservation.RoomId = roomId;
                reservation.NoOfRooms = (int)roomQty;
                reservation.CreatedOn = DateTime.Now;
                reservation.CheckOutDate = checkOutDate;
                reservation.CheckInDate = checkInDate;
                db.Reservations.Add(reservation);
                db.SaveChanges();

                return Json(new { success = true, message = "" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // Return failure status with error message
                message = "The requested number of rooms is not available for the selected dates.";
                return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
