using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using System.Web.Security;
using System.Net;
using System.Text;
using System.Data;
using DataLayer;
using RK.Security;
using RK.Core;
using RK.Libraries;
using RK.Models;
namespace RK.Controllers
{
    public class AccountsController : AdminController
    {
        //
        // GET: /Accounts/
         //[BasicAuthentication]
        private rekursosEntities db = new rekursosEntities();
        public AccountsController()
        {

        }
        public ActionResult Index(string App="",string ReturnUrl="/")
        {
            string cookieToken;
            string formToken;
            
            AntiForgery.GetTokens(null, out cookieToken, out formToken);
            ViewBag.CookieToken = cookieToken;
            ViewBag.FormToken = formToken;
            ViewBag.App = App;
            ViewBag.ReturnUrl = ReturnUrl;

            //Version 1
            //var client = Security.ClientHelper.GetClient("bcauich", "osobabas");
            //var auth = client.DefaultRequestHeaders.Authorization;

            //Response.Write(FormsAuthentication.FormsCookiePath);

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string user="",string pass="",string ReturnUrl= "/",string App="")
        {
            if (Ion_Auth.Login(user, pass) == true)
            {
               

               
                if (App == "")
                {
                    App = "RK";
                   
                }
               
               
                string url = "";
                FormsAuthenticationTicket authticket = new FormsAuthenticationTicket(1, user, DateTime.Now, DateTime.Now.AddMinutes(40), true,user, FormsAuthentication.FormsCookiePath);
                
                // Encrypt the ticket.
                string encTicket = FormsAuthentication.Encrypt(authticket);

                // Create the cookie.
                Response.Cookies.Add(new HttpCookie(".APP" + App, encTicket));
                switch (App)
                {
                        case "COBAVIAT":
                        case "COVECO":
                            url = Settings.Get("app_"+App.ToLower()) + ReturnUrl;
                           
                            break;
                        default:
                            url = ReturnUrl;
                       
                            break;
                        
                            
                }


                return Redirect(url);
                   
               
                

                
            }
            ViewBag.App = App;
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }


        //
        // GET: /Accounts/Details/5
        [HttpGet]       
        public ActionResult Logout(string App="")
        {
            
            HttpContext.Session.Abandon();
            // Delete the authentication ticket and sign out.
            FormsAuthentication.SignOut();
            // Clear authentication cookie.
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName,"");
            cookie.Expires = DateTime.Now.AddYears(-1);

            Response.Cookies.Add(cookie);
            //response.Cookies.Add(cookie);
            return Redirect("~/");
        }
       // [AuthorizeRoles(ModuleLevel = "users")]
        public ActionResult Details()
        {
            UserModel user_m = new UserModel();
            DataLayer.users user = Ion_Auth.GetUser(User.Identity.Name);
            user_m.email = user.email;
            user_m.display_name = user.display_name;
            user_m.username = user.username;

            return View(user_m);
        }
        [HttpPost, ActionName("Details")]
        public ActionResult Save(UserModel user_m,string tab="profile")
        {
            users update = new users();
            users user = Ion_Auth.GetUser(User.Identity.Name,false);

            update.id = user.id;
            update.group_id = user.group_id;
            update.password = user.password;
            update.active = user.active;
            update.created_on = user.created_on;
            update.last_login = user.last_login;
            update.username = user.username;

            update.display_name = user.display_name;
            update.email = user.email;
           

            switch(tab)
            {
                case "profile":
                    
                    update.email = user_m.email;
                    update.display_name = user_m.display_name;
                    //update.password = user.password;
                    break;
                case "password":
                    if (user_m.old_password!="" && Sha1.SHA1HashStringForUTF8String(user_m.old_password) != user.password)
                    {
                        ModelState.AddModelError("Error", "La antigua contraseña es inválida.");
                    }
                    if (user_m.new_password != user_m.retype_password)
                    {
                        ModelState.AddModelError("Error1", "La nueva contraseña no coinciden.");
                    }
                    else
                    {
                        update.password = Sha1.SHA1HashStringForUTF8String(user_m.new_password);
                    }
                    break;
            }
            if (ModelState.IsValid)
            {

                //user.groups = null;
                db.Entry(update).State = EntityState.Modified;
                db.SaveChanges();

                FlashData.SetFlashData("success", "Tu cuenta ha sido modificada satisfactoriamente");

            }
            else
            {
                string errors = "";
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        errors += error.ErrorMessage+"\n";
                    }
                }

                if (errors != "")
                {
                    FlashData.SetFlashData("error", errors);
                }
            }
            return RedirectToAction("Details");
        }


       
    }
}
