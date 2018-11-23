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
    public class PwdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Pwds
        public ActionResult Index()
        {
            var pwds = db.Pwds.Include(p => p.CreatedBy).Include(p => p.VerifiedBy);
            return View(pwds.ToList());
        }

        // GET: Pwds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pwd pwd = db.Pwds.Find(id);
            if (pwd == null)
            {
                return HttpNotFound();
            }
            return View(pwd);
        }

        // GET: Pwds/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Pwds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PwdId,Status,CreatedByUserId,VerifiedByUserId,PsychosocialMentalDisability,VisualDisability,CommunicationDisability,LearningDisability,OrthopedicDisability,IntellectualDisability,Inborn,Acquired,AgeAcquired,InterviewDate,ApplicationDate,ApprovalDate,ReleaseDate")] Pwd pwd)
        {
            if (ModelState.IsValid)
            {
                db.Pwds.Add(pwd);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(pwd);
        }

        // GET: Pwds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pwd pwd = db.Pwds.Find(id);
            if (pwd == null)
            {
                return HttpNotFound();
            }

            return View(pwd);
        }

        // POST: Pwds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PwdId,Status,CreatedByUserId,VerifiedByUserId,PsychosocialMentalDisability,VisualDisability,CommunicationDisability,LearningDisability,OrthopedicDisability,IntellectualDisability,Inborn,Acquired,AgeAcquired,InterviewDate,ApplicationDate,ApprovalDate,ReleaseDate")] Pwd pwd)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pwd).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pwd);
        }

        // GET: Pwds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pwd pwd = db.Pwds.Find(id);
            if (pwd == null)
            {
                return HttpNotFound();
            }
            return View(pwd);
        }

        // POST: Pwds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pwd pwd = db.Pwds.Find(id);
            db.Pwds.Remove(pwd);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult UpdatePWDHomeVisit(int? id)
        {
            Pwd c = db.Pwds.FirstOrDefault(s => s.PwdId == id);

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

        public ActionResult UpdatePWDHomeVisitDate()
        {
            int id = Convert.ToInt16(Request.Form["PwdId"]);
            string unparsedDate = Request.Form["Date"];

            DateTime date = DateTime.Parse(unparsedDate);

            Pwd c = db.Pwds.FirstOrDefault(s => s.PwdId == id);

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

        public ActionResult UpdatePWDApproved(int? id)
        {
            Pwd c = db.Pwds.FirstOrDefault(s => s.PwdId == id);

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

        public ActionResult UpdatePWDReleaseDate()
        {
            int id = Convert.ToInt16(Request.Form["PwdId"]);
            string unparsedDate = Request.Form["Date"];

            DateTime date = DateTime.Parse(unparsedDate);

            Pwd c = db.Pwds.FirstOrDefault(s => s.PwdId == id);

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

        public ActionResult UpdatePWDRejected(int? id)
        {
            Pwd c = db.Pwds.FirstOrDefault(s => s.PwdId == id);

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

        public ActionResult UpdatePWDPending(int? id)
        {
            Pwd c = db.Pwds.FirstOrDefault(s => s.PwdId == id);

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
