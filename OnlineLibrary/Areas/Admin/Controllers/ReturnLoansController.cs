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
    public class ReturnLoansController : Controller
    {
      public ILoanService service { get; set; }

      public ReturnLoansController(ILoanService service)
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

      [HttpPost]
      public ActionResult EditLoanedBook(int LoanedBook)
      {
      return View();
      }

      
    }
}