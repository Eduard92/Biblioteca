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

namespace RK.Api
{
    public class WSLocalidadesController : ApiController
    {
        private rekursosEntities db = new rekursosEntities();

        // GET api/Default1
        public IEnumerable<string> Get()
        {
            List<string> localidades= new List<string>();
            var result = db.cat_localidades.GroupBy(g => g.d_estado).Select(s => new{ estado=s.Key }).OrderBy(o=>o.estado).ToList();

            foreach (var item in result)
            {
                localidades.Add(item.estado);
            }


           return localidades;

            

            
        }

        // GET api/Default1/5
        public IEnumerable<cat_localidades> Getcat_localidades(string id,string d_asenta="")
        {
            var cat_localidades = db.cat_localidades.Where(w=>w.d_estado==id).AsEnumerable();

            if (d_asenta != "")
            {
                cat_localidades = cat_localidades.Where(w=>w.d_asenta==d_asenta);
            }
            if (cat_localidades == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return cat_localidades;
        }

        // PUT api/Default1/5
        public HttpResponseMessage Putcat_localidades(int id, cat_localidades cat_localidades)
        {
            if (ModelState.IsValid && id == cat_localidades.id)
            {
                db.Entry(cat_localidades).State = EntityState.Modified;

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
        public HttpResponseMessage Postcat_localidades(cat_localidades cat_localidades)
        {
            if (ModelState.IsValid)
            {
                db.cat_localidades.Add(cat_localidades);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, cat_localidades);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = cat_localidades.id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Default1/5
        public HttpResponseMessage Deletecat_localidades(int id)
        {
            cat_localidades cat_localidades = db.cat_localidades.Find(id);
            if (cat_localidades == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.cat_localidades.Remove(cat_localidades);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, cat_localidades);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}