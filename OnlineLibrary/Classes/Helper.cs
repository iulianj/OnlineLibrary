using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using OnlineLibrary.Models;
using System.Web.Mvc.Ajax;
using OnlineLibrary.Data.Entities;
using System.Globalization;
using OnlineLibrary.Services;
using Omu.ValueInjecter;
using OnlineLibrary.Data.Infrastructure;


namespace OnlineLibrary.myClasses
{
  public static class Helper
  {
    public static byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
    {
      HashAlgorithm algorithm = new SHA256Managed();

      byte[] plainTextWithSaltBytes =
        new byte[plainText.Length + salt.Length];

      for (int i = 0; i < plainText.Length; i++)
      {
        plainTextWithSaltBytes[i] = plainText[i];
      }
      for (int i = 0; i < salt.Length; i++)
      {
        plainTextWithSaltBytes[plainText.Length + i] = salt[i];
      }

      return algorithm.ComputeHash(plainTextWithSaltBytes);
    }

    public static bool CompareByteArrays(byte[] array1, byte[] array2)
    {
      if (array1.Length != array2.Length)
      {
        return false;
      }

      for (int i = 0; i < array1.Length; i++)
      {
        if (array1[i] != array2[i])
        {
          return false;
        }
      }

      return true;
    }

    public static string GenerateSalt(int length)    
    {
      const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
      var randNum = new Random();
      var chars = new char[length];
      var allowedCharCount = allowedChars.Length;
      for (var i = 0; i <= length - 1; i++)
      {
        chars[i] = allowedChars[Convert.ToInt32((allowedChars.Length) * randNum.NextDouble())];
      }
      return new string(chars);
    }



    public static List<NewSideBarModel> GetSideBarElements(string menu)
    {
      
      List<NewSideBarModel> result=new List<NewSideBarModel>();
      

      switch (menu)
      {
        case "categories":
          {
            var service = DependencyResolver.Current.GetService<ICategoryService>();
            var DbList = service.GetAllCategories();
            result.InjectFrom(DbList);
            for (var i = 0; i < result.Count; i++)
            {
              result[i].Badge = DbList[i].Books.Count().ToString();
            }
            result = result.OrderBy(c => c.Category).ToList();
            break;
          }
        case "authors":
          {
            var service = DependencyResolver.Current.GetService<IAuthorService>();
            var DbList = service.GetAllAuthors();
            result.InjectFrom(DbList);
            for (var i = 0; i < result.Count; i++)
            {
              result[i].Authors = DbList[i].FullName;
              result[i].Badge = DbList[i].Books.Count().ToString();
            }
            result = result.OrderBy(c => c.Authors.Split(' ').Last()).ToList();
            break;
          }
        case "publishing":
          {
            var service = DependencyResolver.Current.GetService<IPublishingService>();
            var DbList = service.GetAllPublishings();
            result.InjectFrom(DbList);
            for (var i = 0; i < result.Count; i++)
            {
              result[i].Badge = DbList[i].Books.Count().ToString();
            }
            result = result.OrderBy(c => c.Publishing).ToList();
            break;
          }
        case "year":
          {
            var service = DependencyResolver.Current.GetService<IBookService>();
            var DbList = service.GetAllBooks();
            result.InjectFrom(DbList);
            for (var i = 0; i < result.Count; i++)
            {
              result[i].Year = DbList[i].Year.ToString();
              result[i].Badge = DbList.FindAll(y=>y.Year==DbList[i].Year).Count().ToString();
            }
            result = result.GroupBy(c => c.Year).Select(g=>g.First()).OrderBy(c => c.Year).ToList();
            break;
          }
      }
      return result;
    }

    public static string Capitalize(this string toCapitalize)
    {
      return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(toCapitalize.ToLower());
    }


  }
}