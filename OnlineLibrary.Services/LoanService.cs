using OnlineLibrary.Data.Infrastructure;
using OnlineLibrary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Services
{
  public class LoanService:ILoanService
  {
    private IRepository<Loans> loanRepo;
    private IUnitOfWork unitOfWork;

    public LoanService(IRepository<Loans> loanRepo, IUnitOfWork unitOfWork)
    {
      this.loanRepo = loanRepo;
      this.unitOfWork = unitOfWork;
    }

    public List<Loans> GetAllLoans()
    {
      return loanRepo.GetAll().ToList();
    }

    public void Loan(Loans loan)
    {
      loanRepo.Add(loan);
      unitOfWork.Commit();
    }

    public void ReturnBook(Loans loan)
    {
      loanRepo.Delete(loan);
      unitOfWork.Commit();
    }

    public List<Loans> GetList(int pageNumber, int pageSize, string sort, string sortDir)
    {
      return loanRepo.GetAll().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
      //return userRepo.GetAll((pageNumber - 1) * pageSize, pageSize, sort, sortDir).ToList();
    }

    public int Count()
    {
      return loanRepo.Count();
    }


  }
}

