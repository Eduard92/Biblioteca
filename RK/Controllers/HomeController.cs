using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RK.Core;
using DataLayer;
using System.Net;
using System.Text;

namespace RK.Controllers
{
    public class HomeController : AdminController
    {
        //
        // GET: /Home/
        private rekursosEntities db = new rekursosEntities();
        public ActionResult Index()
        {
            //HttpWebRequest request = new HttpWebRequest();
            //this.httpContext.Request.Headers["Authorization"];
           // HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.stackoverflow.com?question=1");
            //request.Credentials = new NetworkCredential("bcauich", "osobabas");
            
            //Version 1
            /*string credidentials =  "bcauich:osobabas" ;
            string authorization = Convert.ToBase64String(Encoding.Default.GetBytes(credidentials));
           HttpContext.Request.Headers["Authorization"] = "Basic " + authorization;

            Response.Write(HttpContext.Request.Headers["Authorization"]);*/

            //Version2

            /*var client = Security.ClientHelper.GetClient("bcauich", "osobabas");

            var auth = client.DefaultRequestHeaders.Authorization;*/

            //Response.Write(auth.Parameter);

            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.stackoverflow.com?question=1");
            //request.Credentials = new NetworkCredential("bcauich", "osobabas");

          //  Response.Write(HttpContext.Request.Headers["Authorization"]);

            Dictionary<string, int> Resume = new Dictionary<string, int>();

            Resume.Add("empleados", db.empleados.Where(w => w.activo==1).Count());
            Resume.Add("areas", db.cat_areas.Count());

            ViewBag.Resume = Resume;
            return View();
        }

        //
        // GET: /Home/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Home/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Home/Create

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
        // GET: /Home/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Home/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Home/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Home/Delete/5

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
    }
}
