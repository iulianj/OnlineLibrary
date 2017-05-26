using OnlineLibrary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace OnlineLibrary.Models
{
  public class BookViewModel
  {
    public int ID { get; set; }
    public string Title { get; set; }
    
    public string Author { get; set; }

    public string ISBN { get; set; }
    public string Category { get; set; }
    public int Availability { get; set; }
    public string AvailabilityDate { get; set; }
    public string imageURL { get; set; }
    public string Publishing { get; set; }

    public int Year { get; set; }

    public string Description { get; set; }

    public int CategoriesID { get; set; }
    public int PublishingsID { get; set; }
    public int AuthorID { get; set; }

  }
}