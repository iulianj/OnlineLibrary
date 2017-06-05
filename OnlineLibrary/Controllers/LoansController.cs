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
    public class LoansController : Controller
    {
    public ILoanService service { get; set; }

    public LoansController(ILoanService service)
    {
      this.service = service;
    }

    public ActionResult Loan(string loanedId)
    {
      List<BookViewModel> model = (List<BookViewModel>)Session["books_model"];
      if (Session["user_name"] != null)
      {
        
        Loans loanedBook = new Loans();
        loanedBook.BookID = Convert.ToInt32(loanedId);
        loanedBook.UserID = Convert.ToInt32(Session["user_id"]);
        loanedBook.loanDate = DateTime.Now;

        service.Loan(loanedBook);
      }
      else
      {
        TempData["must_login"] = "You must be logged in to loan books! Register if you don't have an account yet.";
        return RedirectToAction("../NewAccount/Login");
      }

      return PartialView("_Loan", model);
    }

    public ActionResult YAccount()
    {

      if (Session["user_name"] == null)
      {
        return RedirectToAction("Index", "Home");
      }
        int userID = Convert.ToInt32(Session["user_id"]);
        List<Loans> loans = service.GetAllLoans().FindAll(l=>l.UserID==userID);
        List<UserLoanedBooks> myLoans = new List<UserLoanedBooks>();
        
      foreach (var item in loans)
      {
        myLoans.Add(
          new UserLoanedBooks
          {
            Title = item.Book.Title,
            Author = item.Book.Author.FullName,
            LoanDate = item.loanDate.ToString("dd-MM-yyyy")
          }
        );
      }

      return View("Account", myLoans);
    }

  }
}