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
    public class InquiriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Inquiries
        public ActionResult Index(int? id)
        {
            if (User.IsInRole("Client"))
            {
                string email = User.Identity.Name;
                ApplicationUser user = db.Users.FirstOrDefault(c => c.Email == email);

                var inquiries = db.Inquiries.Include(i => i.Client).Include(i => i.Message).Where(m => m.ClientId == user.ClientId);
                return View(inquiries.ToList());
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

                    return View(c.Inquiries.ToList());
                }
                else
                {
                    var inquiries = db.Inquiries.Include(m => m.Client);
                    return View(inquiries.ToList());
                }
            }
        }

        [HttpPost]
        public ActionResult AddInquiry()
        {
            string category = Request.Form["Category"];
            string message = Request.Form["Message"];

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
                    return RedirectToAction("Index", "Inquiries");
                }
                else
                {
                    Inquiry i = new Inquiry();
                    i.DateCreated = DateTime.UtcNow.AddHours(8);
                    i.ClientId = u.ClientId.Value;
                    i.Status = "Pending";
                    i.Category = category;
                    i.Content = message;

                    db.Inquiries.Add(i);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
        }

        public ActionResult Revert()
        {
            return null;
        }

        public ActionResult Resolve()
        {
            return null;
        }

        public ActionResult Reply()
        {
            return null;
        }

        public ActionResult Replies()
        {
            return null;
        }

        // GET: Inquiries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inquiry inquiry = db.Inquiries.Find(id);
            if (inquiry == null)
            {
                return HttpNotFound();
            }
            return View(inquiry);
        }

        // GET: Inquiries/Create
        public ActionResult Create()
        {
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "GivenName");
            ViewBag.MessageId = new SelectList(db.Messages, "MessageId", "Body");
            return View();
        }

        // POST: Inquiries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InquiryId,ClientId,MessageId,Status,Category,Content,DateCreated")] Inquiry inquiry)
        {
            if (ModelState.IsValid)
            {
                db.Inquiries.Add(inquiry);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "GivenName", inquiry.ClientId);
            ViewBag.MessageId = new SelectList(db.Messages, "MessageId", "Body", inquiry.MessageId);
            return View(inquiry);
        }

        // GET: Inquiries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inquiry inquiry = db.Inquiries.Find(id);
            if (inquiry == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "GivenName", inquiry.ClientId);
            ViewBag.MessageId = new SelectList(db.Messages, "MessageId", "Body", inquiry.MessageId);
            return View(inquiry);
        }

        // POST: Inquiries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InquiryId,ClientId,MessageId,Status,Category,Content,DateCreated")] Inquiry inquiry)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inquiry).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "GivenName", inquiry.ClientId);
            ViewBag.MessageId = new SelectList(db.Messages, "MessageId", "Body", inquiry.MessageId);
            return View(inquiry);
        }

        // GET: Inquiries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inquiry inquiry = db.Inquiries.Find(id);
            if (inquiry == null)
            {
                return HttpNotFound();
            }

            db.Inquiries.Remove(inquiry);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Inquiries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inquiry inquiry = db.Inquiries.Find(id);
            db.Inquiries.Remove(inquiry);
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
