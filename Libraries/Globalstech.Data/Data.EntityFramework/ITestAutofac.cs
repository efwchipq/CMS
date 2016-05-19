using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework {
    public interface ITestAutofac {
        void Add<T>(T article);
    }
}
