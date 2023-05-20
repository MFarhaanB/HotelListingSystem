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
            foreach (var r in reservations)
                r.payment = db.Payments.FirstOrDefault(b => (int)b.ReservationId == r.Id);
            return View(reservations);
        }


        public ActionResult SearchReservations(string IdentityNumber)
        {
            var user = AppHelper.CurrentHotelUser().Id;
            var reservations = db.Reservations
                .Include(c => c.Hotel)
                .Include(c => c.Room)
                .Include(c => c.HotelUser)
                .Where(a => a.Hotel.ReceptionistId == user && a.HotelUser.IdentificationNumber == IdentityNumber).ToList();
            foreach (var r in reservations)
                r.payment = db.Payments.FirstOrDefault(b => (int)b.ReservationId == r.Id);
            return View("Index", reservations);
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
            results.Document = db.Documents.FirstOrDefault(a => ((int)a.ReservationId == results.Id) && (a.DocumentTypeKey == "a_customer_liveness_image"));
            ViewBag.ThisHotelRooms = new SelectList(db.Rooms.Where(a => a.HotelId == results.HotelId).ToList(), "Id", "Name");
            return View(results);
        }



       public ActionResult AllocateRoomsToReservation(int id)
        {
            bool IsRoomAllocatedResults = false;
            try
            {
                using(ApplicationDbContext context = new ApplicationDbContext())
                {
                    var reservation = context.Reservations.Find(id);
                    var hotel = context.Hotels.Find(reservation.HotelId);
                    var customer = context.HotelUsers.Find(reservation.HotelUserId);
                    GenerateHotelRoomNumbers(context, hotel);

                    var rooms = context.CheckInRooms.Where(c => !c.IsTaken).ToList().Take(reservation.NoOfRooms);
                    string roomNumber = "";
                    foreach(var room in rooms)
                    {
                        reservation.CheckInRoomId = room.Id; 
                        room.IsTaken = true;
                        context.Entry(room).State = EntityState.Modified;
                        roomNumber = string.IsNullOrEmpty(roomNumber) ? room.RooomNumber : $"{roomNumber},{room.RooomNumber}";
                    }
                    reservation.RoomNumber = roomNumber;
                    reservation.RoomAllocated = true;
                    context.Entry(reservation).State = EntityState.Modified;
                    context.SaveChanges();
                }
                IsRoomAllocatedResults = true;
            }
            catch
            {
                return Json(IsRoomAllocatedResults, JsonRequestBehavior.AllowGet);
            }
            return Json(IsRoomAllocatedResults, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConfirmHotelCheckIn(int id)
        {
            bool IsRoomAllocatedResults = false;
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var reservation = context.Reservations.Find(id);
                    reservation.CheckInConfirmed = true;
                    reservation.ModifiedOn = DateTime.Now;
                    context.Entry(reservation).State = EntityState.Modified;
                    context.SaveChanges();
                }
                IsRoomAllocatedResults = true;
            }
            catch
            {
                return Json(IsRoomAllocatedResults, JsonRequestBehavior.AllowGet);
            }
            return Json(IsRoomAllocatedResults, JsonRequestBehavior.AllowGet);
        }

        public static void GenerateHotelRoomNumbers(ApplicationDbContext context, Hotel hotel )
        {
            var roomnumbers = context.CheckInRooms.Where(a => a.HotelId == hotel.Id).ToList();
            if (roomnumbers.Count == 0)
            {
                for (int roomNumber = 1; roomNumber <= hotel.MaxOccupancy; roomNumber++)
                {
                    string room = GetRoomNumber(roomNumber, (int)hotel.MaxOccupancy, 25);
                    context.CheckInRooms.Add(new CheckInRoom { RooomNumber = room, HotelId = hotel.Id, IsTaken = false });
                    context.SaveChanges();
                }
            }

        }
        public static string GetRoomNumber(int roomNumber, int totalRooms, int roomsPerFloor)
        {
            if (roomNumber < 1 || roomNumber > totalRooms)
            {
                throw new ArgumentException("Invalid room number.");
            }

            int floorNumber = (roomNumber - 1) / roomsPerFloor + 1;
            char floorLetter = (char)('A' + floorNumber - 1);
            int roomInFloor = (roomNumber - 1) % roomsPerFloor + 1;

            return $"{floorLetter}{roomInFloor:D2}";
        }
    }

    public class ResevationViewModel
    {
        public Reservation Reservation { get; set; }
        public List<AddOnsR>  AddOnsR { get; set; }
        public DiningReservation DiningReservation { get; set; }

    }
}