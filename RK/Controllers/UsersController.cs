using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data;
using DataLayer;
using RK.Libraries;
using RK.Security;
using RK.Models;
using RK.Core;
///using System.Web.Security;
namespace RK.Controllers
{
    [AuthorizeRoles(ModuleLevel = "users")]
    public class UsersController : AdminController
    {
        private rekursosEntities db = new rekursosEntities();
        public UsersController():base("users")
        {
            List<ShortCuts> short_cuts = new List<ShortCuts>();
            short_cuts.Add(new ShortCuts { name = "Nuevo usuario", uri = "users/create", icon = "ion-plus-round" });
            ViewBag.ShortCuts = short_cuts;
        }
        //
        // GET: /Users/
       
        
        
        public ActionResult Index()
        {
            List<vusers> list = db.vusers.ToList();
            return View(list);
        }
        [HttpPost]
        public ActionResult Login(string user="",string pass="",string ReturnUrl="")
        { 

            


            
            if (Ion_Auth.Login(user, pass)==true)
            {
                
                FormsAuthentication.SetAuthCookie(user,false);
                return Redirect(ReturnUrl==null?ReturnUrl:"/");
            }

            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }
        public ActionResult Logout()
        {

            
           

            // Delete the user details from cache.
            HttpContext.Session.Abandon();
            // Delete the authentication ticket and sign out.
            FormsAuthentication.SignOut();
            // Clear authentication cookie.
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie.Expires = DateTime.Now.AddYears(-1);

            Response.Cookies.Add(cookie);
            //response.Cookies.Add(cookie);
            return Redirect("~/");
           
           
        }
        public ActionResult Login(string ReturnUrl)
        {
           
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }
        //
        // GET: /Users/Details/5

        public ActionResult Details(int id=0)
        {
            return View();
        }

        //
        // GET: /Users/Create

        public ActionResult Create()
        {
            ViewBag.group_id = new SelectList(db.groups, "id", "description");

            return View();
            
        }

        //
        // POST: /Users/Create

        [HttpPost]
        public ActionResult Create(users user)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    user.created_on = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
                    user.password = Sha1.SHA1HashStringForUTF8String(user.password);
                    db.users.Add(user);
                    db.SaveChanges();
                   
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch(Exception ex)
            {
                FlashData.SetFlashData("error", ex.Message);
                return RedirectToAction("Index");
            }
        }

        //
        // GET: /Users/Edit/5

        public ActionResult Edit(int id)
        {
            users user = db.users.Find(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            UserModel user_m = new UserModel();
            user_m.group_id = user.group_id.GetValueOrDefault();
            user_m.active = user.active.GetValueOrDefault();
            user_m.id = user.id;
           
            user_m.display_name = user.display_name;
            user_m.username = user.username;
            user_m.email = user.email;

            ViewBag.group_id = new SelectList(db.groups.ToList(), "id", "description",user.id);
            /*ViewBag.active = new SelectList(new List<SelectListItem> { 
            
                new SelectListItem{Text="Activo",Value="1"},
                new SelectListItem{Text="Inactivo",Value="0"}
            }, "Value", "Text", user_m.active);
            */

            
            return View(user_m);
        }

        //
        // POST: /Users/Edit/5

        [HttpPost]
        public ActionResult Edit(UserModel user_m)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    users user = db.users.Find(user_m.id);

                    
                    user.id = user_m.id;
                    user.group_id = user_m.group_id;
                    user.active = user_m.active;
                    user.email = user_m.email;
                    user.username = user_m.username;
                    user.display_name = user_m.display_name;

                    if (user_m.password != null && user_m.password != "")
                        user.password = Sha1.SHA1HashStringForUTF8String(user_m.password);

                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    FlashData.SetFlashData("success", "La cuenta del usuario ha sido modificado satisfactoriamente");


                    return RedirectToAction("Index");
                }

                ViewBag.group_id = new SelectList(db.groups, "id", "description", user_m.group_id);
                return View(user_m);

            }
            catch(Exception ex)
            {
                FlashData.SetFlashData("error", ex.Message);
                return RedirectToAction("Index");
            }
        }

        //
        // GET: /Users/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Users/Delete/5

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
        public ActionResult Help()
        {
            return View();
        }
    }
}
