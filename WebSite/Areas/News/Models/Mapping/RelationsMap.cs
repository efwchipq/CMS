using System.Data.Entity.ModelConfiguration;
using Modules.News.Models.DataModels;

namespace Modules.News.Models.Mapping {
    public class RelationsMap : EntityTypeConfiguration<Relations> {
        public RelationsMap() {
            this.ToTable("news_Relations");
            this.HasKey(t => t.ID);
        }
    }
}