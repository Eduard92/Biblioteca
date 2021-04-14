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
    public class WSAlumnosController : ApiController
    {
        private rekursosEntities db = new rekursosEntities();
         
        // GET api/Default1
         [BasicAuthentication]
        public IEnumerable<alumnos> Getalumnos(string escuela="")
        {
            return db.alumnos.AsEnumerable();
        }

        // GET api/Default1/5
        //[BasicAuthentication]
        public alumnos Getalumnos(int id)
        {
            alumnos alumnos = db.alumnos.Find(id);
            if (alumnos == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return alumnos;
        }

        // PUT api/Default1/5
        public HttpResponseMessage Putalumnos(int id, alumnos alumnos)
        {
            if (ModelState.IsValid && id == alumnos.id)
            {
                db.Entry(alumnos).State = EntityState.Modified;

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

        // POST api/Default1
        public HttpResponseMessage Postalumnos(alumnos alumnos)
        {
            if (ModelState.IsValid)
            {
                db.alumnos.Add(alumnos);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, alumnos);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = alumnos.id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Default1/5
        public HttpResponseMessage Deletealumnos(int id)
        {
            alumnos alumnos = db.alumnos.Find(id);
            if (alumnos == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.alumnos.Remove(alumnos);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, alumnos);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}