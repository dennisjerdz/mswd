using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MSWD.Models;
using System.IO;
using Microsoft.AspNet.Identity;

namespace MSWD.Controllers
{
    public class RequirementsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Requirements
        public ActionResult Index()
        {
            var requirements = db.Requirements.Include(r => r.Client);
            return View(requirements.ToList());
        }

        // GET: Requirements/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requirement requirement = db.Requirements.Find(id);
            if (requirement == null)
            {
                return HttpNotFound();
            }
            return View(requirement);
        }

        [HttpPost]
        public ActionResult UploadAttachment(HttpPostedFileBase Attachment)
        {
            int requirementId = Convert.ToInt16(Request["RequirementId"]);
            Requirement r = db.Requirements.FirstOrDefault(e => e.RequirementId == requirementId);

            if(Attachment != null)
            {
                string path = Server.MapPath("~/Content/Files/");

                if(!Directory.Exists(path)){
                    Directory.CreateDirectory(path);
                }

                string ext = Path.GetExtension(Attachment.FileName);

                try
                {
                    Attachment.SaveAs(path + Attachment.FileName);
                }
                catch (Exception ex)
                {
                    TempData["upload"] = 0;
                    TempData["errorMessage"] = ex.Message;
                    return RedirectToAction("Details", new { @id=requirementId });
                }

                string savePath = "/Content/Files/" + Attachment.FileName;

                RequirementAttachment ra = new RequirementAttachment();
                ra.RequirementId = requirementId;
                ra.Name = Attachment.FileName;
                ra.Location = savePath;
                ra.DateCreated = DateTime.UtcNow.AddHours(8);

                db.RequirementAttachments.Add(ra);

                db.SaveChanges();
                return RedirectToAction("Details", new { @id=requirementId });
            }

            return RedirectToAction("Details", new { @id = requirementId });
        }

        public ActionResult DeleteAttachment(int id)
        {
            RequirementAttachment ra = db.RequirementAttachments.Find(id);
            db.RequirementAttachments.Remove(ra);
            if (db.SaveChanges()>0)
            {
                string fullPath = Request.MapPath(ra.Location);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }

            return RedirectToAction("Details", new { @id=ra.RequirementId });
        }

        [HttpPost]
        public ActionResult Details(Requirement r)
        {
            RequirementComment nrc = new RequirementComment();

            foreach(RequirementComment rc in r.Comments)
            {
                nrc.DateTimeCreated = DateTime.UtcNow.AddHours(8);
                nrc.CreatedById = User.Identity.GetUserId();
                nrc.Content = rc.Content;
                nrc.RequirementId = r.RequirementId;
                db.RequirementComments.Add(nrc);
            }

            db.SaveChanges();
            return RedirectToAction("Details", new { @id=r.RequirementId });
        }

        public ActionResult ApproveRequirement(int id)
        {
            Requirement r = db.Requirements.FirstOrDefault(e => e.RequirementId == id);

            if (r == null)
            {
                return HttpNotFound();
            }

            r.IsDone = true;
            db.SaveChanges();

            return RedirectToAction("Requirements", "Clients", new { @id = r.ClientId });
        }

        public ActionResult RevertRequirement(int id)
        {
            Requirement r = db.Requirements.FirstOrDefault(e => e.RequirementId == id);

            if (r == null)
            {
                return HttpNotFound();
            }

            r.IsDone = false;
            db.SaveChanges();

            return RedirectToAction("Requirements", "Clients", new { @id = r.ClientId });
        }

        // GET: Requirements/Create
        public ActionResult Create()
        {
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "GivenName");
            return View();
        }

        // POST: Requirements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RequirementId,Name,Description,IsDone,ClientId")] Requirement requirement)
        {
            if (ModelState.IsValid)
            {
                db.Requirements.Add(requirement);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "GivenName", requirement.ClientId);
            return View(requirement);
        }

        // GET: Requirements/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requirement requirement = db.Requirements.Find(id);
            if (requirement == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "GivenName", requirement.ClientId);
            return View(requirement);
        }

        // POST: Requirements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RequirementId,Name,Description,IsDone,ClientId")] Requirement requirement)
        {
            if (ModelState.IsValid)
            {
                db.Entry(requirement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "GivenName", requirement.ClientId);
            return View(requirement);
        }

        public ActionResult DeleteComment(int id)
        {
            RequirementComment r = db.RequirementComments.Find(id);
            db.RequirementComments.Remove(r);
            db.SaveChanges();
            return RedirectToAction("Details", new { @id = r.RequirementId });
        }

        // GET: Requirements/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requirement requirement = db.Requirements.Find(id);
            if (requirement == null)
            {
                return HttpNotFound();
            }
            return View(requirement);
        }

        // POST: Requirements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Requirement requirement = db.Requirements.Find(id);
            db.Requirements.Remove(requirement);
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
