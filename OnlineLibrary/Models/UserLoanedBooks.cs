using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineLibrary.Models
{
  public class UserLoanedBooks
  {
    public string Title { get; set; }
    public string Author { get; set; }
    public string LoanDate { get; set; }

  }
}