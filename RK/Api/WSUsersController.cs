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
using System.Web.Routing;

using DataLayer;
using RK.Core;
namespace RK.Api
{
    public class WSUsersController : ApiController
    {
        private rekursosEntities db = new rekursosEntities();
        public WSUsersController()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }
       
        
        // GET api/WSUsers
        // [BasicAuthentication]
        public IEnumerable<users> Getusers()
        {
            var users = db.users.Where(w => w.active == 1);
            return users.AsEnumerable();
        }

        // GET api/WSUsers/5
        // [BasicAuthentication]
        public vusers Getusers(string id)
        {

            int id_out = 0;

            int.TryParse(id, out  id_out);
            vusers users;
            if (id_out > 0)
            {
                users = db.vusers.Where(w => w.id == id_out).SingleOrDefault();
            }
            else
            {
                users = db.vusers.Where(w => w.username == id).SingleOrDefault();
            }
            
            
            
            if (users == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return users;
        }

        // PUT api/WSUsers/5
        public HttpResponseMessage Putusers(int id, users users)
        {
            if (ModelState.IsValid && id == users.id)
            {
                db.Entry(users).State = EntityState.Modified;

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

        // POST api/WSUsers
        public HttpResponseMessage Postusers(users users)
        {
            if (ModelState.IsValid)
            {
                db.users.Add(users);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, users);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = users.id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/WSUsers/5
        public HttpResponseMessage Deleteusers(int id)
        {
            users users = db.users.Find(id);
            if (users == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.users.Remove(users);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, users);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}