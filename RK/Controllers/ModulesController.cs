using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace RK.Controllers
{
    public class ModulesController : Controller
    {
        private rekursosEntities db = new rekursosEntities();

        //
        // GET: /Modules/

        public ActionResult Index()
        {
            return View(db.modules.ToList());
        }

        //
        // GET: /Modules/Details/5

        public ActionResult Details(int id = 0)
        {
            modules modules = db.modules.Find(id);
            if (modules == null)
            {
                return HttpNotFound();
            }
            return View(modules);
        }

        //
        // GET: /Modules/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Modules/Create

        [HttpPost]
        public ActionResult Create(modules modules)
        {
            if (ModelState.IsValid)
            {
                db.modules.Add(modules);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(modules);
        }

        //
        // GET: /Modules/Edit/5

        public ActionResult Edit(int id = 0)
        {
            modules modules = db.modules.Find(id);
            if (modules == null)
            {
                return HttpNotFound();
            }
            return View(modules);
        }

        //
        // POST: /Modules/Edit/5

        [HttpPost]
        public ActionResult Edit(modules modules)
        {
            if (ModelState.IsValid)
            {
                db.Entry(modules).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(modules);
        }

        //
        // GET: /Modules/Delete/5

        public ActionResult Delete(int id = 0)
        {
            modules modules = db.modules.Find(id);
            if (modules == null)
            {
                return HttpNotFound();
            }
            return View(modules);
        }

        //
        // POST: /Modules/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            modules modules = db.modules.Find(id);
            db.modules.Remove(modules);
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