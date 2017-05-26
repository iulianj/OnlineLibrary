using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineLibrary.Areas.Admin.Models
{
  public class AddBookModel
  {
    public int ID { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string ISBN { get; set; }
    [Required]
    public int Year { get; set; }
    [Required]
    public int NoOfBooks { get; set; }
    [Required]
    public string Description { get; set; }
    public HttpPostedFileBase File { get; set; }


    public int CategoriesID { get; set; }
    public int PublishingsID { get; set; }
    public int AuthorID { get; set; }
  }
}