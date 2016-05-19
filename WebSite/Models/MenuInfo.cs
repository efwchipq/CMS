using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebSite.Models {
    public class MenuInfo {
        public Guid ID {
            get;
            set;
        }

        public Guid ParentID {
            get;
            set;
        }

        public int Order {
            get;
            set;
        }

        public string AreasName {
            get;
            set;
        }
        public string ControllerName {
            get;
            set;
        }

        public string ActionName {
            get;
            set;
        }

        public string TabName {
            get;
            set;
        }

        public string TabImg {
            get;
            set;
        }
    }
}