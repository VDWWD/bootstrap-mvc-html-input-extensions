using System;
using System.IO;
using System.Web.Mvc;

namespace DemoWebsite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string version)
        {
            var model = new Models.HomeViewModel()
            {
                Name = "van der Waal Webdesign",
            };

            //set the bootstrap version: www.site.com?version=5
            if (version == "5")
            {
                BootstrapHelper.Start(BootstrapHelper.BootstrapVersion.Version5);
            }
            else
            {
                BootstrapHelper.Start(BootstrapHelper.BootstrapVersion.Version4);
            }

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Models.HomeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ResultMessage = "There was an error submitting the form.";
            }

            //handle the form post here
            model.Result = true;
            model.ResultMessage = "Congratulations! You submitted the form.";

            //just to show how to work with HttpPostedFileBase

            //check if there is an upload
            if (model.Image == null || model.Image.ContentLength == 0 || string.IsNullOrEmpty(model.Image.FileName))
            {
                return View(model);
            }

            //copy the file into a MemoryStream so you can do all kinds of stuff with it
            using (var target = new MemoryStream())
            {
                model.Image.InputStream.Position = 0;
                model.Image.InputStream.CopyTo(target);

                //save as byte array
                byte[] bin = target.ToArray();

                //or convert the stream to base 64
                model.ImageBase64 = string.Format("data:image/jpeg;base64,{0}", Convert.ToBase64String(target.ToArray()));
            }

            return View(model);
        }
    }
}