using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HotelListingSystem.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace HotelListingSystem.Controllers
{
    public class HotelUsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: HotelUsers
        public ActionResult Index()
        {
            return View(db.HotelUsers.ToList());
        }

        // GET: HotelUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                var currentUser = User.Identity.Name;
                var findHotel = db.HotelUsers.FirstOrDefault(x => x.UserName == currentUser);
                if (findHotel == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                id = findHotel.Id;
            }
            HotelUsers hotelUsers = db.HotelUsers.Find(id);
            if (hotelUsers == null)
            {
                return HttpNotFound();
            }
            return View(hotelUsers);
        }
        public ActionResult Profile(int? id)
        {
            if (id == null)
            {
                var currentUser = User.Identity.Name;
                var findHotel = db.HotelUsers.Find(id);
                if (findHotel == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                id = findHotel.Id;
            }
            HotelUsers hotelUsers = db.HotelUsers.Find(id);
            if (hotelUsers == null)
            {
                return HttpNotFound();
            }
            return View(hotelUsers);
        }


        // GET: HotelUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HotelUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,UserName,CompanyName,Designation,EmailAddress,HotelUserType,StatusId,IsPasswordReset,IdentificationNumber,MobileNumber,CreatedOn")] HotelUsers hotelUsers)
        {
            if (ModelState.IsValid)
            {
                db.HotelUsers.Add(hotelUsers);
                db.SaveChanges();

                if (hotelUsers.HotelUserType == "Business")
                {
                    return RedirectToAction("Create", "Business");
                }
                else
                {
                    return RedirectToAction("Create", "Hotels");
                }
            }

            return View(hotelUsers);
        }

        // GET: HotelUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                var currentUser = User.Identity.Name;
                var findHotel = db.HotelUsers.FirstOrDefault(x => x.UserName == currentUser);
                if (findHotel == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                id = findHotel.Id;
            }
            HotelUsers hotelUsers = db.HotelUsers.Find(id);
            if (hotelUsers == null)
            {
                return HttpNotFound();
            }
            return View(hotelUsers);
        }

        // POST: HotelUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,UserName,CompanyName,Designation,EmailAddress,HotelUserType,StatusId,IsPasswordReset,IdentificationNumber,MobileNumber,CreatedOn")] HotelUsers hotelUsers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hotelUsers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hotelUsers);
        }

        // GET: HotelUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HotelUsers hotelUsers = db.HotelUsers.Find(id);
            if (hotelUsers == null)
            {
                return HttpNotFound();
            }
            return View(hotelUsers);
        }

        // POST: HotelUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HotelUsers hotelUsers = db.HotelUsers.Find(id);
            db.HotelUsers.Remove(hotelUsers);
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
