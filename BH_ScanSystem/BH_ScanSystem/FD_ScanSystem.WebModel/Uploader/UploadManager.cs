using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using ZJFramework10.Extension;
namespace FD_ScanSystem.WebModel.Uploader
{
    public class UploadManager
    {


        public static readonly int MAX_UPLOADED = System.Configuration.ConfigurationManager.AppSettings["MAX_FILE"].AsInt(1024 * 2 * 1000);


        public static readonly string UPLOAD_LOC = System.Configuration.ConfigurationManager.AppSettings["UPLOAD_LOC"].AsStr("/Upload/");


        public static readonly string SWFTOOLS_LOC = System.Configuration.ConfigurationManager.AppSettings["SWFTOOLS_LOC"].AsStr(@"D:\SWFTools\pdf2swf.exe");

        public static UploadReturnLocation Upload(HttpPostedFile posted)
        {
            if (posted != null && posted.ContentLength > 0 && posted.ContentLength <= MAX_UPLOADED && posted.FileName.EndsWith("pdf", StringComparison.CurrentCultureIgnoreCase))
            {

                string uploadFileName = String.Format("{0:yyMMddHHmmss}.pdf", DateTime.Now);
                string pLoc = HttpContext.Current.Server.MapPath(String.Format("{0}{1}", UPLOAD_LOC, uploadFileName));
                posted.SaveAs(pLoc);
                UploadReturnLocation location = new UploadReturnLocation();
                location.FileLocation = string.Format("{0}{1}", UPLOAD_LOC, uploadFileName);
                location.SWFLocation = string.Format("{0}{1}", UPLOAD_LOC, uploadFileName.Replace("PDF", "SWF").Replace("pdf", "swf"));
                location.FilePhysicsLocation = pLoc;
                location.SWFPhysicsLocation = HttpContext.Current.Server.MapPath(location.SWFLocation);
                //   ConvertPDF2SWF(location);
                ThreadPool.QueueUserWorkItem(new WaitCallback(ConvertPDF2SWF), location);
                return location;
            }
            return null;

        }

        public static UploadReturnLocation ImgUpload(HttpPostedFile posted)
        {
            if (posted != null && posted.ContentLength > 0 && posted.ContentLength <= MAX_UPLOADED)
            {
                string filename = posted.FileName;
                string type = filename.Substring(filename.LastIndexOf(".") + 1);
                string uploadFileName = String.Format("IMG/{0:yyMMddHHmmss}.{1}", DateTime.Now, type);
                string pLoc = HttpContext.Current.Server.MapPath(String.Format("{0}{1}", UPLOAD_LOC, uploadFileName));
                posted.SaveAs(pLoc);
                UploadReturnLocation location = new UploadReturnLocation();
                location.FileLocation = string.Format("{0}{1}", UPLOAD_LOC, uploadFileName);
                //location.SWFLocation = string.Format("{0}{1}", UPLOAD_LOC, uploadFileName.Replace("PDF", "SWF").Replace("pdf", "swf"));
                //location.FilePhysicsLocation = pLoc;
                //location.SWFPhysicsLocation = HttpContext.Current.Server.MapPath(location.SWFLocation);
                //   ConvertPDF2SWF(location);
                ThreadPool.QueueUserWorkItem(new WaitCallback(ConvertPDF2SWF), location);
                return location;
            }
            return null;

        }



        protected static void ConvertPDF2SWF(object uploadLoc)
        {
            try
            {

                UploadReturnLocation location = uploadLoc as UploadReturnLocation;
                if (location == null) return;
                if (string.IsNullOrEmpty(SWFTOOLS_LOC) || !System.IO.File.Exists(SWFTOOLS_LOC)) return;
                ProcessStartInfo startInfo = new ProcessStartInfo(SWFTOOLS_LOC, string.Format(@"{0} -o {1} -T 9 -f", location.FilePhysicsLocation, location.SWFPhysicsLocation));

                var process = Process.Start(startInfo);
                using (var stream = process.StandardOutput)
                {
                    HttpContext.Current.Response.Write(stream.ReadToEnd());
                }
            }
            catch (Exception ex)
            {

            }
        }
    }

    public class UploadReturnLocation
    {

        public string FilePhysicsLocation { get; set; }
        public string SWFPhysicsLocation { get; set; }

        public string FileLocation { get; set; }

        public string SWFLocation { get; set; }
    }
}
