using System.Data.Entity.ModelConfiguration;
using Modules.News.Models.DataModels;

namespace Modules.News.Models.Mapping {
    public class TestMap : EntityTypeConfiguration<Test> {

        public TestMap() {
            this.ToTable("Test");
            this.HasKey(t => t.ID);
        }

    }
}