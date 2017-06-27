using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineLibrary.Data.Entities;
using OnlineLibrary.Domain.Entities;
using OnlineLibrary.Services;
using OnlineLibrary.Classes;
using OnlineLibrary.Areas.Admin.Models;
using Omu.ValueInjecter;

namespace OnlineLibrary.Areas.Admin.Controllers
{
  public class AdminAuthorsController : Controller
  {
    public IAuthorService service { get; set; }

    public AdminAuthorsController(IAuthorService service)
    {
      this.service = service;
    }
    public ActionResult Index()
    {
      return View(service.GetAllRecords());
    }
    [HttpPost]
    public ActionResult AddNew(string authorFirstName, string authorLastName, int? viewReturn)
    {
      Authors newAuthor = new Authors();
      newAuthor.FirstName = authorFirstName.Capitalize();
      newAuthor.LastName = authorLastName.Capitalize(); ;
      if (service.GetAllRecords().Find(a => a.FirstName == newAuthor.FirstName && a.LastName == newAuthor.LastName) == null)
      {
        service.CreateAuthor(newAuthor);
      }
      else
      {
        ViewBag.ErrorMsg = "Author "+authorFirstName.Capitalize()+" "+authorLastName.Capitalize()+" already exists!";
        ViewBag.ToController = "AdminAuthors";
        return View("AuthorExists");
      }
      if(viewReturn == null) return new EmptyResult();
      return RedirectToAction("Index");
      //return PartialView("_Authors", service.GetAllRecords());
    }

    public ActionResult Edit(AdminAuthorModel model)
    {
      try
      {
        if (ModelState.IsValid)
        {
          Authors editAuthor = new Authors();
          editAuthor.ID = model.authorID;
          editAuthor.FirstName = model.authorFirstName.Capitalize();
          editAuthor.LastName = model.authorLastName.Capitalize();
          service.EditAuthor(editAuthor);
        }
      }
      catch {
        ViewBag.ErrorMsg = "Author " + model.authorFirstName.Capitalize() + " " + model.authorLastName.Capitalize() + " not exists!";
        ViewBag.ToController = "AdminAuthors";
        return View("NotFound");
      }

      return RedirectToAction("Index");
    }

    public ActionResult Delete(AdminAuthorModel model)
    {
      try
      {
        if (ModelState.IsValid)
        {
          Authors deleteAuthor = new Authors();
          deleteAuthor = service.GetAllRecords().Find(a => a.ID == model.authorID);
          service.DeleteAuthor(deleteAuthor);
        }
      }
      catch
      {
        ViewBag.ErrorMsg = "Author " + model.authorFirstName.Capitalize() + " " + model.authorLastName.Capitalize() + " not exists!";
        ViewBag.ToController = "AdminAuthors";
        return View("NotFound");
      }

      return RedirectToAction("Index");
    }

    public JsonResult PopulateDropDown() { 

      List<Authors> allAuthors = service.GetAllRecords();
      var authors = allAuthors.Select(a => new
      {
        AuthorID = a.ID,
        Text = a.FirstName + " " + a.LastName
      });
      return Json(authors, JsonRequestBehavior.AllowGet);
    }

    public ActionResult AuthorExists()
    {
      return View();
    }

  }
    
}
