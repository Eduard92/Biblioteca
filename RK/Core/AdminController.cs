using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using RK.Security;
using RK.Models;
namespace RK.Core
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

       
        public AdminController(string section = "", string icon = "")
        {
            rekursosEntities db = new rekursosEntities();
            System.Web.HttpContext HttpContext = System.Web.HttpContext.Current;
            var type = this.GetType();

            ViewBag.Type = type;
            Dictionary<string, List<Menu>> MenuItems = new Dictionary<string, List<Menu>>();

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var User = Ion_Auth.GetUser(HttpContext.User.Identity.Name);
                ViewBag.UserCurrent = User;
                Dictionary<string, string[]> ListPermissions = PermissionModel.GetGroup((int)User.group_id);

                foreach (var module in db.modules.Where(w => w.menu != "0" && w.is_backend==1).Where(w => w.installed == 1).ToList())
                {
                    if (ListPermissions.ContainsKey(module.slug) || User.groups.name == "admin")
                    {

                        if (MenuItems.ContainsKey(module.menu) == false)
                        {


                            MenuItems.Add(module.menu, new List<Menu>());


                        }
                        MenuItems[module.menu].Add(new Menu { slug = module.slug, name = module.name });
                   }


                }
            }
            

            Module ModuleDetails = new Module() { icon = "", description = "", name = "" };

            if (section != "")
            {

                var module = db.modules.Where(w => w.slug == section).SingleOrDefault();

                if (module != null)
                {
                    //ModuleDetails.ShortCuts = new List<ShortCuts>();
                    switch (module.slug)
                    {
                        /*case "viaticos":
                            ModuleDetails.icon = "ion-card";
                            break;
                        case "comisiones":
                            ModuleDetails.icon = "ion-android-list";
                            break;

                        case "empleados":
                            ModuleDetails.icon = "ion-ios-people";
                            break;
                        case "settings":
                            ModuleDetails.icon = "ion-gear-b";
                            break;
                        case "liquidaciones":
                            ModuleDetails.icon = "ion-cash";
                            break;*/
                        default:
                            //ModuleDetails.icon = icon;
                            break;
                    }
                    ModuleDetails.name = module.name;
                    ModuleDetails.description = module.description;



                    ///ModuleDetails.ShortCuts.Add(new ShortCuts { Name = "Agregar localidad", Class = "btn btn-success", Uri = "localidades/Create" });
                }
                else
                {
                    ModuleDetails.icon = "ion-help";
                    ModuleDetails.name = "NA";
                    ModuleDetails.description = "Sin descripción del módulo.";
                }
            }
            ViewBag.MenuItems = MenuItems;

            ViewBag.ModuleDetails = ModuleDetails;
        }

    }
}
