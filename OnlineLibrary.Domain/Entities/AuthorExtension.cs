﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Domain.Entities
{
  public partial class Authors
  {
    public string FullName { get { return this.FirstName + " " + this.LastName; } }
  }
}
