using HafifCMS.Helper;
using HafifCMS.ModelBinders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HafifCMS.Controllers
{
    public class AdminController : Controller
    {
        [AuthorizationAttribute]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            ViewBag.ReturnUrl = HttpContext.Request["returningURL"];

            return View();
        }

        public ActionResult Logout()
        {
            Session["Admin"] = null;
            return RedirectToAction("Login");
        }

        [HttpPost]
        public ActionResult Login(string txtUsername, string txtPassword, string returnUrl)
        {
            string Username = txtUsername.ToLower();
            string Password = Util.HashStringSHA1(txtPassword);

            string aUsername = ConfigurationManager.AppSettings["AdminUsername"].ToString().ToLower();
            string aPassword = ConfigurationManager.AppSettings["AdminPassword"].ToString();

            if (Username == aUsername && Password == aPassword)
            {
                Session["Admin"] = "1";
                if (!String.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index");
            }
            return View();

        }

        [AuthorizationAttribute]
        public ActionResult SiteConfigurations()
        {
            ViewBag.Fields = Util.GetData()["SiteConfigurations"];

            return View();
        }

        [AuthorizationAttribute]
        [HttpPost]
        public ActionResult SiteConfigurations([ModelBinder(typeof(FormModelBinder))] dynamic obj)
        {
            Dictionary<string, string> valuesToUpdate = new Dictionary<string, string>();

            foreach (var item in obj.fields)
            {
                valuesToUpdate.Add("siteconfigurations-" + item.Key, item.Value);
            }

            foreach (var file in obj.files)
            {
                if (file.Value != null && file.Value.ContentLength > 0)
                {
                    var filePath = "Images/" + Path.GetFileName(file.Value.FileName);
                    var path = Path.Combine(Server.MapPath("~/"), filePath);
                    file.Value.SaveAs(path);
                    valuesToUpdate.Add("siteconfigurations-" + file.Key, filePath);
                }
            }

            Util.UpdateDataFile(valuesToUpdate, new List<string>(), new List<string>());

            return RedirectToAction("SiteConfigurations");
        }

        [AuthorizationAttribute]
        public ActionResult PublishChanges()
        {
            Util.SetFileContent("generated_content", "");
            Util.SetFileContent("generated_head", "");
            return RedirectToAction("Index");
        }

        [AuthorizationAttribute]
        public ActionResult OpenGraph()
        {
            ViewBag.Fields = Util.GetData()["OpenGraph"];

            return View();
        }

        [AuthorizationAttribute]
        [HttpPost]
        public ActionResult OpenGraph([ModelBinder(typeof(FormModelBinder))] dynamic obj)
        {
            Dictionary<string, string> valuesToUpdate = new Dictionary<string, string>();

            foreach (var item in obj.fields)
            {
                valuesToUpdate.Add("opengraph-" + item.Key, item.Value);
            }

            Util.UpdateDataFile(valuesToUpdate, new List<string>(), new List<string>());

            return RedirectToAction("OpenGraph");
        }

        [AuthorizationAttribute]
        public ActionResult About(string job, string id)
        {
            if (job == "del" && !String.IsNullOrEmpty(id))
            {
                List<string> Delete = new List<string>();
                Delete.Add("about-" + id);
                Util.UpdateDataFile(new Dictionary<string, string>(), Delete, new List<string>());
            }
            if (job == "add")
            {
                List<string> Add = new List<string>();
                Add.Add("about");
                Util.UpdateDataFile(new Dictionary<string, string>(), new List<string>(), Add);
            }

            ViewBag.Fields = Util.GetData()["About"];

            return View("AboutListing");
        }
    }
}
