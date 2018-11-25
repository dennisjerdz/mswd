using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MSWD.Models;
using System.Diagnostics;
using System.Web.Configuration;
using Globe.Connect;

namespace MSWD.Controllers
{
    public class SoloParentsController : Controller
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
            SoloParent c = db.SoloParents.FirstOrDefault(s => s.SoloParentId == id);

            if (c == null)
            {
                return HttpNotFound();
            }
            else
            {
                c.Status = "For Home Visit";
                db.SaveChanges();

                #region SMS NOTIF
                if (c.Client.MobileNumbers != null)
                {
                    MobileNumber mb = c.Client.MobileNumbers.FirstOrDefault(m => m.IsDisabled == false && m.Token != null);

                    if (mb != null)
                    {
                        try
                        {
                            SMS(mb.MobileNo, "Hello " + c.Client.GivenName + ", your Solo Parent Application status is now For Home Visit. Please wait for the updates regarding home visit date.");
                        }
                        catch (Exception e)
                        {
                            Trace.TraceInformation(e.Message);
                        }
                    }
                }
                #endregion

                return RedirectToAction("Index", new { });
            }
        }

        public ActionResult UpdateSPHomeVisitDate()
        {
            int id = Convert.ToInt16(Request.Form["SoloParentId"]);
            string unparsedDate = Request.Form["Date"];

            DateTime date = DateTime.Parse(unparsedDate);

            SoloParent c = db.SoloParents.FirstOrDefault(s => s.SoloParentId == id);

            if (c == null)
            {
                return HttpNotFound();
            }
            else
            {
                c.InterviewDate = date;
                db.SaveChanges();

                #region SMS NOTIF
                if (c.Client.MobileNumbers != null)
                {
                    MobileNumber mb = c.Client.MobileNumbers.FirstOrDefault(m => m.IsDisabled == false && m.Token != null);

                    if (mb != null)
                    {
                        try
                        {
                            SMS(mb.MobileNo, "Hello " + c.Client.GivenName + ", the home visit date for your Solo Parent Application is on " + date + ".");
                        }
                        catch (Exception e)
                        {
                            Trace.TraceInformation(e.Message);
                        }
                    }
                }
                #endregion

                return RedirectToAction("Index", new { });
            }
        }

        public ActionResult UpdateSPApproved(int? id)
        {
            SoloParent c = db.SoloParents.FirstOrDefault(s => s.SoloParentId == id);

            if (c == null)
            {
                return HttpNotFound();
            }
            else
            {
                c.Status = "Approved";
                db.SaveChanges();

                #region SMS NOTIF
                if (c.Client.MobileNumbers != null)
                {
                    MobileNumber mb = c.Client.MobileNumbers.FirstOrDefault(m => m.IsDisabled == false && m.Token != null);

                    if (mb != null)
                    {
                        try
                        {
                            SMS(mb.MobileNo, "Hello " + c.Client.GivenName + ", your Solo Parent Application has been approved. Please wait for the updates regarding release date.");
                        }
                        catch (Exception e)
                        {
                            Trace.TraceInformation(e.Message);
                        }
                    }
                }
                #endregion

                return RedirectToAction("Index", new { });
            }
        }

        public ActionResult UpdateSPReleaseDate()
        {
            int id = Convert.ToInt16(Request.Form["SoloParentId"]);
            string unparsedDate = Request.Form["Date"];

            DateTime date = DateTime.Parse(unparsedDate);

            SoloParent c = db.SoloParents.FirstOrDefault(s => s.SoloParentId == id);

            if (c == null)
            {
                return HttpNotFound();
            }
            else
            {
                c.ReleaseDate = date;
                db.SaveChanges();

                #region SMS NOTIF
                if (c.Client.MobileNumbers != null)
                {
                    MobileNumber mb = c.Client.MobileNumbers.FirstOrDefault(m => m.IsDisabled == false && m.Token != null);

                    if (mb != null)
                    {
                        try
                        {
                            SMS(mb.MobileNo, "Hello " + c.Client.GivenName + ", the release date for Solo Parent ID is on " + date + ".");
                        }
                        catch (Exception e)
                        {
                            Trace.TraceInformation(e.Message);
                        }
                    }
                }
                #endregion

                return RedirectToAction("Index", new { });
            }
        }

        public ActionResult UpdateSPRejected(int? id)
        {
            SoloParent c = db.SoloParents.FirstOrDefault(s => s.SoloParentId == id);

            if (c == null)
            {
                return HttpNotFound();
            }
            else
            {
                c.Status = "Rejected";
                db.SaveChanges();

                #region SMS NOTIF
                if (c.Client.MobileNumbers != null)
                {
                    MobileNumber mb = c.Client.MobileNumbers.FirstOrDefault(m => m.IsDisabled == false && m.Token != null);

                    if (mb != null)
                    {
                        try
                        {
                            SMS(mb.MobileNo, "Hello " + c.Client.GivenName + ", your Solo Parent Application has been rejected.");
                        }
                        catch (Exception e)
                        {
                            Trace.TraceInformation(e.Message);
                        }
                    }
                }
                #endregion

                return RedirectToAction("Index", new { });
            }
        }

        public ActionResult UpdateSPPending(int? id)
        {
            SoloParent c = db.SoloParents.FirstOrDefault(s => s.SoloParentId == id);

            if (c == null)
            {
                return HttpNotFound();
            }
            else
            {
                c.Status = "Pending";
                db.SaveChanges();

                #region SMS NOTIF
                if (c.Client.MobileNumbers != null)
                {
                    MobileNumber mb = c.Client.MobileNumbers.FirstOrDefault(m => m.IsDisabled == false && m.Token != null);

                    if (mb != null)
                    {
                        try
                        {
                            SMS(mb.MobileNo, "Hello " + c.Client.GivenName + ", your Solo Parent Application is now Pending.");
                        }
                        catch (Exception e)
                        {
                            Trace.TraceInformation(e.Message);
                        }
                    }
                }
                #endregion

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
