using OnlineLibrary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Services
{
  public interface IBookService
  {
    List<Books> GetAllBooks();
    List<Books> GetList(int pageNumber, int pageSize, string sort, string sortDir);
    void CreateBook(Books book);

    void EditBook(Books book);
    int Count();
  }
}
