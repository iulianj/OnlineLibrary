using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using OnlineLibrary.Classes;

namespace OnlineLibrary.Controllers
{
  public class HomeController : Controller
  {
    public ActionResult Index()
    {
          
      return RedirectToAction("Index","Books");
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