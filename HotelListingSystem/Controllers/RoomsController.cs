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
    public class RoomsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Rooms
        public ActionResult IndexAll()
        {
            return View(db.Rooms.ToList());
        }
        public ActionResult Index()
        {
            var user = AppHelper.CurrentHotelUser().Id;
            var rooms = db.Rooms.Include(x => x.Hotel).Where(x => x.Hotel.HotelUserId == user).ToList();
            return View(rooms);
        }

        // GET: Rooms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // GET: Rooms/Create
        public ActionResult Create(int? hotelId)
        {
            Room room = new Room();
            if (hotelId != null )
            {
                ViewBag.AddRooms = "true";
                room.HotelId = hotelId;
            }
            ViewBag.HotelId = new SelectList(db.Hotels, "Id", "Name");
            return View(room);
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Room room, List<HttpPostedFileBase> documents)
        {
            if (ModelState.IsValid)
            {
                room.CreatedOn = DateTime.Now;
                if (documents?.Count() >= 1)
                {
                    var file = documents[0];
                    var fileContent = file.InputStream;

                    room.RoomImageName1 = file.FileName;
                    room.RoomImageContentType1 = file.ContentType;
                    room.RoomImageContent1 = new byte[fileContent.Length];
                    fileContent.Read(room.RoomImageContent1, 0, (int)fileContent.Length);
                    room.RoomImageFileSize1 = (Int64)file.ContentLength;
                }
                if (documents?.Count() >= 2)
                {
                    var file = documents[1];
                    var fileContent = file.InputStream;

                    room.RoomImageName2 = file.FileName;
                    room.RoomImageContentType2 = file.ContentType;
                    room.RoomImageContent2 = new byte[fileContent.Length];
                    fileContent.Read(room.RoomImageContent2, 0, (int)fileContent.Length);
                    room.RoomImageFileSize2 = (Int64)file.ContentLength;
                }

                db.Rooms.Add(room);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HotelId = new SelectList(db.Hotels, "Id", "Name");
            return View(room);
        }

        public ActionResult DisplayImage(int roomId, int imageType = 1)
        {
            var hotel = db.Rooms.FirstOrDefault(h => h.Id == roomId);
            if (imageType == 1 && hotel != null && hotel.RoomImageContent1 != null)
            {
                return File(hotel.RoomImageContent1, hotel.RoomImageContentType1);
            }
            else if (imageType == 2 && hotel != null && hotel.RoomImageContent2 != null)
            {
                return File(hotel.RoomImageContent1, hotel.RoomImageContentType2);
            }
            else
            {
                // Return a default image or an error image
                // depending on your requirements
                return File("~/Content/default_image.jpg", "image/jpeg");
            }
        }


        // GET: Rooms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Sleeps,NoOfBeds,Baths,Balcony,RoomImageName1,RoomImageContent1,RoomImageContentType1,RoomImageFileSize1,RoomImageName2,RoomImageContent2,RoomImageContentType2,RoomImageFileSize2")] Room room)
        {
            if (ModelState.IsValid)
            {
                db.Entry(room).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(room);
        }

        // GET: Rooms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Room room = db.Rooms.Find(id);
            db.Rooms.Remove(room);
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