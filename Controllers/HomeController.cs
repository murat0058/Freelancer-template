using HafifCMS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HafifCMS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Util.GetFileContent("generated_content") == "" || Util.GetFileContent("generated_head") == "")
                PageRender.GeneratePage();

            ViewBag.Header = Util.GetFileContent("generated_head");
            ViewBag.Content = Util.GetFileContent("generated_content");

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}