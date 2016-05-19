using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Globalstech.Core.Models.BaseEntity;

namespace Modules.News.Models.DataModels {
    public class NewsCategory : BaseEntity {
        public int? ParentID {
            get;
            set;
        }
        [Required(ErrorMessage = "栏目名称不能为空")]
        public string Name {
            get;
            set;
        }
        public string DisplayName {
            get;
            set;
        }
        [Range(0, 9999, ErrorMessage = "排序必须在0至9999之间的数字")]
        [RegularExpression(@"\d+", ErrorMessage = "排序必须是个数字")]
        [Required(ErrorMessage = "排序不能为空")]
        public int Order {
            get;
            set;
        }
        public string Describe {
            get;
            set;
        }

        public string CategoryPath {
            get;
            set;
        }

    }
}