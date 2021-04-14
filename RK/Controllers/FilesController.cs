using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using System.IO;
using System.Drawing;

namespace RK.Controllers
{
    public class FilesController : Controller
    {
        //
        // GET: /Files/
        private rekursosEntities db = new rekursosEntities();
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Cloud_Thumb(string id, int? width = 100, int? height = 100, string mode = "fit")
        {
            System.Web.HttpContext HttpContext = System.Web.HttpContext.Current;
            files file_bd = db.files.Where(w => w.id == id).SingleOrDefault();
            string path = "";
            if (file_bd == null)
            {
                path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/img/no-image.png"));

                return File(path, "image/png");
                // return HttpNotFound();
            }

            
            try
            {

                path = Path.Combine(HttpContext.Current.Server.MapPath("~/Uploads"), file_bd.filename);
               
                /*var  ratio = file_bd.width / file_bd.height;
                var crop_width = width;
                var crop_height = height;
                double crop_ratio	= (crop_height == null || crop_width == null) ? 0 : (double)(crop_width / crop_height);


                if (ratio >= crop_ratio && crop_height > 0)
				{
                    var tmp_w = ratio * crop_height;
					width	= (int)Math.Ceiling((decimal)tmp_w);
					height	= crop_height;
				}
				else
				{
					width	= crop_width;
					height	= crop_width / ratio;
				}
                Response.Write(ratio.ToString());*/

                //return Content(width.ToString()+" "+height.ToString());

                if (System.IO.File.Exists(path) == true)
                {

                    //Response.Write(path);
                    //return Content("");
                    using (Image image = Image.FromFile(path))
                    {

                        var thumb = image.GetThumbnailImage((int)width, (int)height, () => false, IntPtr.Zero);

                        return File(RK.Libraries.File.ImageToByte(thumb), file_bd.mimetype);
                    }
                }
                else
                {
                    path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/img/no-image.png"));

                    return File(path, "image/png");
                }
            }
            catch (FileNotFoundException ex)
            {
                return new HttpStatusCodeResult(500);
            }
        }

    }
}
