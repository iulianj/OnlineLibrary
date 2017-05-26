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



    public static IQueryable<SideBarModel> GetSideBarElements(string menu)
    {
      BookStoreEntities Context = new BookStoreEntities();
      IQueryable<SideBarModel> result=null;
      
      switch (menu)
      {
        case "categories":
          {
            result = from c in Context.Categories
                     group c by c.Category into cb
                     join b in Context.Books on cb.FirstOrDefault().ID equals b.CategoriesID into cbb
                     select new SideBarModel
                     {
                       ID= cb.FirstOrDefault().ID,
                       Categories = cb.FirstOrDefault().Category,
                       CatBadge = cbb.Where(l => l.CategoriesID == cb.FirstOrDefault().ID).Count().ToString()
                     };
            break;
          }
        case "author":
          {
            result = from a in Context.Authors
                     group a by a.LastName into ab
                     join b in Context.Books on ab.FirstOrDefault().ID equals b.AuthorID into abb
                     select new SideBarModel
                     {
                       ID = ab.FirstOrDefault().ID,
                       Authors = ab.FirstOrDefault().FirstName+" "+ ab.FirstOrDefault().LastName,
                       AuthBadge = abb.Where(l => l.AuthorID == ab.FirstOrDefault().ID).Count().ToString()
                     };
            break;
          }
        case "publishing":
          {
            result = from p in Context.Publishings
                     group p by p.Publishing into pb
                     join b in Context.Books on pb.FirstOrDefault().ID equals b.PublishingsID into pbb
                     select new SideBarModel
                     {
                       ID = pb.FirstOrDefault().ID,
                       Publishings = pb.FirstOrDefault().Publishing,
                       PubBadge = pbb.Where(l => l.PublishingsID == pb.FirstOrDefault().ID).Count().ToString()
                     };
            break;
          }
        case "year":
          {
            result = from b in Context.Books
                     group b by b.Year into yb
                     select new SideBarModel
                     {
                       ID = yb.FirstOrDefault().ID,
                       Year = yb.FirstOrDefault().Year.ToString(),
                       YearBadge = yb.Where(l => l.Year == yb.FirstOrDefault().Year).Count().ToString()
                     };
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