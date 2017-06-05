using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineLibrary.Models
{
  public class SideBarModel
  {
    public int? ID { get; set; }
    public string Authors { get; set; }

    public string Category { get; set; }

    public string Publishings { get; set; }

    public string Year { get; set; }
    public string CatBadge { get; set; }

    public string AuthBadge { get; set; }
    public string YearBadge { get; set; }
    public string PubBadge { get; set; }

  }
}