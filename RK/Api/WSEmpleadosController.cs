using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using DataLayer;
using RK.Core;

namespace RK.Api
{
    public class WSEmpleadosController : ApiController
    {
        private rekursosEntities db = new rekursosEntities();

        public WSEmpleadosController()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }
        // GET api/WSEmpleados
       // [BasicAuthentication]
        public IEnumerable<empleados> Getempleados(int? id_centro=0)
        {
            var empleados = db.empleados.Where(w=>w.id_empleado!=null&& w.activo==1).AsEnumerable();

            if (id_centro != 0)
                empleados = empleados.Where(w => w.id_centro == id_centro).AsEnumerable();
            return empleados.OrderBy(o=>o.nombre).AsEnumerable();
        }

        // GET api/WSEmpleados/5
        public vempleados Getempleados(string id)
        {
            vempleados empleados = db.vempleados.Where(w=>w.id_empleado == id).SingleOrDefault();
            if (empleados == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return empleados;
        }

        // PUT api/WSEmpleados/5
        public HttpResponseMessage Putempleados(int id, empleados empleados)
        {
            if (ModelState.IsValid && id == empleados.id)
            {
                db.Entry(empleados).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/WSEmpleados
        public HttpResponseMessage Postempleados(empleados empleados)
        {
            if (ModelState.IsValid)
            {
                db.empleados.Add(empleados);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, empleados);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = empleados.id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/WSEmpleados/5
        public HttpResponseMessage Deleteempleados(int id)
        {
            empleados empleados = db.empleados.Find(id);
            if (empleados == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.empleados.Remove(empleados);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, empleados);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}