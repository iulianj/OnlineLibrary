using OnlineLibrary.Domain.Entities;
using OnlineLibrary.Classes;
using OnlineLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineLibrary.Areas.Admin.Models;

namespace OnlineLibrary.Areas.Admin.Controllers
{
  public class AdminPublishingsController : Controller
  {
    public IPublishingService service { get; set; }

    public AdminPublishingsController(IPublishingService service)
    {
      this.service = service;
    }
    public ActionResult Index()
    {
      return View(service.GetAllRecords());
    }
    [HttpPost]
    public ActionResult AddNew(string publishing, int? viewReturn)
    {
      Publishings newPublishing = new Publishings();
      newPublishing.Publishing = publishing.Capitalize();
      if (service.GetAllRecords().Find(p => p.Publishing == newPublishing.Publishing) == null)
      {
        service.CreatePublishing(newPublishing);
      }
      else
      {
        ViewBag.ErrorMsg = "Publishing " + publishing.Capitalize() + " already exists!";
        ViewBag.ToController = "AdminPublishings";
        return View("PublishingExists");
      }
      if (viewReturn == null) return new EmptyResult();
      return RedirectToAction("Index");
    }

    public ActionResult Edit(AdminPublishingModel model)
    {
      try
      {
        if (ModelState.IsValid)
        {
          Publishings editPublishing = new Publishings();
          editPublishing.ID = model.publishingID;
          editPublishing.Publishing = model.publishing.Capitalize();
          service.EditPublishing(editPublishing);
        }
      }
      catch
      {
        ViewBag.ErrorMsg = "Publishing " + model.publishing.Capitalize() + " not exists!";
        ViewBag.ToController = "AdminPublishings";
        return View("NotFound");
      }

      return RedirectToAction("Index");
    }

    public ActionResult Delete(AdminPublishingModel model)
    {
      try
      {
        if (ModelState.IsValid)
        {
          Publishings deletePublishing = new Publishings();
          deletePublishing = service.GetAllRecords().Find(a => a.ID == model.publishingID);
          service.DeletePublishing(deletePublishing);
        }
      }
      catch
      {
        ViewBag.ErrorMsg = "Publishing " + model.publishing.Capitalize() + " not exists!";
        ViewBag.ToController = "AdminPublishings";
        return View("NotFound");
      }

      return RedirectToAction("Index");
    }


    public JsonResult PopulateDropDown()
    {

      List<Publishings> allPublishings = service.GetAllRecords();
      var publishings = allPublishings.Select(p => new
      {
        PublishingID = p.ID,
        Text = p.Publishing
      });
      return Json(publishings, JsonRequestBehavior.AllowGet);
    }
  }
}