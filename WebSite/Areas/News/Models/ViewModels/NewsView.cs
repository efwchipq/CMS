using System;
using System.ComponentModel.DataAnnotations;

namespace Modules.News.Models.ViewModels {
    public class NewsView {

        //public int CategoryID {
        //    get;
        //    set;
        //}

        public NewsType Type {
            get;
            set;
        }

        [Required(ErrorMessage = "新闻发布时间不能为空")]
        public DateTime PublishTime {
            get;
            set;
        }

        [Required(ErrorMessage = "新闻过期时间不能为空")]
        public DateTime ExpireDate {
            get;
            set;
        }

        [Required(ErrorMessage = "新闻标题不能为空")]
        public string Title {
            get;
            set;
        }

        
        //[UIHint("input")]
        public string Description {
            get;
            set;
        }

        public string Content {
            get;
            set;
        }

        public string Source {
            get;
            set;
        }

        public string KeyWords {
            get;
            set;
        }

        public bool IsExtURL {
            get;
            set;
        }

        public string ExtURL {
            get;
            set;
        }

        [Required(ErrorMessage = "新闻作者不能为空")]
        public string Author {
            get;
            set;
        }

        public string AuthorFirst {
            get;
            set;
        }

        public string AuthorPhoto {
            get;
            set;
        }

        public string AuthorCompany {
            get;
            set;
        }

        public string Image {
            get;
            set;
        }

        public string AttachFile {
            get;
            set;
        }

        public int Hits {
            get;
            set;
        }

        public bool Status {
            get;
            set;
        }

        public string SubTitle {
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