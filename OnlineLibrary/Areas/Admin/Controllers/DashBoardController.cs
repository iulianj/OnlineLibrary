using Omu.ValueInjecter;
using OnlineLibrary.Areas.Admin.Models;
using OnlineLibrary.Data.Entities;
using OnlineLibrary.Domain.Entities;
using OnlineLibrary.Models;
using OnlineLibrary.myClasses;
using OnlineLibrary.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineLibrary.Areas.Admin.Controllers
{
  public class DashBoardController : Controller
  {
    const int pageSize = 10;
    public IBookService service { get; set; }

    public DashBoardController(IBookService service)
    {
      this.service = service;
    }

    [MyRoleAuthorization("Admin")]
    public ActionResult Index()
    {
      List<BookViewModel> booksList = new List<BookViewModel>();

      var booksDbList = service.GetAllBooks();


      booksList.InjectFrom(booksDbList);
      for (var i = 0; i < booksList.Count; i++)
      {
        booksList[i].Author = booksDbList[i].Author.FullName;
      }

      return View(booksList);
    }


    public ActionResult Add()
    {
      BookStoreEntities model = new BookStoreEntities();
      ViewBag.AuthorID = new SelectList(model.Authors.OrderBy(m=>m.LastName), "ID", "FullName");
      ViewBag.CategoriesID = new SelectList(model.Categories.OrderBy(m => m.Category), "ID", "Category");
      ViewBag.PublishingsID = new SelectList(model.Publishings.OrderBy(m => m.Publishing), "ID", "Publishing");
      return View();
    }

    [HttpPost]
    public ActionResult Add(AddBookModel model)
    {
      try
      {
        if (ModelState.IsValid)
        {
          Books newBook = new Books();
          newBook.InjectFrom(model);
          var fileName = Path.GetFileName(newBook.File.FileName);
          var path = Path.Combine(Server.MapPath("~/images"), fileName);

          newBook.File.SaveAs(path);
          newBook.imageURL = "../../images/" + fileName;
          
          service.CreateBook(newBook);
          
        }

        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }
  }
}