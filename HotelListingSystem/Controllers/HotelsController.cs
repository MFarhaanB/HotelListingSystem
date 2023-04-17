﻿using System;
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
    public class HotelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Hotels
        public ActionResult Index()
        {
            var hotels = db.Hotels.Include(h => h.HotelUser);
            return View(hotels.ToList());
        }
        // GET: Hotels
        public ActionResult MyHotels()
        {
            var hotels = db.Hotels.Include(h => h.HotelUser).Where(x => x.HotelUser.UserName == User.Identity.Name).ToList();
            return View(hotels);
        }

        public ActionResult HotelPayment()
        {
            var hotels = db.Hotels.Include(h => h.HotelUser).ToList();
            return View(hotels);
        }

        [HttpPost]
        public JsonResult UpdateHotel(int id, string paymentPaid, DateTime dueDate, DateTime paymentDoneDate)
        {
            // Perform the necessary logic to update the hotel row
            var hotel = db.Hotels.FirstOrDefault(x => x.Id == id);
            hotel.PaymentPaid = paymentPaid?.ToLower() == "true";
            hotel.DueDate = dueDate;
            hotel.PaymentDoneDate = paymentDoneDate;
            db.Entry(hotel).State = EntityState.Modified;
            int savechanges = db.SaveChanges();

            // Return a JSON response to the AJAX request
            return Json(new { success = savechanges > 0, message = "Hotel updated successfully" });
        }

        [HttpPost]
        public JsonResult BlacklistHotel(int id)
        {
            // Perform the necessary logic to update the hotel row
            var hotel = db.Hotels.FirstOrDefault(x => x.Id == id);
            hotel.Blacklisted = true;
            db.Entry(hotel).State = EntityState.Modified;
            int savechanges = db.SaveChanges();

            // Return a JSON response to the AJAX request
            return Json(new { success = savechanges > 0, message = "Hotel updated successfully" });
        }


        // GET: Hotels
        public ActionResult FindHotel()
        {
            ViewBag.City = new SelectList(db.Hotels.Select(h => h.City).Distinct().ToList());
            ViewBag.Suburb = new SelectList(db.Hotels.Select(h => h.Suburb).Distinct().ToList());

            var hotels = db.Hotels.Include(h => h.HotelUser);
            return View(hotels.ToList());
        }
        // GET: /Hotels/Search
        public ActionResult Search(string suburb, string city, DateTime? checkin, DateTime? checkout)
        {
            // Store the search criteria in ViewBag to pass to the view for display
            ViewBag.Suburb = suburb;
            ViewBag.City = city;
            ViewBag.CheckInDate = checkin;
            ViewBag.CheckOutDate = checkout;

            // Query the database to get hotels based on the search criteria
            var result = (from hotel in db.Hotels
                         join reservation in db.Reservations on hotel.Id equals reservation.HotelId into hotelReservationGroup
                         from reservation in hotelReservationGroup.DefaultIfEmpty()
                         where hotelReservationGroup != null && hotel.Blacklisted != true
                         select new
                         {
                             HotelId = hotel.Id,
                             HotelName = hotel.Name,
                             Suburb = hotel.Suburb,
                             City = hotel.City,
                             HotelUserId = hotel.HotelUserId == null ? null : hotel.HotelUserId,
                             Rating = hotel.Rating,
                             CheckInDate = reservation.CheckInDate,
                             CheckOutDate = reservation.CheckOutDate,
                             HotelImageName = hotel.HotelImageName,
                             HotelImageContent = hotel.HotelImageContent,
                         }).ToList();


            var hotels = result.Where(h =>
                (string.IsNullOrEmpty(suburb) || h.Suburb.Contains(suburb)) &&
                (string.IsNullOrEmpty(city) || h.City.Contains(city)) &&
                (!checkin.HasValue || h.CheckInDate <= checkin.Value) &&
                (!checkout.HasValue || h.CheckOutDate >= checkout.Value)
            ).ToList();

            return View(hotels); // Return the list of hotels to the view for display
        }
        public ActionResult DisplayImage(int hotelId, int imageType = 1)
        {
            var hotel = db.Hotels.FirstOrDefault(h => h.Id == hotelId);
            if (imageType == 1 && hotel != null && hotel.HotelImageContent != null)
            {
                return File(hotel.HotelImageContent, hotel.HotelImageContentType);
            }
            else if (imageType == 2 && hotel != null && hotel.CertificateOfOccupancyDocContent != null)
            {
                return File(hotel.CertificateOfOccupancyDocContent, hotel.CertificateOfOccupancyDoContentType);
            }
            else if (imageType == 3 && hotel != null && hotel.COADocContent != null)
            {
                return File(hotel.COADocContent, hotel.COADocContentType);
            }
            else
            {
                // Return a default image or an error image
                // depending on your requirements
                return File("~/Content/default_image.jpg", "image/jpeg");
            }
        }



        // GET: Hotels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                var currentUser = User.Identity.Name;
                var findHotel = db.Hotels.Include(x => x.HotelUser).FirstOrDefault(x => x.HotelUser.UserName == currentUser);
                if (findHotel == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                id = findHotel.Id;
            }
            Hotel hotel = db.Hotels.Find(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            return View(hotel);
        }

        // GET: Hotels/Create
        public ActionResult Create()
        {
            ViewBag.HotelUserId = new SelectList(db.HotelUsers, "Id", "FirstName");
            return View();
        }

        // POST: Hotels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Hotel hotel, List<HttpPostedFileBase> documents)
        {
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();
            if (ModelState.IsValid)
            {
                hotel.HotelUserId = AppHelper.CurrentHotelUser()?.Id;
                hotel.CreatedOn = DateTime.Now;
                if (documents.Count() >= 1)
                {
                    var file = documents[0];
                    var fileContent = file.InputStream;

                    hotel.HotelImageName = file.FileName;
                    hotel.HotelImageContentType = file.ContentType;
                    hotel.HotelImageContent = new byte[fileContent.Length];
                    fileContent.Read(hotel.HotelImageContent, 0, (int)fileContent.Length);
                    hotel.HotelImageFileSize = (Int64)file.ContentLength;
                }
                if (documents.Count() >= 2)
                {
                    var file = documents[1];
                    var fileContent = file.InputStream;

                    hotel.CertificateOfOccupancyDocName = file.FileName;
                    hotel.CertificateOfOccupancyDoContentType = file.ContentType;
                    hotel.CertificateOfOccupancyDocContent = new byte[fileContent.Length];
                    fileContent.Read(hotel.CertificateOfOccupancyDocContent, 0, (int)fileContent.Length);
                    hotel.CertificateOfOccupancyDoFileSize = (Int64)file.ContentLength;
                }
                if (documents.Count() >= 3)
                {
                    var file = documents[2];
                    var fileContent = file.InputStream;

                    hotel.COADocName = file.FileName;
                    hotel.COADocContentType = file.ContentType;
                    hotel.COADocContent = new byte[fileContent.Length];
                    fileContent.Read(hotel.COADocContent, 0, (int)fileContent.Length);
                    hotel.COADocFileSize = (Int64)file.ContentLength;
                }

                db.Hotels.Add(hotel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HotelUserId = new SelectList(db.HotelUsers, "Id", "FirstName", hotel.HotelUserId);
            return View(hotel);
        }

        // GET: Hotels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotel hotel = db.Hotels.Find(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            ViewBag.HotelUserId = new SelectList(db.HotelUsers, "Id", "FirstName", hotel.HotelUserId);
            return View(hotel);
        }

        // POST: Hotels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,MaxOccupancy,PhysicalAddress1,PhysicalAddress2,PhysicalAddress3,PhysicalAddress4,PhysicalAddress5,PhysicalAddressCode,HotelUserId,HotelImageName,HotelImageContent,HotelImageContentType,HotelImageFileSize,IsVerified")] Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hotel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HotelUserId = new SelectList(db.HotelUsers, "Id", "FirstName", hotel.HotelUserId);
            return View(hotel);
        }

        // GET: Hotels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotel hotel = db.Hotels.Find(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            return View(hotel);
        }

        // POST: Hotels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Hotel hotel = db.Hotels.Find(id);
            db.Hotels.Remove(hotel);
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