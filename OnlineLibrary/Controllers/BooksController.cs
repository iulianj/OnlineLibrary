using Omu.ValueInjecter;
using Omu.ValueInjecter.Injections;
using OnlineLibrary.Classes;
using OnlineLibrary.Controllers;
using OnlineLibrary.Domain.Entities;
using OnlineLibrary.Models;
using OnlineLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineLibrary.Controllers
{
  public class BooksController : Controller
  {
    const int pageSize = 10;
    public IBookService service { get; set; }

    public BooksController(IBookService service)
    {
      this.service = service;
    }

    // GET: Books
    public ActionResult Index(int? page, string sort, string sortdir, string searchString)
    {
      Session["searchString"] = searchString;
      var pageNumber = page.HasValue ? page.Value : 1;
      ViewBag.CurrentPage = pageNumber;
      ViewBag.SearchString = searchString;
      ViewBag.Action = null;

      var model=CreateBooksModel(pageNumber,sort, sortdir,searchString);
      ViewBag.Count = model.Count();
      ViewBag.TotalPages = Math.Ceiling((double)ViewBag.Count / pageSize);
      ViewBag.PageSize = pageSize;
      Session["books_model"] = model;
      

      
      
      if (searchString != null && searchString !="")
      {
        ViewBag.Title = "Search Result";
        ViewBag.SubTitle = "Search result: ";
        ViewBag.Action = "search";
        

        return PartialView("_Books", model.Skip((pageNumber - 1) * pageSize).Take(pageSize));
      }
      
        ViewBag.Title = "Our Books";
        ViewBag.SubTitle = "You can loan from ";
      
      try
      {
        if (Session["user_name"] == null)
        {
          ViewBag.Message = "Hello " + Session["userName"];
        }
      }
      catch
      {
        ViewBag.Message = "Sign in please!";
      }
      if (searchString == null)
      {
        return View(model.Skip((pageNumber - 1) * pageSize).Take(pageSize));
      }
      else
      {
        ViewBag.Title = "Search Result";
        ViewBag.SubTitle = "Search result: 0 books";
        ViewBag.Action = "search";


        return PartialView("_NotFound");
      }
    }


    private List<BookViewModel> CreateBooksModel(int pageNumber, string sort, string sortdir, string searchString)
    {

      List<BookViewModel> booksList = new List<BookViewModel>();

      var booksDbList = service.GetAllBooks();


      booksList.InjectFrom(booksDbList);
      for (var i = 0; i < booksList.Count; i++)
      {
        booksList[i].Author = booksDbList[i].Author.FullName;
        booksList[i].Category = booksDbList[i].Category.Category;
        booksList[i].Publishing = booksDbList[i].Publishing.Publishing;
        booksList[i].Availability = booksDbList[i].NoOfBooks - booksDbList[i].Loans.Where(l => l.BookID == booksDbList[i].ID).Count();
        booksList[i].AvailabilityDate = (booksDbList[i].Loans.Where(l => l.BookID == booksDbList[i].ID).Count() != 0) ?
                     booksDbList[i].Loans.Where(l => l.BookID == booksDbList[i].ID).OrderBy(l => l.loanDate).First().loanDate.ToString("dd-MM-yyyy") : "";
      }
      if (searchString != null && searchString !="")
      {
        var booksListSearch=CreateSearchBooksModel(booksList);
        return booksListSearch.ToList();
      }
      return booksList;
    }

    private List<BookViewModel> CreateSearchBooksModel(List<BookViewModel> booksList)
    {
      int yearParsed = 0;
      string searchString = Session["searchString"].ToString();
      bool isNumeric = int.TryParse(searchString, out yearParsed);

      string searchStringLower = searchString.ToLower();
      var booksListSearch = booksList.Where(x => x.Title.ToLower().Contains(searchStringLower)
                              || x.Category.ToLower().Contains(searchStringLower)
                              || x.Author.ToLower().Contains(searchStringLower)
                              || x.ISBN.ToLower().Contains(searchStringLower)
                              || x.Publishing.ToLower().Contains(searchStringLower)
                              || x.Year == yearParsed);
      return booksListSearch.ToList();
    }

    [HttpPost]
    public ActionResult FilterBooks(int? page, FormCollection collection)
    {
      
      string categories = collection["CategoriesID"];
      string publishings = collection["PublishingsID"];
      string authors = collection["AuthorsID"];
      string years = collection["Year"];

      var pageNumber = page.HasValue ? page.Value : 1;
      ViewBag.CurrentPage = pageNumber;
      string searchString= "", sort = null, sortdir = null;

      
      var booksListFilter = CreateBooksModel(pageNumber, sort, sortdir, searchString);
      //var booksListFilter = model;
      if (categories == null && publishings == null && authors == null && years == null)
      {
        ViewBag.Count = service.Count();
        ViewBag.TotalPages = Math.Ceiling((double)ViewBag.Count / pageSize);
        ViewBag.PageSize = pageSize;
        ViewBag.Title = "Our Books";
        ViewBag.SubTitle = "You can loan from: ";
        ViewBag.Action = "index";
        Session["books_model"] = booksListFilter;
        return PartialView("_Books", booksListFilter.ToList().Skip((pageNumber - 1) * pageSize).Take(pageSize));

      }
      else
      {
 
        string[] cats = (categories != null)?categories.Split(','): new string[0];
        string[] pubs = (publishings != null)? publishings.Split(','): new string[0];
        string[] aths = (authors != null)? authors.Split(',') : new string[0];
        string[] year = (years != null)? years.Split(','): new string[0];

        booksListFilter = booksListFilter.Where(x => cats.Contains(x.CategoriesID.ToString())
                              || pubs.Contains(x.PublishingsID.ToString())
                              || aths.Contains(x.AuthorID.ToString())
                              || year.Contains(x.Year.ToString())
                              ).ToList();

        ViewBag.Count = booksListFilter.Count();
        ViewBag.TotalPages = Math.Ceiling((double)ViewBag.Count / pageSize);
        ViewBag.PageSize = pageSize;
        ViewBag.Title = "Filter Result";
        ViewBag.SubTitle = "Filter result: ";
        ViewBag.Action = "filter";
        Session["books_model"] = booksListFilter;
        return PartialView("_Books", booksListFilter.Skip((pageNumber - 1) * pageSize).Take(pageSize));
        

      }

    }

    [HttpPost]
    public ActionResult SortBooks(int? page,string orderby)
    {
      
      var pageNumber = page.HasValue ? page.Value : 1;
      ViewBag.CurrentPage = pageNumber;
      ViewBag.Title = "Sorted Books";
      ViewBag.Action = "sort";
      ViewBag.OrderBy = orderby;
      List<BookViewModel> model = (List<BookViewModel>)Session["books_model"];
      List<BookViewModel> sortModel=new List<BookViewModel>();
      switch (orderby)
      {
        case "category":
          sortModel = model.OrderBy(m => m.Category).ToList();
          ViewBag.SubTitle = "Sort by <b>Category</b> ";
          break;

        case "author":
          sortModel = model.OrderBy(m => m.Author.Split(' ').Last()).ToList();
          ViewBag.SubTitle = "Sort by <b>Author</b> ";
          break;

        case "title":
          sortModel = model.OrderBy(m => m.Title).ToList();
          ViewBag.SubTitle = "Sort by <b>Title</b> ";
          break;

        case "publishing":
          sortModel = model.OrderBy(m => m.Publishing).ToList();
          ViewBag.SubTitle = "Sort by <b>Publishing</b> ";
          break;

        case "year":
          sortModel = model.OrderBy(m => m.Year).ToList();
          ViewBag.SubTitle = "Sort by <b>Year ascending</b> ";
          break;

        case "year-desc":
          sortModel = model.OrderByDescending(m => m.Year).ToList();
          ViewBag.SubTitle = "Sort by <b>Year descending</b> ";
          break;

      }

      ViewBag.Count = sortModel.Count();
      ViewBag.TotalPages = Math.Ceiling((double)ViewBag.Count / pageSize);
      ViewBag.PageSize = pageSize;

      return PartialView("_Books", sortModel.Skip((pageNumber - 1) * pageSize).Take(pageSize));
    }
    [HttpPost]
    public ActionResult LoadModal(string bookId)
    {
      List<BookViewModel> model = (List<BookViewModel>)Session["books_model"];
      
      return PartialView("_BookDetail", model.Where(m => m.ID.ToString() == bookId));
    }


  }
}