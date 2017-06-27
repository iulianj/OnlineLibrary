using OnlineLibrary.Data.Infrastructure;
using OnlineLibrary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Services
{
  public class CategoryService: ICategoryService
  {
    private IRepository<Categories> categoryRepo;
    private IUnitOfWork unitOfWork;

    public CategoryService(IRepository<Categories> categoryRepo, IUnitOfWork unitOfWork)
    {
      this.categoryRepo = categoryRepo;
      this.unitOfWork = unitOfWork;
    }

    public List<Categories> GetAllRecords()
    {
      return categoryRepo.GetAll().ToList();
    }

    public void CreateCategory(Categories category)
    {
      categoryRepo.Add(category);
      unitOfWork.Commit();
    }

    public void EditCategory(Categories category)
    {
      categoryRepo.Update(category);
      unitOfWork.Commit();
    }

    public void DeleteCategory(Categories category)
    {
      categoryRepo.Delete(category);
      unitOfWork.Commit();
    }
  }
}

