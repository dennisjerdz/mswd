using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MSWD.Models;
using Microsoft.AspNet.Identity;
using System.Security.Principal;
using System.Security.Claims;
using Globe.Connect;
using System.Diagnostics;
using System.Web.Configuration;

namespace MSWD.Controllers
{
    [Authorize(Roles = "Social Worker,OIC,Client")]
    public class ClientsController : Controller
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

        // GET: Clients
        public ActionResult Index()
        {
            var clients = db.Clients.Include(c => c.City).Include(c => c.Pwd).Include(c => c.SeniorCitizen).Include(c => c.SoloParent);
            return View(clients.ToList());
        }

        // GET: Clients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // GET: Clients/Create
        public ActionResult Create()
        {
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "Name");
            ViewBag.CreatedByUserId = new SelectList(db.Users, "Id", "Email");
            ViewBag.ClientId = new SelectList(db.Pwds, "PwdId", "Status");
            ViewBag.ClientId = new SelectList(db.SeniorCitizens, "SeniorCitizenId", "Status");
            ViewBag.ClientId = new SelectList(db.SoloParents, "SoloParentId", "Status");
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClientId,GivenName,MiddleName,SurName,Gender,CivilStatus,Occupation,Citizenship,CityId,CityAddress,ProvincialAddress,ContactNumber,TypeOfResidency,StartOfResidency,BirthDate,BirthPlace,Religion,DateOfMarriage,PlaceOfMarriage,SpouseName,SpouseBirthDate,CreatedByUserId,DateCreated")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Clients.Add(client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityId = new SelectList(db.Cities, "CityId", "Name", client.CityId);
            //ViewBag.CreatedByUserId = new SelectList(db.Users, "Id", "Email", client.CreatedByUserId);
            ViewBag.ClientId = new SelectList(db.Pwds, "PwdId", "Status", client.ClientId);
            ViewBag.ClientId = new SelectList(db.SeniorCitizens, "SeniorCitizenId", "Status", client.ClientId);
            ViewBag.ClientId = new SelectList(db.SoloParents, "SoloParentId", "Status", client.ClientId);
            return View(client);
        }

        // GET: Clients/Details/5
        public ActionResult Manage(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        [HttpPost]
        public ActionResult Manage([Bind(Include = "ClientId,GivenName,MiddleName,SurName,Gender,CivilStatus,Occupation,Citizenship,CityId,CityAddress,ProvincialAddress,ContactNumber,TypeOfResidency,StartOfResidency,BirthDate,BirthPlace,Religion,DateOfMarriage,PlaceOfMarriage,SpouseName,SpouseBirthDate,CreatedByUserId,DateCreated,ClientBeneficiaries")] Client client)
        {
            if (ModelState.IsValid)
            {
                //Client dbClient = db.Clients.FirstOrDefault(c => c.ClientId == client.ClientId);
                //db.ClientBeneficiary.RemoveRange(dbClient.ClientBeneficiaries);

                /*if (db.Entry(client).Entity.ClientBeneficiaries != null) {
                    db.ClientBeneficiary.RemoveRange(db.Entry(client).Entity.ClientBeneficiaries);
                }*/

                /*
                if (client.ClientBeneficiaries != null)
                {
                    foreach (var item in client.ClientBeneficiaries)
                    {
                        if (db.ClientBeneficiary.FirstOrDefault(b=>b.ClientBeneficiaryId == item.ClientBeneficiaryId) != null) {
                            // db.Entry(item).State = EntityState.Modified;
                        }else
                        {
                            item.ClientId = client.ClientId;
                            db.ClientBeneficiary.Add(item);
                        }
                    }
                }
                
                db.SaveChanges();
                */

                var parentEntry = db.Entry(client);
                parentEntry.State = EntityState.Modified;

                int[] dbChildEntities = db.ClientBeneficiary.Where(b => b.ClientId == client.ClientId).Select(c=>c.ClientBeneficiaryId).ToArray();

                foreach (var item in client.ClientBeneficiaries)
                {
                    if (dbChildEntities.Contains(item.ClientBeneficiaryId)) {
                        db.Entry(item).State = EntityState.Modified;
                    }else
                    {
                        db.ClientBeneficiary.Add(item);
                    }
                }

                int[] childEntities = client.ClientBeneficiaries.Select(c => c.ClientBeneficiaryId).ToArray();

                var deleteChildEntities = db.ClientBeneficiary.Where(b => !childEntities.Contains(b.ClientBeneficiaryId));

                db.ClientBeneficiary.RemoveRange(deleteChildEntities);

                /*foreach (var item in client.ClientBeneficiaries) {
                    db.Entry(item).State = EntityState.Modified;
                }*/

                db.SaveChanges();
                return RedirectToAction("Manage",new { @id=client.ClientId });
            }

            return View(client);
        }

        [HttpPost]
        public ActionResult AddNote()
        {
            int id = Convert.ToInt16(Request.Form["ClientId"]);
            string Content = Request.Form["Content"];

            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }

            ClientNote cn = new ClientNote();
            cn.ClientId = id;
            cn.Note = Content;
            cn.DateCreated = DateTime.UtcNow.AddHours(8);
            cn.Done = 0;
            cn.CreatedByUserId = User.Identity.GetUserId();

            db.ClientNotes.Add(cn);
            db.SaveChanges();

            return RedirectToAction("Manage", "Clients", new { @id = id});
        }

        public ActionResult MarkDone(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientNote cn = db.ClientNotes.Find(id);
            if (cn == null)
            {
                return HttpNotFound();
            }

            cn.Done = 1;
            db.SaveChanges();

            return RedirectToAction("Manage","Clients",new {  @id = cn.ClientId });
        }

        public ActionResult RemoveNote(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientNote cn = db.ClientNotes.Find(id);
            if (cn == null)
            {
                return HttpNotFound();
            }

            db.ClientNotes.Remove(cn);
            db.SaveChanges();

            return RedirectToAction("Manage", "Clients", new { @id = cn.ClientId });
        }

        // GET: Clients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "Name", client.CityId);
            //ViewBag.CreatedByUserId = new SelectList(db.Users, "Id", "Email", client.CreatedByUserId);
            ViewBag.ClientId = new SelectList(db.Pwds, "PwdId", "Status", client.ClientId);
            ViewBag.ClientId = new SelectList(db.SeniorCitizens, "SeniorCitizenId", "Status", client.ClientId);
            ViewBag.ClientId = new SelectList(db.SoloParents, "SoloParentId", "Status", client.ClientId);
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClientId,GivenName,MiddleName,SurName,Gender,CivilStatus,Occupation,Citizenship,CityId,CityAddress,ProvincialAddress,ContactNumber,TypeOfResidency,StartOfResidency,BirthDate,BirthPlace,Religion,DateOfMarriage,PlaceOfMarriage,SpouseName,SpouseBirthDate,CreatedByUserId,DateCreated")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "Name", client.CityId);
            //ViewBag.CreatedByUserId = new SelectList(db.Users, "Id", "Email", client.CreatedByUserId);
            ViewBag.ClientId = new SelectList(db.Pwds, "PwdId", "Status", client.ClientId);
            ViewBag.ClientId = new SelectList(db.SeniorCitizens, "SeniorCitizenId", "Status", client.ClientId);
            ViewBag.ClientId = new SelectList(db.SoloParents, "SoloParentId", "Status", client.ClientId);
            return View(client);
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = db.Clients.Find(id);
            db.Clients.Remove(client);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Apply()
        {
            string email = User.Identity.GetUserName();
            ApplicationUser au = db.Users.FirstOrDefault(u => u.UserName == email);
            return View(au);
        }

        [Route("SeniorCitizen/Apply")]
        public ActionResult ApplySC()
        {
            
            string username = User.Identity.GetUserName();
            ApplicationUser au = db.Users.FirstOrDefault(u => u.UserName == username);

            if (au.Client != null)
            {
                return RedirectToAction("Apply");
            }

            ClientEditModel ce = new ClientEditModel();
            ce.GivenName = au.GivenName;
            ce.MiddleName = au.MiddleName;
            ce.SurName = au.LastName;

            return View(ce);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("SeniorCitizen/Apply")]
        public ActionResult ApplySC(ClientEditModel ce)
        {
            Client newClient = new Client(ce);

            ClaimsIdentity identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            newClient.CityId = Convert.ToInt16(claims.FirstOrDefault(c=>c.Type=="CityId").Value);
            newClient.DateCreated = DateTime.UtcNow.AddHours(8);

            db.Clients.Add(newClient);

            SeniorCitizen newSC = new SeniorCitizen();
            newSC.Status = "Pending";
            newSC.Client = newClient;
            newSC.ApplicationDate = newClient.DateCreated;

            db.SeniorCitizens.Add(newSC);

            if (ce.ClientBeneficiaries != null)
            {
                ce.ClientBeneficiaries.ForEach(c => c.ClientId = newClient.ClientId);
                ce.ClientBeneficiaries.ForEach(c => db.ClientBeneficiary.Add(c));
            }

            string email = User.Identity.GetUserName();
            ApplicationUser au = db.Users.FirstOrDefault(u => u.UserName == email);

            au.ClientId = newClient.ClientId;

            List<Requirement> rl = new List<Requirement>{
                    new Requirement { ClientId = newClient.ClientId, IsDone = false, Name = "Senior Citizen Recent Photo", Description = "Upload a recent picture of yourself" },
                    new Requirement { ClientId = newClient.ClientId, IsDone = false, Name = "Senior Citizen Supporting Document", Description = "Any of the following; Driver's License, Voter’s ID, NBI Clearance, Old Residence Certificate, Police Clearance" },
            };

            db.Requirements.AddRange(rl);

            if (db.SaveChanges() > 1)
            {
                return RedirectToAction("Requirements", "Clients", null);
            }
            else
            {
                return View(ce);
            }
        }

        [Route("Pwd/Apply")]
        public ActionResult ApplyPwd()
        {
            string username = User.Identity.GetUserName();
            ApplicationUser au = db.Users.FirstOrDefault(u => u.UserName == username);

            ClientPWDEditModel cpwde = new ClientPWDEditModel();
            cpwde.GivenName = au.GivenName;
            cpwde.MiddleName = au.MiddleName;
            cpwde.SurName = au.LastName;

            return View(cpwde);
        }

        [Route("Pwd/Apply")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApplyPwd(ClientPWDEditModel cpwde)
        {
            string username = User.Identity.GetUserName();
            ApplicationUser au = db.Users.FirstOrDefault(u => u.UserName == username);

            if (au.Client != null)
            {
                // if existing client

            }
            else
            {
                // if new client
                Client newClient = new Client(cpwde);

                ClaimsIdentity identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;

                newClient.CityId = Convert.ToInt16(claims.FirstOrDefault(c => c.Type == "CityId").Value);
                newClient.DateCreated = DateTime.UtcNow.AddHours(8);

                db.Clients.Add(newClient);

                Pwd newPWD = new Pwd();
                newPWD.Status = "Pending";
                newPWD.Client = newClient;
                newPWD.ApplicationDate = newClient.DateCreated;

                db.Pwds.Add(newPWD);

                if (cpwde.ClientBeneficiaries != null)
                {
                    cpwde.ClientBeneficiaries.ForEach(c => c.ClientId = newClient.ClientId);
                    cpwde.ClientBeneficiaries.ForEach(c => db.ClientBeneficiary.Add(c));
                }
                
                au.ClientId = newClient.ClientId;

                List<Requirement> rl = new List<Requirement>{
                    new Requirement { ClientId = newClient.ClientId, IsDone = false, Name = "PWD Baranggay Certificate", Description = "Baranggay Certificate" },
                    new Requirement { ClientId = newClient.ClientId, IsDone = false, Name = "PWD Medical Certificate", Description = "Medical Certificate or abstract from Physician" },
                    new Requirement { ClientId = newClient.ClientId, IsDone = false, Name = "PWD Recent Photo", Description = "Upload a recent picture of yourself" },
                    new Requirement { ClientId = newClient.ClientId, IsDone = false, Name = "PWD Authorization Letter", Description = "Disregard if applying by yourself" },
                };

                db.Requirements.AddRange(rl);

                if (db.SaveChanges() > 1)
                {
                    return RedirectToAction("Requirements", "Clients", null);
                }
                else
                {
                    return View(cpwde);
                }
            }

            return View(cpwde);
        }

        [Route("SoloParent/Apply")]
        public ActionResult ApplySP()
        {
            string username = User.Identity.GetUserName();
            ApplicationUser au = db.Users.FirstOrDefault(u => u.UserName == username);

            ClientEditModel ce = new ClientEditModel();
            ce.GivenName = au.GivenName;
            ce.MiddleName = au.MiddleName;
            ce.SurName = au.LastName;

            return View(ce);
        }

        /*
            Client newClient = new Client(ce);

            ClaimsIdentity identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            newClient.CityId = Convert.ToInt16(claims.FirstOrDefault(c=>c.Type=="CityId").Value);
            newClient.DateCreated = DateTime.UtcNow.AddHours(8);

            db.Clients.Add(newClient);

            SeniorCitizen newSC = new SeniorCitizen();
            newSC.Status = "Pending";
            newSC.Client = newClient;
            newSC.ApplicationDate = newClient.DateCreated;

            db.SeniorCitizens.Add(newSC);

            if (ce.ClientBeneficiaries != null)
            {
                ce.ClientBeneficiaries.ForEach(c => c.ClientId = newClient.ClientId);
                ce.ClientBeneficiaries.ForEach(c => db.ClientBeneficiary.Add(c));
            } 

            string email = User.Identity.GetUserName();
            ApplicationUser au = db.Users.FirstOrDefault(u => u.UserName == email);

            au.ClientId = newClient.ClientId;

            List<Requirement> rl = new List<Requirement>{
                    new Requirement { ClientId = newClient.ClientId, IsDone = false, Name = "Senior Citizen Recent Photo", Description = "Upload a recent picture of yourself" },
                    new Requirement { ClientId = newClient.ClientId, IsDone = false, Name = "Senior Citizen Supporting Document", Description = "Any of the following; Driver's License, Voter’s ID, NBI Clearance, Old Residence Certificate, Police Clearance" },
            };

            db.Requirements.AddRange(rl);
        */

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("SoloParent/Apply")]
        public ActionResult ApplySP(ClientEditModel ce)
        {
            Client newClient = new Client(ce);

            ClaimsIdentity identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            newClient.CityId = Convert.ToInt16(claims.FirstOrDefault(c => c.Type == "CityId").Value);
            newClient.DateCreated = DateTime.UtcNow.AddHours(8);

            db.Clients.Add(newClient);

            SoloParent newSC = new SoloParent();
            newSC.Status = "Pending";
            newSC.Client = newClient;
            newSC.ApplicationDate = newClient.DateCreated;

            db.SoloParents.Add(newSC);

            if (ce.ClientBeneficiaries != null)
            {
                ce.ClientBeneficiaries.ForEach(c => c.ClientId = newClient.ClientId);
                ce.ClientBeneficiaries.ForEach(c => db.ClientBeneficiary.Add(c));
            }

            string email = User.Identity.GetUserName();
            ApplicationUser au = db.Users.FirstOrDefault(u => u.UserName == email);

            au.ClientId = newClient.ClientId;

            List<Requirement> rl = new List<Requirement>{
                new Requirement { ClientId = newClient.ClientId, IsDone = false, Name = "Solo Parent Baranggay Certificate", Description = "Baranggay Certificate" },
                new Requirement { ClientId = newClient.ClientId, IsDone = false, Name = "Solo Parent Proof of Financial Status", Description = "Payslip, Bank Transactions" },
                new Requirement { ClientId = newClient.ClientId, IsDone = false, Name = "Solo Parent Supporting Document", Description = "Nullity of Marriage, Death Certificate" },
                new Requirement { ClientId = newClient.ClientId, IsDone = false, Name = "Solo Parent Birth Certificate (Children)", Description = "Birth Certificate of your child/children" },
           };

            db.Requirements.AddRange(rl);

            if (db.SaveChanges() > 1)
            {
                return RedirectToAction("Requirements", "Clients", null);
            }
            else
            {
                return View(ce);
            }
        }

        public ActionResult Requirements(int? id)
        {
            if (id != null)
            {
                Client c = db.Clients.FirstOrDefault(l => l.ClientId == id);

                if (c == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    List<Requirement> rl = c.Requirements.ToList();
                    return View(rl);
                }
            }
            else
            {
                string email = User.Identity.GetUserName();
                ApplicationUser au = db.Users.FirstOrDefault(u => u.UserName == email);

                ViewBag.clientName = au.getFullName();

                if (au.Client == null)
                {
                    List<Requirement> rl = new List<Requirement>();
                    return View(rl);
                }
                else
                {
                    List<Requirement> rl = au.Client.Requirements.ToList();
                    return View(rl);
                }
            }
        }

        public ActionResult Reports(int? id)
        {
            var clients = db.Clients.ToList();
            var inquiries = db.Inquiries.ToList();
            var scl = db.SeniorCitizens.ToList();
            var spl = db.SoloParents.ToList();
            var pwdl = db.Pwds.ToList();

            if (id != null)
            {

            }
            else
            {
                ViewBag.janCount = clients.Where(c => c.DateCreated.Month == 1 && c.DateCreated.Year == 2018).Count();
                ViewBag.febCount = clients.Where(c => c.DateCreated.Month == 2 && c.DateCreated.Year == 2018).Count();
                ViewBag.marCount = clients.Where(c => c.DateCreated.Month == 3 && c.DateCreated.Year == 2018).Count();
                ViewBag.aprCount = clients.Where(c => c.DateCreated.Month == 4 && c.DateCreated.Year == 2018).Count();
                ViewBag.mayCount = clients.Where(c => c.DateCreated.Month == 5 && c.DateCreated.Year == 2018).Count();
                ViewBag.junCount = clients.Where(c => c.DateCreated.Month == 6 && c.DateCreated.Year == 2018).Count();
                ViewBag.julCount = clients.Where(c => c.DateCreated.Month == 7 && c.DateCreated.Year == 2018).Count();
                ViewBag.augCount = clients.Where(c => c.DateCreated.Month == 8 && c.DateCreated.Year == 2018).Count();
                ViewBag.septCount = clients.Where(c => c.DateCreated.Month == 9 && c.DateCreated.Year == 2018).Count();
                ViewBag.octCount = clients.Where(c => c.DateCreated.Month == 10 && c.DateCreated.Year == 2018).Count();
                ViewBag.novCount = clients.Where(c => c.DateCreated.Month == 11 && c.DateCreated.Year == 2018).Count();
                ViewBag.decCount = clients.Where(c => c.DateCreated.Month == 12 && c.DateCreated.Year == 2018).Count();

                ViewBag.totalCount = clients.Count();

                ViewBag.scCount = clients.Where(c => c.SeniorCitizen != null).Count();
                ViewBag.spCount = clients.Where(c => c.SoloParent != null).Count();
                ViewBag.pwdCount = clients.Where(c => c.Pwd != null).Count();

                ViewBag.janICount = inquiries.Where(c => c.DateCreated.Month == 1 && c.DateCreated.Year == 2018).Count();
                ViewBag.febICount = inquiries.Where(c => c.DateCreated.Month == 2 && c.DateCreated.Year == 2018).Count();
                ViewBag.marICount = inquiries.Where(c => c.DateCreated.Month == 3 && c.DateCreated.Year == 2018).Count();
                ViewBag.aprICount = inquiries.Where(c => c.DateCreated.Month == 4 && c.DateCreated.Year == 2018).Count();
                ViewBag.mayICount = inquiries.Where(c => c.DateCreated.Month == 5 && c.DateCreated.Year == 2018).Count();
                ViewBag.junICount = inquiries.Where(c => c.DateCreated.Month == 6 && c.DateCreated.Year == 2018).Count();
                ViewBag.julICount = inquiries.Where(c => c.DateCreated.Month == 7 && c.DateCreated.Year == 2018).Count();
                ViewBag.augICount = inquiries.Where(c => c.DateCreated.Month == 8 && c.DateCreated.Year == 2018).Count();
                ViewBag.septICount = inquiries.Where(c => c.DateCreated.Month == 9 && c.DateCreated.Year == 2018).Count();
                ViewBag.octICount = inquiries.Where(c => c.DateCreated.Month == 10 && c.DateCreated.Year == 2018).Count();
                ViewBag.novICount = inquiries.Where(c => c.DateCreated.Month == 11 && c.DateCreated.Year == 2018).Count();
                ViewBag.decICount = inquiries.Where(c => c.DateCreated.Month == 12 && c.DateCreated.Year == 2018).Count();

                ViewBag.totalICount = inquiries.Count();

                ViewBag.scPending = scl.Where(c => c.Status == "Pending").Count();
                ViewBag.scInProgress = scl.Where(c => c.Status != "Pending" && c.Status != "Approved").Count();
                ViewBag.scApproved = scl.Where(c => c.Status == "Approved").Count();

                ViewBag.spPending = spl.Where(c => c.Status == "Pending").Count();
                ViewBag.spInProgress = spl.Where(c => c.Status != "Pending" && c.Status != "Approved").Count();
                ViewBag.spApproved = spl.Where(c => c.Status == "Approved").Count();

                ViewBag.pwdPending = pwdl.Where(c => c.Status == "Pending").Count();
                ViewBag.pwdInProgress = pwdl.Where(c => c.Status != "Pending" && c.Status != "Approved").Count();
                ViewBag.pwdApproved = pwdl.Where(c => c.Status == "Approved").Count();

                ViewBag.questionCount = inquiries.Where(c => c.Category == "Question").Count();
                ViewBag.requirementCount = inquiries.Where(c => c.Category == "Requirement").Count();
            }


            return View();
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
