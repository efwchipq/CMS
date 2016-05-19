using System;
using System.ComponentModel.DataAnnotations;
using Globalstech.Core.Models.BaseEntity;

namespace Modules.News.Models.DataModels {
    public class BasicInformation : BaseEntity {

        public int CategoryID {
            get;
            set;
        }

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

    }
}