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
    public class SoloParentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SoloParents
        public ActionResult Index()
        {
            var soloParents = db.SoloParents.Include(s => s.CreatedBy).Include(s => s.VerifiedBy);
            return View(soloParents.ToList());
        }

        // GET: SoloParents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SoloParent soloParent = db.SoloParents.Find(id);
            if (soloParent == null)
            {
                return HttpNotFound();
            }
            return View(soloParent);
        }

        // GET: SoloParents/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: SoloParents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SoloParentId,Status,CreatedByUserId,VerifiedByUserId,DateCreated,InterviewDate,ApplicationDate,ApprovalDate,ReleaseDate")] SoloParent soloParent)
        {
            if (ModelState.IsValid)
            {
                db.SoloParents.Add(soloParent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            return View(soloParent);
        }

        // GET: SoloParents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SoloParent soloParent = db.SoloParents.Find(id);
            if (soloParent == null)
            {
                return HttpNotFound();
            }

            return View(soloParent);
        }

        // POST: SoloParents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SoloParentId,Status,CreatedByUserId,VerifiedByUserId,DateCreated,InterviewDate,ApplicationDate,ApprovalDate,ReleaseDate")] SoloParent soloParent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(soloParent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(soloParent);
        }

        // GET: SoloParents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SoloParent soloParent = db.SoloParents.Find(id);
            if (soloParent == null)
            {
                return HttpNotFound();
            }
            return View(soloParent);
        }

        // POST: SoloParents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SoloParent soloParent = db.SoloParents.Find(id);
            db.SoloParents.Remove(soloParent);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult UpdateSPHomeVisit(int? id)
        {
            SeniorCitizen c = db.SeniorCitizens.FirstOrDefault(s => s.SeniorCitizenId == id);

            if (c == null)
            {
                return HttpNotFound();
            }
            else
            {
                c.Status = "For Home Visit";
                db.SaveChanges();
                return RedirectToAction("Index", new { });
            }
        }

        public ActionResult UpdateSPHomeVisitDate()
        {
            int id = Convert.ToInt16(Request.Form["SeniorCitizenId"]);
            string unparsedDate = Request.Form["Date"];

            DateTime date = DateTime.Parse(unparsedDate);

            SeniorCitizen c = db.SeniorCitizens.FirstOrDefault(s => s.SeniorCitizenId == id);

            if (c == null)
            {
                return HttpNotFound();
            }
            else
            {
                c.InterviewDate = date;
                db.SaveChanges();
                return RedirectToAction("Index", new { });
            }
        }

        public ActionResult UpdateSPApproved(int? id)
        {
            SeniorCitizen c = db.SeniorCitizens.FirstOrDefault(s => s.SeniorCitizenId == id);

            if (c == null)
            {
                return HttpNotFound();
            }
            else
            {
                c.Status = "Approved";
                db.SaveChanges();
                return RedirectToAction("Index", new { });
            }
        }

        public ActionResult UpdateSPReleaseDate()
        {
            int id = Convert.ToInt16(Request.Form["SeniorCitizenId"]);
            string unparsedDate = Request.Form["Date"];

            DateTime date = DateTime.Parse(unparsedDate);

            SeniorCitizen c = db.SeniorCitizens.FirstOrDefault(s => s.SeniorCitizenId == id);

            if (c == null)
            {
                return HttpNotFound();
            }
            else
            {
                c.ReleaseDate = date;
                db.SaveChanges();
                return RedirectToAction("Index", new { });
            }
        }

        public ActionResult UpdateSPRejected(int? id)
        {
            SeniorCitizen c = db.SeniorCitizens.FirstOrDefault(s => s.SeniorCitizenId == id);

            if (c == null)
            {
                return HttpNotFound();
            }
            else
            {
                c.Status = "Rejected";
                db.SaveChanges();
                return RedirectToAction("Index", new { });
            }
        }

        public ActionResult UpdateSPPending(int? id)
        {
            SeniorCitizen c = db.SeniorCitizens.FirstOrDefault(s => s.SeniorCitizenId == id);

            if (c == null)
            {
                return HttpNotFound();
            }
            else
            {
                c.Status = "Pending";
                db.SaveChanges();
                return RedirectToAction("Index", new { });
            }
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
