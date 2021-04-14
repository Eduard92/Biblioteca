using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using RK.Models;
using RK.Security;
using RK.Core;
using Excel;
using System.IO;
using RK.Libraries;

using PagedList;

namespace RK.Controllers
{
    public class EmpleadosController :AdminController
    {
        private rekursosEntities db = new rekursosEntities();
       
        //
        // GET: /Empleados/
        public EmpleadosController():base("empleados")
        {
            List<ShortCuts> short_cuts = new List<ShortCuts>();
            string[] scripts = new string[]{
               
                "empleados.controller.js",
                //"camera.js"
                
                

            };
            short_cuts.Add(new ShortCuts { name = "Agregar empleado", uri = "empleados/create", icon = "ion-plus-round" });
            short_cuts.Add(new ShortCuts { name = "Importar", uri = "empleados/upload", icon = "ion-upload" });
            ViewBag.Scripts = scripts;
            ViewBag.ShortCuts = short_cuts;
        }
       
        public ActionResult Index(int? id_centro=0,string q="",int page=1)
        {
            ViewBag.id_centro = new SelectList(db.centros, "id", "nombre");
            var empleados = db.vempleados.OrderBy(o=>o.id_empleado).AsEnumerable();//db.vempleados.Include(e => e.cat_areas);

            if (q != "")
            {
                empleados = empleados.Where(w=>w.empleado.Contains(q));
            }
            if (id_centro != 0)
            {
                empleados = empleados.Where(w => w.id_centro == id_centro);

                ViewBag.IdCentro = id_centro;
            }

            ViewBag.TotalRows = empleados.Count();
            return View(empleados.ToPagedList(page,Int32.Parse(Settings.Get("records_per_page"))));
        }

        //
        // GET: /Empleados/Details/5

        public ActionResult Details(int id = 0,string id_empleado="")
        {
            empleados empleado = db.empleados.Find(id,id_empleado);

            EmpleadoModel empleado_m = new EmpleadoModel();
            if (empleado == null)
            {
                return HttpNotFound();
            }
            empleado_m.id = empleado.id;
            empleado_m.id_empleado = empleado.id_empleado;
            empleado_m.nombre = empleado.nombre;
            empleado_m.apellido_paterno = empleado.apellido_paterno;
            empleado_m.apellido_materno = empleado.apellido_materno;
            empleado_m.fecha_nacimiento = (DateTime)empleado.fecha_nacimiento.GetValueOrDefault();
            empleado_m.email = empleado.email;
            empleado_m.direccion = empleado.direccion;
            empleado_m.telefono = empleado.telefono;
            empleado_m.rfc = empleado.rfc;
            empleado_m.curp = empleado.curp;
            empleado_m.activo = (int)empleado.activo;

            empleado_m.id_centro = (int)empleado.id_centro.GetValueOrDefault();
            empleado_m.id_area = (int)empleado.id_area.GetValueOrDefault();
            
            empleado_m.tipo_mando = empleado.tipo_mando;
            empleado_m.cargo = empleado.cargo;
            empleado_m.fotografia = empleado.fotografia;

            ViewBag.centro = db.centros.Find(empleado.id_centro).nombre;
            var area = db.cat_areas.Find(empleado.id_area,empleado.id_centro);
            ViewBag.area = area == null ? null : area.nombre;

            return View(empleado_m);
        }

        //
        // GET: /Empleados/Create

        public ActionResult Create()
        {
            ViewBag.id_area = new SelectList(db.cat_areas, "id", "nombre");
            ViewBag.id_centro = new SelectList(db.centros, "id", "nombre");
            ViewBag.tipo_mando = new SelectList(new List<SelectListItem>{
                 //new SelectListItem { Text="[Seleccione]",Value="" },
                new SelectListItem { Text="Operativo",Value="operativo" },
                new SelectListItem { Text="Medio",Value="medio"},
                new SelectListItem { Text="Superior",Value="superior"},},"Value","Text");
            //EmpleadoModel empleado_m = new EmpleadoModel();
            EmpleadoModel empleado_m = new EmpleadoModel();
            return View(empleado_m);
        }

        //
        // POST: /Empleados/Create

        [HttpPost]
        public ActionResult Create(EmpleadoModel empleado_m)
        {
            if (ModelState.IsValid)
            {
                empleados empleado = new empleados();

               // empleado.id = empleado_m.no_empleado;
                empleado.id_empleado = empleado_m.id_empleado;
                empleado.nombre = empleado_m.nombre;
                empleado.apellido_paterno = empleado_m.apellido_paterno;
                empleado.apellido_materno = empleado_m.apellido_materno;
                empleado.fecha_nacimiento = empleado_m.fecha_nacimiento;
                empleado.id_area = null;//empleado_m.id_area;
                empleado.id_centro = empleado_m.id_centro;
                empleado.cargo = empleado_m.cargo;
                empleado.rfc = empleado_m.rfc;
                empleado.curp = empleado_m.curp;
                empleado.direccion = empleado_m.direccion;
                empleado.telefono = empleado_m.telefono;
                empleado.email = empleado_m.email;
                empleado.activo = 1;

                db.empleados.Add(empleado);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.id_area = new SelectList(db.cat_areas, "id", "nombre", empleados.id_area);

            ViewBag.id_area = new SelectList(db.cat_areas, "id", "nombre");
            ViewBag.id_centro = new SelectList(db.centros, "id", "nombre");
            ViewBag.tipo_mando = new SelectList(new List<SelectListItem>{
                 //new SelectListItem { Text="[Seleccione]",Value="" },
                new SelectListItem { Text="Operativo",Value="operativo" },
                new SelectListItem { Text="Medio",Value="medio"},
                new SelectListItem { Text="Superior",Value="superior"},}, "Value", "Text");
            return View(empleado_m);
        }

        //
        // GET: /Empleados/Edit/5

        public ActionResult Edit(int id = 0)
        {
            empleados empleado = db.empleados.Where(w=>w.id== id).SingleOrDefault();
            if (empleado == null)
            {
                return HttpNotFound();
            }
            EmpleadoModel empleado_m = new EmpleadoModel();
            //Insercion de datos
            empleado_m.id = empleado.id;
            empleado_m.id_empleado = empleado.id_empleado;
            empleado_m.nombre = empleado.nombre;
            empleado_m.apellido_paterno = empleado.apellido_paterno;
            empleado_m.apellido_materno = empleado.apellido_materno;
            empleado_m.fecha_nacimiento = (DateTime)empleado.fecha_nacimiento.GetValueOrDefault();
            empleado_m.email = empleado.email;
            empleado_m.direccion = empleado.direccion;
            empleado_m.telefono = empleado.telefono;
            empleado_m.rfc = empleado.rfc;
            empleado_m.curp = empleado.curp;
            empleado_m.activo = (int)empleado.activo;

            empleado_m.id_centro = (int)empleado.id_centro.GetValueOrDefault();
            empleado_m.id_area = (int)empleado.id_area.GetValueOrDefault();
            empleado_m.tipo_mando = empleado.tipo_mando;
            empleado_m.cargo = empleado.cargo;
            empleado_m.fotografia = empleado.fotografia;

            ViewBag.id_area = new SelectList(db.cat_areas, "id", "nombre", empleado.id_area);
            ViewBag.id_centro = new SelectList(db.centros, "id", "nombre");
            ViewBag.tipo_mando = new SelectList(new List<SelectListItem>{
                 //new SelectListItem { Text="[Seleccione]",Value="" },
                new SelectListItem { Text="Operativo",Value="operativo" },
                new SelectListItem { Text="Medio",Value="medio"},
                new SelectListItem { Text="Superior",Value="superior"},}, "Value", "Text",empleado_m.tipo_mando);
            return View(empleado_m);
        }

        //
        // POST: /Empleados/Edit/5

        [HttpPost]
        public ActionResult Edit(EmpleadoModel empleado_m)
        {
            if (ModelState.IsValid)
            {
                empleados empleado = new empleados();

                empleado.id = empleado_m.id;
                empleado.id_empleado = empleado_m.id_empleado;
                empleado.nombre = empleado_m.nombre;
                empleado.apellido_paterno = empleado_m.apellido_paterno;
                empleado.apellido_materno = empleado_m.apellido_materno;
                empleado.fecha_nacimiento = empleado_m.fecha_nacimiento;
                empleado.id_area = empleado_m.id_area;
                empleado.id_centro = empleado_m.id_centro;
                empleado.cargo = empleado_m.cargo;
                empleado.tipo_mando = empleado_m.tipo_mando;
                empleado.rfc = empleado_m.rfc;
                empleado.curp = empleado_m.curp;
                empleado.direccion = empleado_m.direccion;
                empleado.telefono = empleado_m.telefono;
                empleado.email = empleado_m.email;
                empleado.activo = empleado_m.activo;
                empleado.fotografia = empleado_m.fotografia;

                HttpPostedFileBase file = Request.Files["fotografia"];
                

                var result_camera = Libraries.File.SaveCamera(empleado_m.camera,1);

                if (result_camera.status)
                {
                    empleado.fotografia = result_camera.data.id;
                }
                if (file != null && file.ContentLength > 0)
                {
                    //Response.Write(file.ContentLength);
                    var result = Libraries.File.upload(file, 1);
                   
                    if (result.status)
                    {
                        empleado.fotografia = result.data.id;
                    }
                }


                db.Entry(empleado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_area = new SelectList(db.cat_areas, "id", "nombre");
            ViewBag.id_centro = new SelectList(db.centros, "id", "nombre");
            ViewBag.tipo_mando = new SelectList(new List<SelectListItem>{
                 //new SelectListItem { Text="[Seleccione]",Value="" },
                new SelectListItem { Text="Operativo",Value="operativo" },
                new SelectListItem { Text="Medio",Value="medio"},
                new SelectListItem { Text="Superior",Value="superior"},}, "Value", "Text");
            return View(empleado_m);
        }

        //
        // GET: /Empleados/Delete/5

        public ActionResult Delete(int id = 0,string id_empleado="")
        {
            empleados empleado = db.empleados.Find(id,id_empleado);
            EmpleadoModel empleado_m = new EmpleadoModel();

            if (empleado == null)
            {
                return HttpNotFound();
            }

            empleado_m.id = empleado.id;
            empleado_m.id_empleado = empleado.id_empleado;
            empleado_m.nombre = empleado.nombre;
            empleado_m.apellido_paterno = empleado.apellido_paterno;
            empleado_m.apellido_materno = empleado.apellido_materno;
            empleado_m.fecha_nacimiento = (DateTime)empleado.fecha_nacimiento.GetValueOrDefault();
            empleado_m.email = empleado.email;
            empleado_m.direccion = empleado.direccion;
            empleado_m.telefono = empleado.telefono;
            empleado_m.rfc = empleado.rfc;
            empleado_m.curp = empleado.curp;
            empleado_m.activo = (int)empleado.activo;

            empleado_m.id_centro = (int)empleado.id_centro.GetValueOrDefault();
            empleado_m.id_area = (int)empleado.id_area.GetValueOrDefault();

            empleado_m.tipo_mando = empleado.tipo_mando;
            empleado_m.cargo = empleado.cargo;
            empleado_m.fotografia = empleado.fotografia;
            return View(empleado_m);
        }

        //
        // POST: /Empleados/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id,string id_empleado="")
        {
            empleados empleados = db.empleados.Find(id,id_empleado);
            db.empleados.Remove(empleados);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(IEnumerable<empleados> empleados)
        {

           

            var all = db.empleados.Where(w => w.activo == 1).ToList();

            all.ForEach(w=>w.activo=0);
            db.SaveChanges();

            int adds = 0;
            int updates = 0;
            List<string> errors = new List<string>();
            //return Json(empleados);
            foreach (empleados empleado in empleados)
            {
                var exists = db.empleados.Where(w=>w.id_empleado== empleado.id_empleado).SingleOrDefault();

                if (exists != null)
                {
                    exists.activo = 1;
                    exists.fecha_nacimiento = empleado.fecha_nacimiento;
                    exists.nombre = empleado.nombre;
                    exists.apellido_paterno = empleado.apellido_paterno;
                    exists.apellido_materno = empleado.apellido_materno;
                    exists.cargo = empleado.cargo;
                    exists.email = empleado.email;
                    exists.telefono = empleado.telefono;
                    exists.tipo_mando = empleado.tipo_mando;
                    exists.id_area = empleado.id_area == 0 ? null : empleado.id_area;
                    exists.id_centro = empleado.id_centro == 0 ? null : empleado.id_centro;
                    exists.rfc = empleado.rfc;
                    exists.curp = empleado.curp;
                    exists.direccion = empleado.direccion;
                    if (empleado.fotografia != null)
                    {
                        exists.fotografia = empleado.fotografia;
                    }
                    
                     try
                    {
                        //Response.Write(System.Web.Helpers.Json.Encode(empleado));
                        db.Entry(exists).State = EntityState.Modified;
                        db.SaveChanges();

                        updates++;
                    }
                     catch (Exception ex)
                     {
                         errors.Add(empleado.id_empleado);
                     }
                }
                else {
                    empleado.activo = 1;
                    try
                    {
                        empleado.id_area = empleado.id_area == 0 ? null : empleado.id_area;
                        empleado.id_centro = empleado.id_centro == 0 ? null : empleado.id_centro;
                        db.empleados.Add(empleado);
                        db.SaveChanges();

                        adds++;
                    }
                    catch(Exception ex)
                    {
                        errors.Add(empleado.id_empleado);
                       
                    }
                }
                
                
               
            }
            if (errors.Count > 0)
            {
                FlashData.SetFlashData("error", "Algunos registros no se agregaron: " + String.Join(",",errors));

               
            }
            //if (adds > 0)
            //{
                FlashData.SetFlashData("success", "Se han agregado " + adds.ToString() + " registros y actualizado " + updates.ToString());
            //}
               // return Content("");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult FileUpload(HttpPostedFileBase file)
        {
            //HttpPostedFileBase file = Request.Files["file"];
            
            //return Content(file.FileName);
            var upload = Libraries.File.upload(file,1);

            if (upload.status)
            {
                var centros = db.centros.ToDictionary(d => d.nombre, d => d.id);
                var areas   = db.cat_areas.ToDictionary(d =>d.id_centro+"_"+d.nombre, d => d.id);
                
                string path = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/")+upload.data.filename;
                List<string> array = new List<string>();
                var csv = Libraries.Csv.LoadCsvFile(path);


                if (csv.status==true)
                {
                    foreach (var item in csv.data)
                    {
                        if (item.ContainsKey("centro") && centros.ContainsKey(item["centro"]))
                        {
                            item["centro"] = centros[item["centro"]].ToString();
                        }
                        if (item.ContainsKey("centro") && item.ContainsKey("area") && areas.ContainsKey(item["centro"] + "_" + item["area"]))
                        {
                            item["area"] = areas[item["centro"]+"_"+item["area"]].ToString();
                        }
                       // Response.Write(System.Web.Helpers.Json.Encode(centros));
                    }

                }
               
                return Json(csv);
            }
            else
            {
                return Json(upload);
            }
        }
        public ActionResult ListAreas(int id_centro = 0)
        {
            Object result = new Object();
            bool estatus = false;
            Dictionary<int, string> list = new Dictionary<int, string>();


            var areas = db.cat_areas.Where(w => w.id_centro == id_centro).ToList();
            if (areas != null)
            {
                foreach (var area in areas)
                {
                    list.Add((int)area.id, area.nombre);
                }
                estatus = true;
            }
            result = new { estatus = estatus, message = "", list = list };


            return Json(list.ToArray(), JsonRequestBehavior.AllowGet);


        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}