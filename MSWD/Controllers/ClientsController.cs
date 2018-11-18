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

namespace MSWD.Controllers
{
    [Authorize(Roles = "Social Worker,OIC,Client")]
    public class ClientsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

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

            if (au.Client.SeniorCitizen == null)
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

            db.SeniorCitizens.Add(newSC);

            ce.ClientBeneficiaries.ForEach(c => c.ClientId = newClient.ClientId);
            ce.ClientBeneficiaries.ForEach(c => db.ClientBeneficiary.Add(c));

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

            ClientEditModel ce = new ClientEditModel();
            ce.GivenName = au.GivenName;
            ce.MiddleName = au.MiddleName;
            ce.SurName = au.LastName;

            return View(ce);
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

        public ActionResult Requirements(int? clientID)
        {
            if (clientID != null)
            {
                Client c = db.Clients.FirstOrDefault(l => l.ClientId == clientID);

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

                List<Requirement> rl = au.Client.Requirements.ToList();
                return View(rl);
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
