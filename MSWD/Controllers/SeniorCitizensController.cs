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
    public class SeniorCitizensController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SeniorCitizens
        public ActionResult Index()
        {
            return View(db.SeniorCitizens.ToList());
        }

        // GET: SeniorCitizens/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SeniorCitizen seniorCitizen = db.SeniorCitizens.Find(id);
            if (seniorCitizen == null)
            {
                return HttpNotFound();
            }
            return View(seniorCitizen);
        }

        // GET: SeniorCitizens/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SeniorCitizens/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SeniorCitizenId")] SeniorCitizen seniorCitizen)
        {
            if (ModelState.IsValid)
            {
                db.SeniorCitizens.Add(seniorCitizen);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(seniorCitizen);
        }

        // GET: SeniorCitizens/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SeniorCitizen seniorCitizen = db.SeniorCitizens.Find(id);
            if (seniorCitizen == null)
            {
                return HttpNotFound();
            }
            return View(seniorCitizen);
        }

        // POST: SeniorCitizens/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SeniorCitizenId")] SeniorCitizen seniorCitizen)
        {
            if (ModelState.IsValid)
            {
                db.Entry(seniorCitizen).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(seniorCitizen);
        }

        // GET: SeniorCitizens/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SeniorCitizen seniorCitizen = db.SeniorCitizens.Find(id);
            if (seniorCitizen == null)
            {
                return HttpNotFound();
            }
            return View(seniorCitizen);
        }

        // POST: SeniorCitizens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SeniorCitizen seniorCitizen = db.SeniorCitizens.Find(id);
            db.SeniorCitizens.Remove(seniorCitizen);
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
