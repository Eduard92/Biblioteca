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
    public class WSGroupsController : ApiController
    {
        private rekursosEntities db = new rekursosEntities();

        // GET api/Default1
        public IEnumerable<vgroups> Getgroups()
        {
            var groups = db.vgroups.AsEnumerable();
           
           
            return groups;
        }



        public vgroups Get(int id)
        {
            return db.vgroups.Where(w => w.id == id).SingleOrDefault();

        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}