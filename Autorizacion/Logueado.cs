using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebTIGA.Autorizacion
{
    public class Logueado : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (HttpContext.Current.Session["usuario"] == null)
            {
                httpContext.Session["ShowLoginFirstMessage"] = true;
                return false;
            }
            else
            {
                return true;
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            string lastPage = HttpContext.Current.Request.Url.AbsolutePath;

            base.HandleUnauthorizedRequest(filterContext);
            filterContext.Result = new RedirectResult("../Login/Login_?ReturnUrl=" + lastPage);
        }        
    }
}