using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using RK.Security;
using RK.Core;
using DataLayer;
using PagedList;
using System.IO;

namespace RK.Controllers
{
    public class AlumnosController : AdminController
    {
        private rekursosEntities db = new rekursosEntities();
        private string path = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/");
        public AlumnosController():base("alumnos")
        {
            List<ShortCuts> short_cuts = new List<ShortCuts>();
            string[] scripts = new string[]{
               
                "alumnos.controller.js",
                
                
                

            };
            //short_cuts.Add(new ShortCuts { name = "Importar", uri = "alumnos/upload", icon = "ion-upload", extra = "open-modal", title = "Subir csv", controller = "InputUpload" });
            ViewBag.Scripts = scripts;
            ViewBag.ShortCuts = short_cuts;
        }
        //
        // GET: /Alumnos/

        public ActionResult Index(int? act,int page=1,string q="")
        {


            var items = db.valumnos.OrderBy(o=>o.nombre).AsEnumerable();

            if (q != "")
            {
                items = items.Where(w=>w.nombre.Contains(q) || w.matricula==q);
            }

            if (act != null)
            {
                items = items.Where(w => w.activo == act);
            }
            ViewBag.Total = items.Count();

            ViewBag.Page = page;
            ViewBag.q = q; 
            return View(items.ToPagedList((Int32)page, int.Parse(Libraries.Settings.Get("records_per_page"))));
        }

        //
        // GET: /Alumnos/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Alumnos/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Alumnos/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Alumnos/Edit/5

        public ActionResult Edit(int id,string id_alum)
        {
            alumnos alumno = db.alumnos.Find(id,id_alum);

            if (alumno == null)
            {
                return HttpNotFound();
            }
            return View(alumno);
        }

        //
        // POST: /Alumnos/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, alumnos alumno,HttpPostedFileBase fotografia)
        {
            try
            {
                if (fotografia != null)
                {
                    var result = Libraries.File.upload(fotografia, 1);

                    if (result.status)
                    {
                        alumno.fotografia = result.data.id;
                    }
                }
                db.Entry(alumno).State = EntityState.Modified;
                db.SaveChanges();
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Upload()
        {
            return View();
        }
        //
        // GET: /Alumnos/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Alumnos/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult FileUpload(HttpPostedFileBase file)
        {
            
            var upload = Libraries.File.upload(file, 1);
            Object result = new Object();
            int adds = 0;
            int updates = 0;
            if (upload.status)
            {
                

                
                List<string> array = new List<string>();
                var csv = Libraries.Csv.LoadCsvFile(path+upload.data.filename);

             

                if (csv.status==true)
                {
                  
                    var all = db.alumnos.Where(w => w.activo == 1).ToList();
                   
                        all.ForEach(w => w.activo = 0);
                        db.SaveChanges();
                        string message = "";

                        foreach (Dictionary<string, string> alumno in csv.data)
                        {




                            if (alumno["idalum"] != null)
                            {
                                string id_alum = alumno["idalum"];
                                alumnos exists = db.alumnos.Where(w => w.id_alum == id_alum).SingleOrDefault();


                                try
                                {

                                    if (exists != null)
                                    {
                                        exists.matricula = alumno["matricula"];
                                        exists.nombre = alumno["nombre"];
                                        exists.apellido_paterno = alumno["apellido_paterno"];
                                        exists.apellido_materno = alumno["apellido_materno"];
                                        exists.id_peri = alumno["idperi"];
                                        exists.curp = alumno["curp"];
                                        if (alumno["fechanaci"] != null)
                                            exists.fecha_naci = DateTime.Parse(alumno["fechanaci"]);
                                        if (alumno["sexo"] != null)
                                            exists.sexo = Int32.Parse(alumno["sexo"]);
                                        exists.escuela = alumno["escuela"];
                                        exists.grado = alumno["grado"];
                                        exists.grupo = alumno["grupo"];
                                        exists.dom_alum = alumno["domalum"];

                                        exists.tutor = alumno["tutor"];
                                        exists.dom_tutor = alumno["domtutor"];
                                        exists.activo = 1;

                                        db.Entry(exists).State = EntityState.Modified;
                                        db.SaveChanges();
                                        updates++;
                                    }
                                    else
                                    {
                                        alumnos data = new alumnos();
                                        data.id_alum = id_alum;
                                        data.matricula = alumno["matricula"];
                                        data.nombre = alumno["nombre"];
                                        data.apellido_paterno = alumno["apellido_paterno"];
                                        data.apellido_materno = alumno["apellido_materno"];
                                        data.id_peri = alumno["idperi"];
                                        data.curp = alumno["curp"];
                                        if (alumno["fechanaci"] != null)
                                            data.fecha_naci = DateTime.Parse(alumno["fechanaci"]);
                                        if (alumno["sexo"] != null)
                                            data.sexo = Int32.Parse(alumno["sexo"]);
                                        data.escuela = alumno["escuela"];
                                        data.grado = alumno["grado"];
                                        data.grupo = alumno["grupo"];
                                        data.dom_alum = alumno["domalum"];

                                        data.tutor = alumno["tutor"];
                                        data.dom_tutor = alumno["domtutor"];
                                        data.activo = 1;

                                        db.alumnos.Add(data);
                                        db.SaveChanges();

                                        adds++;
                                    }
                                }
                                catch(Exception ex)
                                {
                                   
                                    message += alumno["idalum"]+":"+ex.Message+"<br/>";
                                    
                                }
                            }
                        }
                        if (message == "")
                            result = new { status = true, message = "Se han agregado " + adds.ToString() + "  y actualizado " + updates.ToString() + " registros." };
                        else
                            result = new { status=false,message = message };
                    
                    
                }
                
               
                return Json(result);
            }
            else
            {
                return Json(upload);
            }
        }
    }
}
