using OnlineLibrary.Data.Infrastructure;
using OnlineLibrary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Services
{
  public class AuthorService: IAuthorService
  {
      private IRepository<Authors> authorRepo;
      private IUnitOfWork unitOfWork;

      public AuthorService(IRepository<Authors> authorRepo, IUnitOfWork unitOfWork)
      {
        this.authorRepo = authorRepo;
        this.unitOfWork = unitOfWork;
      }

      public List<Authors> GetAllAuthors()
      {
        return authorRepo.GetAll().ToList();
      }

      public void CreateAuthor(Authors author)
      {
        authorRepo.Add(author);
        unitOfWork.Commit();
      }
    }
  }

