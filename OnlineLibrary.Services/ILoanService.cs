using OnlineLibrary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Services
{
  public interface ILoanService
  {
    List<Loans> GetAllLoans();
    List<Loans> GetList(int pageNumber, int pageSize, string sort, string sortDir);
    void Loan(Loans loan);
    void ReturnBook(Loans loan);

    void EditLoan(Loans loan);
    int Count();
  }
}
