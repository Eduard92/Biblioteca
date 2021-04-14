using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Drawing;
using DataLayer;

namespace RK.Libraries
{
    public class File
    {
        public string allow_ext { get; set; }
        public string message { get; set; }
        public bool status { get; set; }
        public files data { get; set; }

        /********/

        public static string _ext;
        public static string _mimetype;
        public static string _type;
        public static Dictionary<string, string[]> allow_file_ext = new Dictionary<string, string[]>() { 
        
            {"a",new string[]{"mpga", "mp2", "mp3", "ra", "rv", "wav"}},
            {"v",new string[]{"mpeg", "mpg", "mpe", "mp4", "flv", "qt", "mov", "avi", "movie"}},
            {"d",new string[]{"pdf", "xls", "ppt", "pptx", "txt", "text", "log", "rtx", "rtf", "xml", "xsl", "doc", "docx", "xlsx", "word", "xl", "csv", "pages", "numbers"}},
            {"i",new string[]{"bmp", "gif", "jpeg", "jpg", "jpe", "png", "tiff", "tif"}},
            {"o",new string[]{"psd", "gtar", "swf", "tar", "tgz", "xhtml", "zip", "css", "html", "htm", "shtml", "svg"}},

        };
        public File()
        {
        }
        /*public static byte[] CropImage(Image image, int width,int height)
        {

        }*/
        public static byte[] ImageToByte(Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        public static void _check_ext(HttpPostedFileBase file)
        {
            _ext = Path.GetExtension(file.FileName);

            var ext = _ext.Substring(1, _ext.Length - 1);//_ext.Split(".".ToCharArray());


            foreach (var allowed in allow_file_ext)
            {
                if (allowed.Value.Contains(ext.ToLower()) == true)
                {
                    _type = allowed.Key;
                }
            }


        }
        //Eliminacion del archivo fisico y en la BD
        public static File delete_file(string id)
        {
            rekursosEntities db = new rekursosEntities();
            File result = new File();
            result.status = true;
            try
            {

                var file_result = db.files.Where(w => w.id == id).SingleOrDefault();

                if (file_result != null)
                {
                    _unlink_file(file_result);
                    db.files.Remove(file_result);
                    db.SaveChanges();

                    result.message = file_result.name + " ha sido eliminado";
                    result.status = true;
                }
            }
            catch (Exception ex)
            {
                result.status = false;
                result.message = ex.Message.ToString();
            }

            return result;
        }
        //Eliminacion del archivo fisico del disco duro
        public static bool _unlink_file(files file)
        {
            if (file.filename == null)
            {
                return false;
            }
            string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Uploads"), file.filename);

            if (System.IO.File.Exists(path) == true)
            {
                FileStream f = new FileStream(path, FileMode.Open);
                f.Close();
                f.Dispose();

                System.IO.File.Delete(path);
            }

            return true;

        }
        public static File get_file(string id)
        {
            File file = new File();
            rekursosEntities db = new rekursosEntities();
            var file_result = db.files.Where(w => w.id == id).SingleOrDefault();

            if (file_result == null)
            {
                file.status = false;
                file.message = "El archivo no existe";
            }
            else
            {
                file.status = true;

                file.data = file_result;
            }

            return file;
        }

        public static File SaveCamera(string image, int folder_id)
        {
            File result = new File();

            result.status = false;
            if (image != null)
            {
                rekursosEntities db = new rekursosEntities();
                files insert = new files();

                image = image.Replace("data:image/png;base64,", "");
                byte[] data = Convert.FromBase64String(image);

                MD5 md5 = MD5.Create();
                insert.filename = GetMd5Hash(md5, GetUniqID()) + ".png";
                insert.name = "camera_" + insert.filename;
                var destinationImgPath = Path.Combine(HttpContext.Current.Server.MapPath("~/Uploads"), insert.filename);

                System.IO.File.WriteAllBytes(destinationImgPath, data);

                if (System.IO.File.Exists(destinationImgPath))
                {

                    DateTime now = DateTime.Now;
                    var info = new System.IO.FileInfo(destinationImgPath);
                    // var dto = new DateTimeOffset(DateTime.Now);




                    insert.filesize = (int)info.Length;
                    insert.mimetype = "image/png";
                    insert.folder_id = folder_id;
                    insert.extension = ".png";
                    insert.type = "i";
                    insert.date_added = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;

                    System.Drawing.Image myImage = System.Drawing.Image.FromFile(destinationImgPath);


                    insert.width = myImage.Width;
                    insert.height = myImage.Height;

                    string id = GetMd5Hash(md5, insert.filename);



                    insert.id = id.Substring(0, 15);

                    db.files.Add(insert);
                    db.SaveChanges();


                    result.status = true;
                    result.message = "El archivo se ha subido correctamente";
                    result.data = insert;


                }

            }
            
            return result;
        }
        public static File upload(HttpPostedFileBase file, int folder_id)
        {
            rekursosEntities db = new rekursosEntities();
            files insert = new files();
            File  result = new File();

            result.status = false;

            try
            {
                

                var file_name = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(file.FileName);

                //Extraemos mime,type y extension
                _check_ext(file);


                MD5 md5 = MD5.Create();
                DateTime now = DateTime.Now;
                // var dto = new DateTimeOffset(DateTime.Now);


                insert.filename = GetMd5Hash(md5, GetUniqID()) + extension;
                insert.name = file_name;
                insert.filesize = file.ContentLength;
                insert.mimetype = file.ContentType;
                insert.folder_id = folder_id;
                insert.extension = _ext;
                insert.type = _type;
                insert.date_added = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
                insert.user_id = null;
                insert.width = null;
                insert.height = null;
                if (System.Web.HttpContext.Current.User != null)
                {
                    /*var user = Cobacam.Security.Ion_Auth.GetUser(System.Web.HttpContext.Current.User.Identity.Name);

                    if (user != null)
                    {
                        file_m.user_id = user.id;
                    }*/
                }
                string id = GetMd5Hash(md5, insert.filename);



                insert.id = id.Substring(0, 15);

                if (IsImage(file.ContentType))
                {
                    System.Drawing.Image myImage = System.Drawing.Image.FromStream(file.InputStream);


                    insert.width = myImage.Width;
                    insert.height = myImage.Height;

                }
               
                db.files.Add(insert);
                db.SaveChanges();

                var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Uploads"), insert.filename);
                file.SaveAs(path);
                result.status = true;
                result.message = "El archivo se ha subido correctamente";
                result.data = insert;


            }
            catch (Exception ex)
            {
               
               result.status = false;
               result.message = ex.Message.ToString();

            }
            return result;
        }
        public static bool IsImage(string mime)
        {
            string[] mimes = new string[] { "image/jpg", "image/jpe", "image/jpeg", "image/pjpeg", "image/x-png", "image/png" };

            return mimes.Contains(mime);


        }
        public static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public static string GetUniqID()
        {
            var ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            double t = ts.TotalMilliseconds / 1000;

            int a = (int)Math.Floor(t);
            int b = (int)((t - Math.Floor(t)) * 1000000);

            return a.ToString("x8") + b.ToString("x5");
        }
        /*public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;


                using (var wrapMode = new System.Drawing.Imaging.ImageAttributes())
                {
                    wrapMode.t
                    wrapMode.SetWrapMode(wrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }*/
        public static Image Resize(Image image, int targetWidth, int targetHeight)
        {
            var resizedImage = new Bitmap(targetWidth, targetHeight);

            using (var graphics = Graphics.FromImage(resizedImage))
            {
                graphics.DrawImage(image, 0, 0, targetWidth, targetHeight);
            }

            return resizedImage;
        }
    }
}