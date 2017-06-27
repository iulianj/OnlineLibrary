﻿using Omu.ValueInjecter;
using OnlineLibrary.Areas.Admin.Models;
using OnlineLibrary.Data.Entities;
using OnlineLibrary.Domain.Entities;
using OnlineLibrary.Models;
using OnlineLibrary.Classes;
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
    public ActionResult Index(string searchString)
    {
      List<BookViewModel> booksList = new List<BookViewModel>();

      var booksDbList = service.GetAllRecords();


      booksList.InjectFrom(booksDbList);
      for (var i = 0; i < booksList.Count; i++)
      {
        booksList[i].Author = booksDbList[i].Author.FullName;
        booksList[i].Category = booksDbList[i].Category.Category;
        booksList[i].Publishing = booksDbList[i].Publishing.Publishing;
      }

      var model = booksList;
      if (searchString != null)
      {
        int yearParsed = 0;
        //string searchString = Session["searchString"].ToString();
        bool isNumeric = int.TryParse(searchString, out yearParsed);

        string searchStringLower = searchString.ToLower();
        var booksListSearch = booksList.Where(x => x.Title.ToLower().Contains(searchStringLower)
                                || x.Category.ToLower().Contains(searchStringLower)
                                || x.Author.ToLower().Contains(searchStringLower)
                                || x.ISBN.ToLower().Contains(searchStringLower)
                                || x.Publishing.ToLower().Contains(searchStringLower)
                                || x.Year == yearParsed);
        model = booksListSearch.ToList();
      }

      return View(model);
    }


    public ActionResult Add()
    {
      AdminBookModel addBook = new AdminBookModel();
      BookStoreEntities model = new BookStoreEntities();

      addBook.Authors = new SelectList(model.Authors.OrderBy(m => m.LastName), "ID", "FullName");
      addBook.Categories = new SelectList(model.Categories.OrderBy(m => m.Category), "ID", "Category");
      addBook.Publishings = new SelectList(model.Publishings.OrderBy(m => m.Publishing), "ID", "Publishing");

      //ViewBag.AuthorID = new SelectList(model.Authors.OrderBy(m=>m.LastName), "ID", "FullName");
      //ViewBag.CategoriesID = new SelectList(model.Categories.OrderBy(m => m.Category), "ID", "Category");
      //ViewBag.PublishingsID = new SelectList(model.Publishings.OrderBy(m => m.Publishing), "ID", "Publishing");
      return View(addBook);
    }

    [HttpPost]
    public ActionResult Add(AdminBookModel model)
    {
      try
      {
        if (ModelState.IsValid)
        {
          Books newBook = new Books();
          newBook.InjectFrom(model);
          if (newBook.File != null)
          {
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
        ViewBag.ToController = "DashBoard";
        return View("NotFound");
      }
      Books books = service.GetAllRecords().Find(b => b.ID == id);
      if (books == null)
      {
        ViewBag.ErrorMsg = "Error. Book not exist";
        ViewBag.ToController = "DashBoard";
        return View("NotFound");
      }
      AdminBookModel editBook = new AdminBookModel();
      editBook.InjectFrom(books);
      BookStoreEntities model = new BookStoreEntities();
      editBook.Authors = new SelectList(model.Authors.OrderBy(m => m.LastName), "ID", "FullName");
      editBook.Categories = new SelectList(model.Categories.OrderBy(m => m.Category), "ID", "Category");
      editBook.Publishings = new SelectList(model.Publishings.OrderBy(m => m.Publishing), "ID", "Publishing");
      return View("Edit", editBook);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(AdminBookModel model)
    {

      try
      {
        if (ModelState.IsValid)
        {
          Books newBook = new Books();
          newBook.InjectFrom(model);
          if (newBook.File != null)
          {
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
        ViewBag.ToController = "DashBoard";
        return View("NotFound");
      }
      Books books = service.GetAllRecords().Find(b => b.ID == id);
      if (books == null)
      {
        ViewBag.ErrorMsg = "Error. Book not exist";
        ViewBag.ToController = "DashBoard";
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

    public ActionResult Delete(int? id)
    {
      if (id == null)
      {
        ViewBag.ErrorMsg = "Error. Book not exist";
        ViewBag.ToController = "DashBoard";
        return View("NotFound");
      }
      Books books = service.GetAllRecords().Find(b => b.ID == id);
      if (books == null)
      {
        ViewBag.ErrorMsg = "Error. Book not exist";
        ViewBag.ToController = "DashBoard";
        return View("NotFound");
      }
      BookViewModel model = new BookViewModel();
      model.InjectFrom(books);

      model.Author = books.Author.FullName;
      model.Category = books.Category.Category;
      model.Publishing = books.Publishing.Publishing;
      return View(model);

    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
      Books book = service.GetAllRecords().Find(b => b.ID == id);
      service.DeleteBook(book);
      return RedirectToAction("Index");
    }

    public JsonResult PopulateDropDown()
    {

      List<Books> allBooks = service.GetAllRecords();
      var books = allBooks.Select(b => new
      {
        BookID = b.ID,
        Text = b.Title.Trim() + " - " + b.Author.FullName.Trim()
      });
      return Json(books, JsonRequestBehavior.AllowGet);
    }
  }
}