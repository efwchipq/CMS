using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using Modules.News.Models.DataModels;

namespace Modules.News.Models.Mapping {
    public class NewsCategoryMap : EntityTypeConfiguration<NewsCategory> {
        public NewsCategoryMap() {
            this.ToTable("news_Category");
            this.HasKey(t => t.ID);
        }
    }
}