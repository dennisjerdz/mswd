using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MSWD.Models;
using System.Web.Configuration;
using Globe.Connect;
using System.Diagnostics;

namespace MSWD.Controllers
{
    public class InquiriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        #region SMS
        public string short_code = WebConfigurationManager.AppSettings["ShortCode"];

        private ActionResult SMS(string mobile_number, string message)
        {
            MobileNumber mb = db.MobileNumbers.FirstOrDefault(m => m.MobileNo == mobile_number);
            string access_token = mb.Token;

            if (access_token != null)
            {
                Sms sms = new Sms(short_code, access_token);

                // mobile number argument is with format 09, convert it to +639
                string globe_format_receiver = "+63" + mobile_number.Substring(1);

                dynamic response = sms.SetReceiverAddress(globe_format_receiver)
                    .SetMessage(message)
                    .SendMessage()
                    .GetDynamicResponse();

                Trace.TraceInformation("Sent message; " + message + " to; " + globe_format_receiver + ".");
            }

            return null;
        }
        #endregion

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

        public ActionResult Revert(int? id)
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

            inquiry.Status = "Pending";
            db.SaveChanges();

            #region SMS NOTIF
            if (inquiry.Client.MobileNumbers != null)
            {
                MobileNumber mb = inquiry.Client.MobileNumbers.FirstOrDefault(m => m.IsDisabled == false && m.Token != null);

                if (mb != null)
                {
                    try
                    {
                        SMS(mb.MobileNo, "Your inquiry; " + inquiry.Content + ", has been reverted to Pending.");
                    }
                    catch (Exception e)
                    {
                        Trace.TraceInformation(e.Message);
                    }
                }
            }
            #endregion

            return RedirectToAction("Index", new { id = inquiry.ClientId });
        }

        public ActionResult Resolve(int? id)
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

            inquiry.Status = "Resolved";
            db.SaveChanges();

            #region SMS NOTIF
            if (inquiry.Client.MobileNumbers != null)
            {
                MobileNumber mb = inquiry.Client.MobileNumbers.FirstOrDefault(m => m.IsDisabled == false && m.Token != null);

                if (mb != null)
                {
                    try
                    {
                        SMS(mb.MobileNo, "Your inquiry; " + inquiry.Content + ", is now resolved.");
                    }
                    catch (Exception e)
                    {
                        Trace.TraceInformation(e.Message);
                    }
                }
            }
            #endregion

            return RedirectToAction("Index", new { id = inquiry.ClientId });
        }

        [HttpPost]
        public ActionResult Reply()
        {
            string message = Request.Form["Message"];
            string inquiryId = Request.Form["InquiryId"];

            Inquiry inquiry = db.Inquiries.Find(Convert.ToInt16(inquiryId));

            #region SMS NOTIF
            if (inquiry.Client.MobileNumbers != null)
            {
                MobileNumber mb = inquiry.Client.MobileNumbers.FirstOrDefault(m => m.IsDisabled == false && m.Token != null);

                if (mb != null)
                {
                    try
                    {
                        SMS(mb.MobileNo, message);
                    }
                    catch (Exception e)
                    {
                        Trace.TraceInformation(e.Message);
                    }
                }
            }
            #endregion

            return RedirectToAction("Index", new { id = inquiry.ClientId });
        }

        public ActionResult Replies(int? id)
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
