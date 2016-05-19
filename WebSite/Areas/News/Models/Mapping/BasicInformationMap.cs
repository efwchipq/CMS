using System.Data.Entity.ModelConfiguration;
using Modules.News.Models.DataModels;

namespace Modules.News.Models.Mapping {
    public class BasicInformationMap : EntityTypeConfiguration<BasicInformation> {
        public BasicInformationMap() {
            this.ToTable("news_BasicInformation");
            this.HasKey(t => t.ID);
        }
    }
}