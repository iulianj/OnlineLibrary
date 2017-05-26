using OnlineLibrary.Data.Infrastructure;
using OnlineLibrary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Services
{

    public class UserService:IUserService
    {
    private IRepository<Users> userRepo;
    private IUnitOfWork unitOfWork;

    public UserService(IRepository<Users> userRepo, IUnitOfWork unitOfWork)
    {
      this.userRepo = userRepo;
      this.unitOfWork = unitOfWork;
    }

    public List<Users> GetAllUsers()
    {
      return userRepo.GetAll().ToList();
    }

    public void CreateUser(Users user)
    {
      userRepo.Add(user);
      unitOfWork.Commit();
    }

    public List<Users> GetList(int pageNumber, int pageSize, string sort, string sortDir)
    {
      return userRepo.GetAll().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
      //return userRepo.GetAll((pageNumber - 1) * pageSize, pageSize, sort, sortDir).ToList();
    }

    public int Count()
    {
      return userRepo.Count();
    }
  }
}
