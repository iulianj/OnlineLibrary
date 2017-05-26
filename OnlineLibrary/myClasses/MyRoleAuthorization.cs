using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OnlineLibrary.myClasses
{
  public class MyRoleAuthorization : AuthorizeAttribute
  {

    readonly string[] allowedTypes;


    public MyRoleAuthorization(params string[] allowedTypes)
    {
      this.allowedTypes = allowedTypes;
    }


    public string[] AllowedTypes
    {
      get { return this.allowedTypes; }
    }


    private string AuthorizeUser(AuthorizationContext filterContext)
    {
      if (filterContext.RequestContext.HttpContext != null)
      {
        var context = filterContext.RequestContext.HttpContext;
        string roleName = Convert.ToString(context.Session["RoleName"]);
        switch (roleName)
        {
          case "Admin":
          case "LoggedIn":
          case "Guest":
            return roleName;
          default:
            return "NotAuthorized";
        }
      }
      throw new ArgumentException("filterContext");
    }


    public override void OnAuthorization(AuthorizationContext filterContext)
    {
      if (filterContext == null)
        throw new ArgumentException("filterContext");
      string authUser = AuthorizeUser(filterContext);
      if (!this.AllowedTypes.Any(x => x.Equals(authUser, StringComparison.CurrentCultureIgnoreCase)))
      {
        //filterContext.Result = new HttpUnauthorizedResult();
        filterContext.Result=new RedirectToRouteResult(
               new RouteValueDictionary
               {
                    { "controller", "MyAccount" },
                    { "action", "Login" },
                    { "area",""}
               });
        return;
      }
    }
  }
}