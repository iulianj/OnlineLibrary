using Omu.ValueInjecter;
using OnlineLibrary.Areas.Admin.Models;
using OnlineLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineLibrary.Areas.Admin
{
    public class AdminLoansController : Controller
    {
      public ILoanService service { get; set; }

      public AdminLoansController(ILoanService service)
      {
        this.service = service;
      }
      // GET: Admin/Loans
      public ActionResult Index()
      {
        List<ReturnBookModel> usersLoans = new List<ReturnBookModel>();
        var Loans = service.GetAllLoans();
        //usersLoans.InjectFrom(Loans);
        foreach (var item in Loans)
        {
          usersLoans.Add(
            new ReturnBookModel
            {
              ID=item.ID,
              UserName = item.User.FirstName + " " + item.User.LastName,
              Title = item.Book.Title,
              Author = item.Book.Author.FullName,
              LoanDate = item.loanDate.ToString("dd-MM-yyyy")
            }
          );
        }


      return View(usersLoans);
      }
      [HttpPost]
      public ActionResult Return(int LoanedBook)
      {
      var loan = service.GetAllLoans().Find(l => l.ID == LoanedBook);
      service.ReturnBook(loan);
      return RedirectToAction("Index");
      }
      
      [HttpPost, ActionName("Edit")]
      public ActionResult EditLoanedBook(int LoanedBook, int UserID, int BookID, string loanDate)
      {
      var loan = service.GetAllLoans().Find(l => l.ID == LoanedBook);
      loan.BookID = BookID;
      loan.UserID = UserID;
      loan.loanDate = DateTime.ParseExact(loanDate, "dd-MM-yyyy",null);
      service.EditLoan(loan);
      return RedirectToAction("Index");
      }


      public ActionResult OverDue()
      {
      var overDueLoans = service.GetAllLoans().Where(l => l.loanDate.AddDays(10) < DateTime.Today);
      List<OverDueLoans> overDueLoansView = new List<OverDueLoans>();

      foreach (var item in overDueLoans)
      {
        overDueLoansView.Add(
          new OverDueLoans
          {
            UserName = item.User.FirstName + " " + item.User.LastName,
            Title = item.Book.Title,
            Author = item.Book.Author.FullName,
            LoanDate = item.loanDate.ToString("dd-MM-yyyy")
          }
        );
      }
      return View("OverDueLoans", overDueLoansView);
      }

      
    }
}