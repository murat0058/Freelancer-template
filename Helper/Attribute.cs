using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace HafifCMS.Helper
{
    public class AuthorizationAttribute : AuthorizeAttribute
    {
        public AuthorizationAttribute(): base()
        {
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //You must check if the user has logged in and return true if he did that.
            return (httpContext.Session["Admin"] != null && httpContext.Session["Admin"].ToString() == "1");

        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.HttpContext.Response.Redirect("/admin/login?returningURL=" +
                filterContext.HttpContext.Server.UrlEncode(filterContext.HttpContext.Request.Url.AbsoluteUri.ToString()));

        }

    }
}