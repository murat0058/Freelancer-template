using HafifCMS.Helper;
using HafifCMS.ModelBinders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
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
        public ActionResult About()
        {
            ViewBag.Fields = Util.GetData()["About"];

            return View("AboutListing");
        }

        [AuthorizationAttribute]
        [HttpPost]
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

        [AuthorizationAttribute]
        public ActionResult GeneratePassword()
        {
            ViewBag.Result = String.Empty;

            return View();
        }

        [AuthorizationAttribute]
        [HttpPost]
        public ActionResult GeneratePassword(string newPassword)
        {
            ViewBag.Result = Util.HashStringSHA1(newPassword);

            return View();
        }

        [AuthorizationAttribute]
        public ActionResult AboutEdit(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return RedirectToAction("About");
            }

            dynamic objAbout = Util.GetData()["About"];
            dynamic objSelected = new ExpandoObject();

            bool exists = false;
            foreach (dynamic obj in objAbout)
            {
                if (obj["id"] == id)
                {
                    objSelected = obj;
                    exists = true;
                    break;
                }
            }
            if (!exists)
                return RedirectToAction("About");

            ViewBag.Fields = (dynamic)objSelected;
            var selectionList = new List<SelectListItem>();

            for (int i = 1; i < 13; i++)
            {
                selectionList.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }

            ViewBag.Selection = selectionList;

            return View();
        }

        [AuthorizationAttribute]
        [HttpPost, ValidateInput(false)]
        public ActionResult AboutEdit([ModelBinder(typeof(FormModelBinder))] dynamic obj)
        {
            Dictionary<string, string> valuesToUpdate = new Dictionary<string, string>();

            valuesToUpdate.Add("about-" + obj.fields.id, obj.fields.content);
            valuesToUpdate.Add("about-cols-" + obj.fields.id, obj.fields.cols);

            Util.UpdateDataFile(valuesToUpdate, new List<string>(), new List<string>());

            return RedirectToAction("About");
        }

        [AuthorizationAttribute]
        public ActionResult Footer()
        {
            ViewBag.Fields = Util.GetData()["Footer"][0];

            return View();
        }

        [AuthorizationAttribute]
        [HttpPost, ValidateInput(false)]
        public ActionResult Footer([ModelBinder(typeof(FormModelBinder))] dynamic obj)
        {
            Dictionary<string, string> valuesToUpdate = new Dictionary<string, string>();

            foreach (var item in obj.fields)
            {
                valuesToUpdate.Add("footer-" + item.Key, item.Value);
            }

            Util.UpdateDataFile(valuesToUpdate, new List<string>(), new List<string>());

            return RedirectToAction("Footer");
        }
    }
}
