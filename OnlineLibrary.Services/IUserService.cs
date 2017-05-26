using OnlineLibrary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Services
{
  public interface IUserService
  {
    List<Users> GetAllUsers();
    List<Users> GetList(int pageNumber, int pageSize, string sort, string sortDir);
    void CreateUser(Users user);
    int Count();
  }
}
