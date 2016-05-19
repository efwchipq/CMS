using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Test.Models.Mapping {
    public class TestMap : EntityTypeConfiguration<Test> {
        public TestMap() {
            this.ToTable("Test");
            this.HasKey(t => t.ID);
        }
    }
}