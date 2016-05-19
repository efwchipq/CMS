using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Common.BaseClass {
    public class DataTables<T> {

        [JsonProperty(PropertyName = "draw")]
        public int Draw {
            get;
            set;
        }

        [JsonProperty(PropertyName = "recordsTotal")]
        public int RecordsTotal {
            get;
            set;
        }

        [JsonProperty(PropertyName = "recordsFiltered")]
        public int RecordsFiltered {
            get;
            set;
        }

        [JsonProperty(PropertyName = "data")]
        public List<T> Data {
            get;
            set;
        }

        [JsonProperty(PropertyName = "error")]
        [DefaultValue(null)]
        public string ErrorMessage {
            get;
            set;
        }
    }
}
