using OnlineLibrary.Domain.Entities;
using OnlineLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineLibrary.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
    public IUserService service { get; set; }

    public UsersController(IUserService service)
    {
      this.service = service;
    }
    public JsonResult PopulateDropDown()
    {

      List<Users> allUsers = service.GetAllUsers();
      var users = allUsers.Select(u => new
      {
        UserID = u.ID,
        Text = u.FirstName + " " + u.LastName
      });
      return Json(users, JsonRequestBehavior.AllowGet);
    }
  }
}