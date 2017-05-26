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
using OnlineLibrary.myClasses;

namespace OnlineLibrary.Areas.Admin.Controllers
{
  public class AuthorsController : Controller
  {
    public IAuthorService service { get; set; }

    public AuthorsController(IAuthorService service)
    {
      this.service = service;
    }
    [HttpPost]
    public ActionResult Addnew(string authorFirstName, string authorLastName)
    {
      Authors newAuthor = new Authors();
      newAuthor.FirstName = authorFirstName.Capitalize();
      newAuthor.LastName = authorLastName.Capitalize(); ;
      if(service.GetAllAuthors().Find(a=>a.FirstName==newAuthor.FirstName && a.LastName == newAuthor.LastName)==null)
      service.CreateAuthor(newAuthor);

      return new EmptyResult();
    }

    public JsonResult populateDropDown() { 

      List<Authors> allAuthors = service.GetAllAuthors();
      var authors = allAuthors.Select(a => new
      {
        AuthorID = a.ID,
        Text = a.FirstName + " " + a.LastName
      });
      return Json(authors, JsonRequestBehavior.AllowGet);
    }

  }
    
}
