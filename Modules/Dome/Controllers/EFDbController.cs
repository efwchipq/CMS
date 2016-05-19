using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.EntityFramework;

namespace Test.Controllers {
    public class EFDbController : Controller {
        private readonly EFDb service = new EFDb();
        //
        // GET: /EFDb/
        public string Index() {
            var test = new Models.Test() {
                Name = "名称"
            };
            service.Add(test);

            return "";
        }
    }
}