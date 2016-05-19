using System;
using System.ComponentModel.DataAnnotations;
using Globalstech.Core.Models.BaseEntity;

namespace Modules.News.Models.DataModels {
    public class Relations : BaseEntity {

        public int CategoryID {
            get;
            set;
        }

        public int NewsId {
            get;
            set;
        }

        public bool IsSortTop {
            get;
            set;
        }

        public DateTime? SortTopExpireDate {
            get;
            set;
        }

        [Required(ErrorMessage = "排序不能为空")]
        [Range(0, 9999, ErrorMessage = "排序必须在0至9999之间的数字")]
        [RegularExpression(@"\d+", ErrorMessage = "排序必须是个数字")]
        public int Order {
            get;
            set;
        }
    }
}