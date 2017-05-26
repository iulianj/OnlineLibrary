using OnlineLibrary.Domain.Entities;
using OnlineLibrary.myClasses;
using OnlineLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineLibrary.Areas.Admin.Controllers
{
    public class PublishingsController : Controller
    {
    public IPublishingService service { get; set; }

    public PublishingsController(IPublishingService service)
    {
      this.service = service;
    }
    [HttpPost]
    public ActionResult AddNew(string publishing)
    {
      Publishings newPublishing = new Publishings();
      newPublishing.Publishing = publishing.Capitalize();
      if (service.GetAllPublishings().Find(p => p.Publishing == newPublishing.Publishing) == null)
        service.CreatePublishing(newPublishing);

      return new EmptyResult();
    }

    public JsonResult PopulateDropDown()
    {

      List<Publishings> allPublishings = service.GetAllPublishings();
      var publishings = allPublishings.Select(p => new
      {
        PublishingID = p.ID,
        Text = p.Publishing
      });
      return Json(publishings, JsonRequestBehavior.AllowGet);
    }
    }
}