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
            var seniorCitizens = db.SeniorCitizens.Include(s => s.CreatedBy).Include(s => s.VerifiedBy);
            return View(seniorCitizens.ToList());
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
            ClientEditModel ce = new ClientEditModel();
            return View(ce);
        }

        // POST: SeniorCitizens/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientEditModel ce)
        {
            Client newClient = new Client(ce);

            newClient.CityId = 1;
            newClient.DateCreated = DateTime.UtcNow.AddHours(8);
            // ce.CreatedByUserId = "";

            db.Clients.Add(newClient);

            SeniorCitizen newSC = new SeniorCitizen();
            newSC.Status = "Pending";
            newSC.Client = newClient;

            db.SeniorCitizens.Add(newSC);

            ce.ClientBeneficiaries.ForEach(c => c.ClientId = newClient.ClientId);
            ce.ClientBeneficiaries.ForEach(c => db.ClientBeneficiary.Add(c));
            
            if (db.SaveChanges() > 1)
            {
                return RedirectToAction("Index","SeniorCitizens",null);
            }else
            {
                return View(ce);
            }
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
            ViewBag.CreatedByUserId = new SelectList(db.Users, "Id", "Email", seniorCitizen.CreatedByUserId);
            ViewBag.VerifiedByUserId = new SelectList(db.Users, "Id", "Email", seniorCitizen.VerifiedByUserId);
            return View(seniorCitizen);
        }

        // POST: SeniorCitizens/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SeniorCitizenId,Status,CreatedByUserId,VerifiedByUserId")] SeniorCitizen seniorCitizen)
        {
            if (ModelState.IsValid)
            {
                db.Entry(seniorCitizen).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreatedByUserId = new SelectList(db.Users, "Id", "Email", seniorCitizen.CreatedByUserId);
            ViewBag.VerifiedByUserId = new SelectList(db.Users, "Id", "Email", seniorCitizen.VerifiedByUserId);
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
