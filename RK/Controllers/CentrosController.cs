using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using RK.Core;

namespace RK.Controllers
{
    public class CentrosController : AdminController
    {
        private rekursosEntities db = new rekursosEntities();

        public CentrosController():base("centros")
        {
        }
        //
        // GET: /Centros/

        public ActionResult Index()
        {
            return View(db.centros.ToList());
        }

        //
        // GET: /Centros/Details/5

        public ActionResult Details(int id = 0)
        {
            centros centros = db.centros.Find(id);
            if (centros == null)
            {
                return HttpNotFound();
            }
            return View(centros);
        }

        //
        // GET: /Centros/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Centros/Create

        [HttpPost]
        public ActionResult Create(centros centros)
        {
            if (ModelState.IsValid)
            {
                db.centros.Add(centros);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(centros);
        }

        //
        // GET: /Centros/Edit/5

        public ActionResult Edit(int id = 0)
        {
            centros centros = db.centros.Find(id);
            if (centros == null)
            {
                return HttpNotFound();
            }
            return View(centros);
        }

        //
        // POST: /Centros/Edit/5

        [HttpPost]
        public ActionResult Edit(centros centros)
        {
            if (ModelState.IsValid)
            {
                db.Entry(centros).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(centros);
        }

        //
        // GET: /Centros/Delete/5

        public ActionResult Delete(int id = 0)
        {
            centros centros = db.centros.Find(id);
            if (centros == null)
            {
                return HttpNotFound();
            }
            return View(centros);
        }

        //
        // POST: /Centros/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            centros centros = db.centros.Find(id);
            db.centros.Remove(centros);
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