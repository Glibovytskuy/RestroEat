using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Restro.Models;

namespace Restro.Controllers
{
    public class KitchenModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: KitchenModels
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            return View(db.Kitchens.ToList());
        }
        // GET: KitchenModels/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: KitchenModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Create([Bind(Include = "Id,Name")] KitchenModel kitchenModel)
        {
            if (ModelState.IsValid)
            {
                db.Kitchens.Add(kitchenModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(kitchenModel);
        }

        // GET: KitchenModels/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KitchenModel kitchenModel = db.Kitchens.Find(id);
            if (kitchenModel == null)
            {
                return HttpNotFound();
            }
            return View(kitchenModel);
        }

        // POST: KitchenModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Edit([Bind(Include = "Id,Name")] KitchenModel kitchenModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kitchenModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kitchenModel);
        }

        // GET: KitchenModels/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KitchenModel kitchenModel = db.Kitchens.Find(id);
            if (kitchenModel == null)
            {
                return HttpNotFound();
            }
            return View(kitchenModel);
        }

        // POST: KitchenModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            KitchenModel kitchenModel = db.Kitchens.Find(id);
            db.Kitchens.Remove(kitchenModel);
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
