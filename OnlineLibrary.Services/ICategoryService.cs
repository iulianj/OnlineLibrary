using OnlineLibrary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Services
{
  public interface ICategoryService
  {
    List<Categories> GetAllCategories();
    void CreateCategory(Categories category);
  }
}
