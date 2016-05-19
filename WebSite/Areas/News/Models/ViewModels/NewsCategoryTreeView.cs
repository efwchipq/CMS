using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Modules.News.Models.ViewModels {
    public class NewsCategoryTreeView {

        public int ID {
            get;
            set;
        }
        public int? ParentID {
            get;
            set;
        }
        public string Name {
            get;
            set;
        }
    }
}