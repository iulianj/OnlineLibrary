﻿using OnlineLibrary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineLibrary.Areas.Admin.Models
{
  public class ReturnBookModel
  {
    public int ID { get; set; }
    public string UserName { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string LoanDate { get; set; }
  }
}