using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HotelListingSystem.Models;
using HotelListingSystem.ViewModel;

namespace HotelListingSystem.Controllers
{
    public class ReservationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reservations
        public ActionResult IndexAll()
        {
            var reservations = db.Reservations.Include(r => r.Hotel).Include(r => r.HotelUser).Include(r => r.Room);
            return View(reservations.ToList());
        }       
        
        // GET: Reservations
        public ActionResult Index()
        {
            var user = AppHelper.CurrentHotelUser().Id;
            var reservations = db.Reservations.Include(r => r.Hotel).Include(r => r.HotelUser).Include(r => r.Room).Where(x=>x.HotelUserId == user).ToList();
            return View(reservations);
        }
        public ActionResult MyReservations()
        {
            var user = AppHelper.CurrentHotelUser();
            var reservations = db.Reservations.Include(r => r.Hotel).Include(r => r.HotelUser).Include(r => r.Room).Where(x=>x.HotelUserId == user.Id).ToList();
            return View(reservations);
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
        public ActionResult Create(int id)
        {
            Reservation reservation = new Reservation
            {
                HotelId = id,
                HotelName = db.Hotels.AsNoTracking().FirstOrDefault(x=>x.Id == id)?.Name,
                CheckInDate = DateTime.Now,
                CheckOutDate = DateTime.Now.AddDays(5),
            };
            ViewBag.HotelId = new SelectList(db.Hotels, "Id", "Name");
            ViewBag.HotelUserId = new SelectList(db.HotelUsers, "Id", "FirstName");
            ViewBag.RoomId = new SelectList(db.Rooms, "Id", "Name");
            return View(reservation);
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Reservation reservation)
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
        //[HttpPost]
        //public ActionResult BookReservationConfimation(int hotelId, DateTime checkInDate, DateTime checkOutDate, int noOfRooms, int roomId)
        //{
        //    ReservationConfirmationVM reservationConfirmationVM = new ReservationConfirmationVM
        //    {
        //        HotelId = hotelId,
        //        CheckInDate = checkInDate,
        //        CheckOutDate = checkOutDate,
        //        NoOfRooms = noOfRooms,
        //        RoomId = roomId
        //    };
        //    return View(reservationConfirmationVM);
        //}

        [HttpPost]
        public ActionResult BookReservation(ReservationConfirmationVM reservationVM /*int hotelId, DateTime checkInDate, DateTime checkOutDate, int noOfRooms, int roomId*/)
        {

            int reservationCount = db.Reservations.Where(x => x.HotelId == reservationVM.HotelId).Count();
            var hotelInfo = db.Hotels.FirstOrDefault(x=>x.Id == reservationVM.HotelId);
            var roomInfo = db.Rooms.FirstOrDefault(x=>x.Id == reservationVM.RoomId);
            var cost = roomInfo.PricePerRoom * reservationVM.NoOfRooms;
            string message = "";

            // Check if room quantity is available
            if (hotelInfo.MaxOccupancy >= reservationCount)
            {
                var user = AppHelper.CurrentHotelUser();
                // Return success status with empty message
                Reservation reservation = new Reservation();
                reservation.HotelId = reservationVM.HotelId;
                reservation.HotelUserId = user?.Id;
                reservation.RoomId = reservationVM.RoomId;
                reservation.NoOfRooms = (int)reservationVM.NoOfRooms;
                reservation.CreatedOn = DateTime.Now;
                reservation.CheckOutDate = reservationVM.CheckOutDate;
                reservation.CheckInDate = reservationVM.CheckInDate;
                reservation.TotalCost = cost;
                reservation.TotalFees = Math.Round((cost * 0.02m), 2);
                db.Reservations.Add(reservation);
                db.SaveChanges();

                //user = new HotelUsers { EmailAddress = "farhaanhotd1@gmail.com" };
                new Email().SendEmail(user.EmailAddress, "Hotel Reservation Success", user.FirstName + " "+user.LastName, "Your booking for " + hotelInfo.Name +", " + reservationVM.NoOfRooms + " at the cost of R"+ cost , false);

                return Json(new { success = true, message = "" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // Return failure status with error message
                message = "The requested number of rooms is not available for the selected dates.";
                return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetTotalCost(int roomId, int noOfRooms)
        {
            var roomInfo = db.Rooms.Include(x=>x.Hotel).FirstOrDefault(x => x.Id == roomId);
            var cost = roomInfo.PricePerRoom * noOfRooms;
            
            return Json(new { success = true, message = cost }, JsonRequestBehavior.AllowGet);
        }
    }
}
