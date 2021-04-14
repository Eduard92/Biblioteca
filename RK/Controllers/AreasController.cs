using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DataLayer;
using RK.Core;
using RK.Security;
using RK.Models;
using RK.Libraries;

namespace RK.Controllers
{
    public class AreasController : AdminController
    {
        private rekursosEntities db = new rekursosEntities();
        public AreasController():base("areas")
        {
            List<ShortCuts> short_cuts = new List<ShortCuts>();
            short_cuts.Add(new ShortCuts { name = "Agregar area", uri = "areas/create", icon = "ion-plus-round" });

            ViewBag.ShortCuts = short_cuts;
        }
        //
        // GET: /Areas/
      
        [AllowAnonymous]
        public ActionResult Index()
        {
            var areas = db.vareas.ToList();
            return View(areas);
        }

        //
        // GET: /Areas/Details/5

        public ActionResult Details(int id,int id_centro)
        {
            vareas area = db.vareas.Where(w => w.id_area == id && w.id_centro == id_centro).SingleOrDefault();


            if (area == null)
            {
                return HttpNotFound();
            }
            return View(area);
            
        }

        //
        // GET: /Areas/Create

        public ActionResult Create()
        {
            ViewBag.id_centro = new MultiSelectList(db.centros.ToList(), "id", "nombre");
            return View();
        }

        //
        // POST: /Areas/Create

        [HttpPost]
        public ActionResult Create(AreaModel area_m)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    cat_areas area = new cat_areas();

                    area.id_centro = area_m.id_centro;
                    area.nombre = area_m.nombre;

                    db.cat_areas.Add(area);
                    db.SaveChanges();

                     FlashData.SetFlashData("success","Registro agregado satisfactoriamente");
                    return RedirectToAction("Index");
                }
                return ViewBag(area_m);
               
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        //
        // GET: /Areas/Edit/5

        public ActionResult Edit(int id,int id_centro)
        {
            cat_areas area = db.cat_areas.Find(id,id_centro);


            if (area == null)
            {
                return HttpNotFound();
            }

            AreaModel area_m = new AreaModel();

            area_m.id_centro = area.id_centro;
            area_m.id = area.id;
            area_m.nombre = area.nombre;
            
            ViewBag.id_centro = new MultiSelectList(db.centros.ToList(), "id", "nombre");
            return View(area_m);
        }

        //
        // POST: /Areas/Edit/5

        [HttpPost]
        
        public ActionResult Edit(int id, AreaModel area_m)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    cat_areas area = new cat_areas();
                    area.id = area_m.id;
                    area.id_centro = area_m.id_centro;
                    area.nombre = area_m.nombre;

                    db.Entry(area).State = EntityState.Modified;
                    db.SaveChanges();
                    FlashData.SetFlashData("success", "Registro modificado satisfactoriamente");
                    return RedirectToAction("Index");
                }
                return ViewBag(area_m);

            }
            catch(Exception ex)
            {
                FlashData.SetFlashData("error", ex.Message);
                return RedirectToAction("Index");
            }
        }

        //
        // GET: /Areas/Delete/5

        public ActionResult Delete(int id,int id_centro)
        {
            vareas area = db.vareas.Where(w=>w.id_area==id && w.id_centro == id_centro).SingleOrDefault();


            if (area == null)
            {
                return HttpNotFound();
            }
            return View(area);
        }

        //
        // POST: /Areas/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id, int id_centro)
        {
            try
            {
                // TODO: Add delete logic here
                cat_areas area = db.cat_areas.Find(id,id_centro);

                db.cat_areas.Remove(area);
                db.SaveChanges();
                FlashData.SetFlashData("success", "Registro eliminado satisfactoriamente");
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                FlashData.SetFlashData("success", ex.Message);
                return RedirectToAction("Index");
            }
        }
    }
}
