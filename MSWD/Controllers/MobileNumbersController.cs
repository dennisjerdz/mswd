using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MSWD.Models;

namespace MSWD.Controllers
{
    public class MobileNumbersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MobileNumbers
        public ActionResult Index(int? id)
        {
            ViewBag.Error = TempData["Error"];

            if (User.IsInRole("Client"))
            {
                string email = User.Identity.Name;
                ApplicationUser user = db.Users.FirstOrDefault(c => c.Email == email);

                var mobileNumbers = db.MobileNumbers.Include(m => m.Client).Where(m => m.ClientId == user.ClientId);
                return View(mobileNumbers.ToList());
            }
            else
            {
                if (id != null)
                {
                    Client c = db.Clients.FirstOrDefault(l => l.ClientId == id);

                    if (c == null)
                    {
                        return HttpNotFound();
                    }

                    return View(c.MobileNumbers.ToList());
                }
                else
                {
                    var mobileNumbers = db.MobileNumbers.Include(m => m.Client);
                    return View(mobileNumbers.ToList());
                }
            }
        }

        public ActionResult AddMobileNumber()
        {
            string mobileNumber = Request.Form["MobileNumber"];

            if (mobileNumber.Length != 11)
            {
                TempData["Error"] = "Incorrect Mobile Number Format, please input your 11-digit mobile number.";
                return RedirectToAction("Index", "MobileNumbers");
            }

            string email = User.Identity.Name;
            ApplicationUser u = db.Users.FirstOrDefault(s => s.Email == email);

            if (u == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                if (u.ClientId == null)
                {
                    TempData["Error"] = "Client Application not found, please apply first.";
                    return RedirectToAction("Index", "MobileNumbers");
                }
                else
                {
                    MobileNumber mb = new MobileNumber();
                    mb.ClientId = u.ClientId.Value;
                    mb.MobileNo = mobileNumber;
                    mb.DateCreated = DateTime.UtcNow.AddHours(8);
                    mb.IsDisabled = false;

                    db.MobileNumbers.Add(mb);
                    db.SaveChanges();

                    return RedirectToAction("Index", "MobileNumbers");
                }
            }
        }

        public ActionResult RemoveDisabled(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MobileNumber mobileNumber = db.MobileNumbers.Find(id);
            if (mobileNumber == null)
            {
                return HttpNotFound();
            }

            mobileNumber.IsDisabled = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Disable(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MobileNumber mobileNumber = db.MobileNumbers.Find(id);
            if (mobileNumber == null)
            {
                return HttpNotFound();
            }

            mobileNumber.IsDisabled = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MobileNumber mobileNumber = db.MobileNumbers.Find(id);
            if (mobileNumber == null)
            {
                return HttpNotFound();
            }

            db.Messages.RemoveRange(mobileNumber.Messages);
            db.MobileNumbers.Remove(mobileNumber);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: MobileNumbers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MobileNumber mobileNumber = db.MobileNumbers.Find(id);
            if (mobileNumber == null)
            {
                return HttpNotFound();
            }
            return View(mobileNumber);
        }

        // GET: MobileNumbers/Create
        public ActionResult Create()
        {
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "GivenName");
            return View();
        }

        // POST: MobileNumbers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MobileNumberId,MobileNo,Token,IsDisabled,ClientId,DateCreated")] MobileNumber mobileNumber)
        {
            if (ModelState.IsValid)
            {
                db.MobileNumbers.Add(mobileNumber);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "GivenName", mobileNumber.ClientId);
            return View(mobileNumber);
        }

        // GET: MobileNumbers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MobileNumber mobileNumber = db.MobileNumbers.Find(id);
            if (mobileNumber == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "GivenName", mobileNumber.ClientId);
            return View(mobileNumber);
        }

        // POST: MobileNumbers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MobileNumberId,MobileNo,Token,IsDisabled,ClientId,DateCreated")] MobileNumber mobileNumber)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mobileNumber).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "GivenName", mobileNumber.ClientId);
            return View(mobileNumber);
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
