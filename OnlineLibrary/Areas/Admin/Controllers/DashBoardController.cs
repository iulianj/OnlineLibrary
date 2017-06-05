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
using System.Net;
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

    [RoleAuthorization("Admin")]
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
      AddBookModel addBook = new AddBookModel();
      BookStoreEntities model = new BookStoreEntities();

      addBook.Authors= new SelectList(model.Authors.OrderBy(m => m.LastName), "ID", "FullName");
      addBook.Categories = new SelectList(model.Categories.OrderBy(m => m.Category), "ID", "Category");
      addBook.Publishings = new SelectList(model.Publishings.OrderBy(m => m.Publishing), "ID", "Publishing");

      //ViewBag.AuthorID = new SelectList(model.Authors.OrderBy(m=>m.LastName), "ID", "FullName");
      //ViewBag.CategoriesID = new SelectList(model.Categories.OrderBy(m => m.Category), "ID", "Category");
      //ViewBag.PublishingsID = new SelectList(model.Publishings.OrderBy(m => m.Publishing), "ID", "Publishing");
      return View(addBook);
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
          if (newBook.File != null) {
          var fileName = Path.GetFileName(newBook.File.FileName);
          var path = Path.Combine(Server.MapPath("~/images"), fileName);

          newBook.File.SaveAs(path);
          newBook.imageURL = "../../images/" + fileName;
        }
        else
        {
          newBook.imageURL = "../../images/default.png";
        }
          
          service.CreateBook(newBook);
          
        }

        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }

    public ActionResult Edit(int? id)
    {
      if (id == null)
      {
        ViewBag.ErrorMsg = "Error. Book not exist";
        return View("NotFound");
      }
      Books books = service.GetAllBooks().Find(b=>b.ID==id);
      if (books == null)
      {
        ViewBag.ErrorMsg = "Error. Book not exist";
        return View("NotFound");
      }
      AddBookModel editBook = new AddBookModel();
      editBook.InjectFrom(books);
      BookStoreEntities model = new BookStoreEntities();
      editBook.Authors = new SelectList(model.Authors.OrderBy(m => m.LastName), "ID", "FullName");
      editBook.Categories = new SelectList(model.Categories.OrderBy(m => m.Category), "ID", "Category");
      editBook.Publishings = new SelectList(model.Publishings.OrderBy(m => m.Publishing), "ID", "Publishing");
      return View("Edit",editBook);
    }

    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(AddBookModel model)
    {

      try
      {
        if (ModelState.IsValid)
        {
          Books newBook = new Books();
          newBook.InjectFrom(model);
          if (newBook.File != null) { 
          var fileName = Path.GetFileName(newBook.File.FileName);
          var path = Path.Combine(Server.MapPath("~/images"), fileName);

          newBook.File.SaveAs(path);
          newBook.imageURL = "../../images/" + fileName;
        }
        

       

          service.EditBook(newBook);

        }

        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }

    public ActionResult Details(int? id)
    {
      if (id == null)
      {
        ViewBag.ErrorMsg = "Error. Book not exist";
        return View("NotFound");
      }
      Books books = service.GetAllBooks().Find(b => b.ID == id);
      if (books == null)
      {
        ViewBag.ErrorMsg = "Error. Book not exist";
        return View("NotFound");
      }
      BookViewModel model = new BookViewModel();
      model.InjectFrom(books);
      
      model.Author = books.Author.FullName;
      model.Category = books.Category.Category;
      model.Publishing = books.Publishing.Publishing;
      model.Availability = books.NoOfBooks - books.Loans.Where(l => l.BookID == books.ID).Count();
      model.AvailabilityDate = (books.Loans.Where(l => l.BookID == books.ID).Count() != 0) ?
                     books.Loans.Where(l => l.BookID == books.ID).OrderBy(l => l.loanDate).First().loanDate.ToString("dd-MM-yyyy") : "";
      

      return View(model);
    }

  }
}