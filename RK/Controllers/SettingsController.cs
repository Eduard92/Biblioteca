using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RK.Models;
using DataLayer;
using RK.Libraries;
using RK.Core;

namespace RK.Controllers
{
    public class SettingsController : AdminController
    {
        //
        // GET: /Settings/
        private rekursosEntities db = new rekursosEntities();
        public SettingsController():base("settings")
        {
        }
        public ActionResult Index()
        {
            //db.settings.ToList()

            SettingModel setting_m = new SettingModel();

            setting_m.Sections = new Dictionary<string, string>();
            setting_m.Settings = new Dictionary<string, List<FormsModel>>();
            string section_active = "";

            foreach (var setting in db.settings.Where(w => w.is_gui == 1)
                                    .ToList())
            {
                setting.module = setting.module == null || setting.module == "" ? "general" : setting.module;


                if (setting_m.Sections.ContainsKey(setting.module) == false)
                {
                    setting_m.Sections.Add(setting.module, SettingModel.GetTitle(setting.module));
                    setting_m.Settings.Add(setting.module, new List<FormsModel>());
                }
                //string form_control = FormsModel.BuildForm(setting);
                setting_m.Settings[setting.module].Add(new FormsModel { slug = setting.slug, title = setting.title, description = setting.description, type = setting.type, value = setting.value, module = setting.module, options = setting.options });




            }
            //Response.Write(System.Web.Helpers.Json.Encode(setting_m.Settings));
            return View(setting_m);
        }
        [HttpPost, ActionName("Index")]
        public ActionResult Edit(SettingModel setting_m)
        {
            foreach (var secctions in setting_m.Settings)
            {
                foreach (var setting_new in secctions.Value)
                {
                    settings setting = db.settings.Where(w => w.slug == setting_new.slug).SingleOrDefault();

                    if (setting != null)
                    {
                        if (setting_new.values != null)
                        {
                            setting.value = String.Join(",", setting_new.values);// setting_new.values.Join(",");
                        }
                        else
                        {
                            setting.value = setting_new.value;
                        }

                        db.Entry(setting).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }

            /* if (ModelState.IsValid)
             {
                 db.Entry(setting).State = EntityState.Modified;
                 db.SaveChanges();
                 return RedirectToAction("Index");
             }*/
            //return View(setting);
            FlashData.SetFlashData("success", "Tu configuración ha sido guardada.");
            return RedirectToAction("Index");
        }

    }
}
