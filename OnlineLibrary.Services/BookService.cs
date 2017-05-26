using OnlineLibrary.Data.Infrastructure;
using OnlineLibrary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OnlineLibrary.Services
{
  public class BookService:IBookService
  {
    private IRepository<Books> bookRepo;
    private IUnitOfWork unitOfWork;

    public BookService(IRepository<Books> bookRepo, IUnitOfWork unitOfWork)
    {
      this.bookRepo = bookRepo;
      this.unitOfWork = unitOfWork;
    }

    public List<Books> GetAllBooks()
    {
      return bookRepo.GetAll().ToList();
    }

    public void CreateBook(Books book)
    {
      bookRepo.Add(book);
      unitOfWork.Commit();
    }

    public List<Books> GetList(int pageNumber, int pageSize, string sort, string sortDir)
    {
      return bookRepo.GetAll().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
      //return userRepo.GetAll((pageNumber - 1) * pageSize, pageSize, sort, sortDir).ToList();
    }

    public int Count()
    {
      return bookRepo.Count();
    }


  }
}

