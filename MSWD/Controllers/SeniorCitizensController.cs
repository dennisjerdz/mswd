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
    [Authorize(Roles = "Social Worker,OIC")]
    public class SeniorCitizensController : Controller
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

        /* Application Status Update */

        public ActionResult UpdateSCHomeVisit(int? id)
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

                #region SMS NOTIF
                if (c.Client.MobileNumbers != null)
                {
                    MobileNumber mb = c.Client.MobileNumbers.FirstOrDefault(m => m.IsDisabled == false && m.Token != null);

                    if (mb != null)
                    {
                        try
                        {
                            SMS(mb.MobileNo, "Hello " + c.Client.GivenName + ", your Senior Citizen Application status is now For Home Visit. Please wait for the updates regarding home visit date.");
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

        public ActionResult UpdateSCHomeVisitDate()
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

                #region SMS NOTIF
                if (c.Client.MobileNumbers != null)
                {
                    MobileNumber mb = c.Client.MobileNumbers.FirstOrDefault(m => m.IsDisabled == false && m.Token != null);

                    if (mb != null)
                    {
                        try
                        {
                            SMS(mb.MobileNo, "Hello " + c.Client.GivenName + ", the home visit date for your Senior Citizen Application is on " + date + ".");
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

        public ActionResult UpdateSCApproved(int? id)
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

                #region SMS NOTIF
                if (c.Client.MobileNumbers != null)
                {
                    MobileNumber mb = c.Client.MobileNumbers.FirstOrDefault(m => m.IsDisabled == false && m.Token != null);

                    if (mb != null)
                    {
                        try
                        {
                            SMS(mb.MobileNo, "Hello " + c.Client.GivenName + ", your Senior Citizen Application has been approved. Please wait for the updates regarding release date.");
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

        public ActionResult UpdateSCReleaseDate()
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

                #region SMS NOTIF
                if (c.Client.MobileNumbers != null)
                {
                    MobileNumber mb = c.Client.MobileNumbers.FirstOrDefault(m => m.IsDisabled == false && m.Token != null);

                    if (mb != null)
                    {
                        try
                        {
                            SMS(mb.MobileNo, "Hello " + c.Client.GivenName + ", the release date for Senior Citizen ID is on " + date + ".");
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

        public ActionResult UpdateSCRejected(int? id)
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

                #region SMS NOTIF
                if (c.Client.MobileNumbers != null)
                {
                    MobileNumber mb = c.Client.MobileNumbers.FirstOrDefault(m => m.IsDisabled == false && m.Token != null);

                    if (mb != null)
                    {
                        try
                        {
                            SMS(mb.MobileNo, "Hello " + c.Client.GivenName + ", your Senior Citizen Application has been rejected.");
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

        public ActionResult UpdateSCPending(int? id)
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

                #region SMS NOTIF
                if (c.Client.MobileNumbers != null)
                {
                    MobileNumber mb = c.Client.MobileNumbers.FirstOrDefault(m => m.IsDisabled == false && m.Token != null);

                    if (mb != null)
                    {
                        try
                        {
                            SMS(mb.MobileNo, "Hello " + c.Client.GivenName + ", your Senior Citizen Application is now Pending.");
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
