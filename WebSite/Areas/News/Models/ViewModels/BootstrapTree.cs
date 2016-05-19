using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Modules.News.Models.ViewModels {
    public class BootstrapTree {

        public string text {
            get;
            set;
        }

        public string href {
            get;
            set;
        }

        public List<BootstrapTree> nodes {
            get;
            set;
        }

    }
}