using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Data.EntityFramework.Entity;

namespace Test.Models {
    public class Test :EFEntity{
        public string Name { get; set; }
    }
}