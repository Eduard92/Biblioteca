using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RK.Core;
using DataLayer;
using RK.Security;

namespace RK.Controllers
{
    public class GroupsController : AdminController
    {
        private rekursosEntities db = new rekursosEntities();
        public GroupsController()
        {
            List<ShortCuts> short_cuts = new List<ShortCuts>();

            short_cuts.Add(new ShortCuts { name = "Nuevo grupo", uri = "groups/create", icon = "ion-plus-round" });

            ViewBag.ShortCuts = short_cuts;
        }
        //
        // GET: /Groups/

        public ActionResult Index()
        {
            return View(db.groups.ToList());
        }

        //
        // GET: /Groups/Details/5

        public ActionResult Details(int id = 0)
        {
            groups groups = db.groups.Find(id);
            if (groups == null)
            {
                return HttpNotFound();
            }
            return View(groups);
        }

        //
        // GET: /Groups/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Groups/Create

        [HttpPost]
        public ActionResult Create(groups groups)
        {
            if (ModelState.IsValid)
            {
                db.groups.Add(groups);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(groups);
        }

        //
        // GET: /Groups/Edit/5

        public ActionResult Edit(int id = 0)
        {
            groups groups = db.groups.Find(id);
            if (groups == null)
            {
                return HttpNotFound();
            }
            return View(groups);
        }

        //
        // POST: /Groups/Edit/5

        [HttpPost]
        public ActionResult Edit(groups groups)
        {
            if (ModelState.IsValid)
            {
                db.Entry(groups).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(groups);
        }

        //
        // GET: /Groups/Delete/5

        public ActionResult Delete(int id = 0)
        {
            groups groups = db.groups.Find(id);
            if (groups == null)
            {
                return HttpNotFound();
            }
            return View(groups);
        }

        //
        // POST: /Groups/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            groups groups = db.groups.Find(id);
            db.groups.Remove(groups);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}