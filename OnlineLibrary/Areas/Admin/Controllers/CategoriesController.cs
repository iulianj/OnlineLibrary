using OnlineLibrary.Domain.Entities;
using OnlineLibrary.myClasses;
using OnlineLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineLibrary.Areas.Admin.Controllers
{
    public class CategoriesController : Controller
    {
    public ICategoryService service { get; set; }

    public CategoriesController(ICategoryService service)
    {
      this.service = service;
    }
    [HttpPost]
    public ActionResult Addnew(string category)
    {
      Categories newCategory = new Categories();
      newCategory.Category = category.Capitalize();
      if (service.GetAllCategories().Find(c => c.Category == newCategory.Category) == null)
        service.CreateCategory(newCategory);

      return new EmptyResult();
    }

    public JsonResult populateDropDown()
    {

      List<Categories> allCategories = service.GetAllCategories();
      var categories = allCategories.Select(c => new
      {
        CategoryID = c.ID,
        Text = c.Category
      });
      return Json(categories, JsonRequestBehavior.AllowGet);
    }
  }
}