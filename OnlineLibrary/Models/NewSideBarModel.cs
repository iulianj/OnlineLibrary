using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineLibrary.Models
{
  public class NewSideBarModel
  {
    public int ID { get; set; }
    public string Authors { get; set; }

    public string Category { get; set; }

    public string Publishing { get; set; }

    public string Year { get; set; }
    
    public string Badge { get; set; }

    
  }
}