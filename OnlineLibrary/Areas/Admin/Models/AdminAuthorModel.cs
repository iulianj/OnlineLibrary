using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineLibrary.Areas.Admin.Models
{
  public class AdminAuthorModel
  {
    public int authorID { get; set; }
    public string authorFirstName { get; set; }
    public string authorLastName { get; set; }
  }
}