using Omu.ValueInjecter;
using OnlineLibrary.Classes;
using OnlineLibrary.Domain.Entities;
using OnlineLibrary.Models;
using OnlineLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OnlineLibrary.Controllers
{
    public class NewAccountController : Controller
    {

    public IUserService service { get; set; }

    public NewAccountController(IUserService service)
    {
      this.service = service;
    }


    public ActionResult LogOff()
    {
      Session["user_name"] = null;

      return RedirectToAction("Index", "Home");
    }
    public ActionResult Login()
    {
      if (TempData["must_login"] != null)
      {
        ViewBag.ErrorMessage = TempData["must_login"].ToString();
      }
      return View();
    }
    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult LogIn(string userName, string password)
    {
      try
      { 
        var logInUser = service.GetAllUsers().Where(s => s.UserName == userName || s.Email == userName).First();
        if (logInUser != null)
        {
          string hashedPassword = Security.HashSHA1(password + logInUser.Salt);
          
          if (logInUser.Password == hashedPassword)
          {
            TempData["userName"] = logInUser.UserName;
            FormsAuthentication.SetAuthCookie(logInUser.UserName, true);
            Session["user_name"] = logInUser.UserName;
            
            Session["user_id"] = logInUser.ID;
            if (logInUser.UserName == "admin")
            {
              Session["RoleName"] = "Admin";
              return RedirectToAction("Index", "DashBoard", new { area = "Admin" });
            }
            else {
              Session["RoleName"] = "Guest";
            }
            return RedirectToAction("Index", "Home");
          }
          ViewBag.ErrorMessage = "Invalid User Name or Password";
          ViewBag.Class = "alert_message";
          return View();
        }
        ViewBag.ErrorMessage = "Invalid User Name or Password";
        ViewBag.Class = "alert_message";
        return View();

      }

      catch
      {
        ViewBag.ErrorMessage = " Error!!! Contact admin!";
        return View();
      }
    }

    public ActionResult Register()
    {
      return View();
    }
    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult Register(CreateUserViewModel model)
    {
      try
      { 
      if (ModelState.IsValid)
      {
        var getUser = service.GetAllUsers().Where(s => s.UserName == model.UserName || s.Email == model.Email).FirstOrDefault();
        if (getUser == null)
        {
          Users newUser = new Users();
          newUser.InjectFrom(model);
          newUser.Salt = Guid.NewGuid().ToString();
          string hashedPassword = Security.HashSHA1(newUser.Password + newUser.Salt);
          newUser.Password = hashedPassword;
          service.CreateUser(newUser);
          return RedirectToAction("LogIn", "NewAccount");
        }




        if (getUser.UserName == model.UserName)
          ViewBag.ErrorMessage = "User Name Already Exists!";
        if (getUser.Email == model.Email)
          ViewBag.ErrorMessage = "Email Already Exists!";
        ViewBag.Class = "alert_message";
        return View();
      }
    }


      catch
      {
        ViewBag.ErrorMessage = "Error. Contact admin!";
        return View();
      }
      return View();
    }
  }
}