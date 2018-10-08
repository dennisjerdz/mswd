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

namespace MSWD.Controllers
{
    [Authorize(Roles = "Social Worker,OIC")]
    public class ClientsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Clients
        public ActionResult Index()
        {
            var clients = db.Clients.Include(c => c.City).Include(c => c.CreatedBy).Include(c => c.Pwd).Include(c => c.SeniorCitizen).Include(c => c.SoloParent);
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
            ViewBag.CreatedByUserId = new SelectList(db.Users, "Id", "Email", client.CreatedByUserId);
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
            ViewBag.CreatedByUserId = new SelectList(db.Users, "Id", "Email", client.CreatedByUserId);
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
            ViewBag.CreatedByUserId = new SelectList(db.Users, "Id", "Email", client.CreatedByUserId);
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
