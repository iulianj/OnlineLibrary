using OnlineLibrary.Domain.Entities;
using OnlineLibrary.Classes;
using OnlineLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineLibrary.Areas.Admin.Models;

namespace OnlineLibrary.Areas.Admin.Controllers
{
  public class AdminCategoriesController : Controller
  {
    public ICategoryService service { get; set; }

    public AdminCategoriesController(ICategoryService service)
    {
      this.service = service;
    }

    public ActionResult Index()
    {
      return View(service.GetAllRecords());
    }
    [HttpPost]
    public ActionResult AddNew(string category, int? viewReturn)
    {
      Categories newCategory = new Categories();
      newCategory.Category = category.Capitalize();
      if (service.GetAllRecords().Find(c => c.Category == newCategory.Category) == null)
      {
        service.CreateCategory(newCategory);
      }
      else
      {
        ViewBag.ErrorMsg = "Category " + category.Capitalize() + " already exists!";
        ViewBag.ToController = "AdminCategories";
        return View("CategoryExists");
      }
      if (viewReturn == null) return new EmptyResult();
      return RedirectToAction("Index");
    }

    public ActionResult Edit(AdminCategoryModel model)
    {
      try
      {
        if (ModelState.IsValid)
        {
          Categories editCategory = new Categories();
          editCategory.ID = model.categoryID;
          editCategory.Category = model.category.Capitalize();
          service.EditCategory(editCategory);
        }
      }
      catch
      {
        ViewBag.ErrorMsg = "Category " + model.category.Capitalize()+ " not exists!";
        ViewBag.ToController = "AdminCategories";
        return View("NotFound");
      }

      return RedirectToAction("Index");
    }

    public ActionResult Delete(AdminCategoryModel model)
    {
      try
      {
        if (ModelState.IsValid)
        {
          Categories deleteCategory = new Categories();
          deleteCategory = service.GetAllRecords().Find(a => a.ID == model.categoryID);
          service.DeleteCategory(deleteCategory);
        }
      }
      catch
      {
        ViewBag.ErrorMsg = "Category " + model.category.Capitalize() + " not exists!";
        ViewBag.ToController = "AdminCategories";
        return View("NotFound");
      }

      return RedirectToAction("Index");
    }

    public JsonResult PopulateDropDown()
    {

      List<Categories> allCategories = service.GetAllRecords();
      var categories = allCategories.Select(c => new
      {
        CategoryID = c.ID,
        Text = c.Category
      });
      return Json(categories, JsonRequestBehavior.AllowGet);
    }
  }
}