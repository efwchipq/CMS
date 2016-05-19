using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Mvc.Extensions.Clients;
using Mvc.Extensions.Tools;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Mvc.Extensions.HtmlExtensions {
    public static class AjaxGridViewExtensions {

        public static AjaxGridViewBuilder<T> AjaxGridView<T>(this HtmlHelper<IEnumerable<T>> htmlHelper, string tableId, string ajaxUrl, bool serverSide = true) where T : class {
            return new AjaxGridViewBuilder<T>(htmlHelper, tableId, ajaxUrl, serverSide);
        }

    }

    public class AjaxGridViewBuilder<T> : IHtmlString {

        private readonly HtmlHelper _htmlHelper;

        private readonly string _tableId;
        private readonly string _tableClassName;
        private readonly string _divClassName;

        private readonly DataTableConfig _dataTableConfig = new DataTableConfig();

        public AjaxGridViewBuilder(HtmlHelper htmlHelper, string tableId, string ajaxUrl, bool serverSide = true) {
            _htmlHelper = htmlHelper;
            _tableId = tableId;
            _dataTableConfig.AjaxUrl = ajaxUrl;
            _dataTableConfig.ServerSide = serverSide;
        }

        /// <summary>
        /// 添加配置
        /// </summary>
        /// <returns></returns>
        public AjaxGridViewBuilder<T> AddSettings(Action<DataTableConfig> settingAction) {
            if (settingAction != null) {
                settingAction(_dataTableConfig);
            }
            return this;
        }

        /// <summary>
        /// 添加列
        /// </summary>
        /// <returns></returns>
        public AjaxGridViewBuilder<T> AddColumn(Action<GridDataTableColumns> columnAction) {
            var column = new GridDataTableColumns();
            if (columnAction != null) {
                columnAction(column);
            }
            _dataTableConfig.Columns.Add(column);
            return this;
        }

        private string Biuder() {
            var table = new DataTableFactory().CreateDataTable(_dataTableConfig.TableClassName, _tableId);
            //table标签
            var tableStr = table.Render();
            //外层DIV
            var divBuilder = new TagBuilder("div");
            divBuilder.AddCssClass(_dataTableConfig.DivClassName);
            divBuilder.InnerHtml = tableStr;
            return divBuilder.ToString();
        }

        private void RegistScript() {
            //Client.RegistCss("/Scripts/Common/Datatables/jquery.dataTables.css");
            Client.RegistScripts("/Scripts/Common/Datatables/jquery.dataTables.js");
            Client.RegistScripts("/Scripts/Common/Datatables/Extensions/jquery.dataTables.extensions.js");
            var jsetting = new JsonSerializerSettings {
                DefaultValueHandling = DefaultValueHandling.Ignore
            };
            string jsonStr = JsonConvert.SerializeObject(_dataTableConfig, Formatting.Indented, jsetting);
            //Client.RegistScriptBlock("$('#" + _tableId + "').DataTable(" + jsonStr + ");");
            Client.RegistScriptBlock("window.registerDataTable('#" + _tableId + "'," + jsonStr + ");");
        }

        public string ToHtmlString() {
            RegistScript();
            return Biuder();
        }
    }

    //http://www.cnblogs.com/yanweidie/p/4605212.html#_label1 Newtonsoft.Json高级用法
    //http://datatables.net/reference/option/columns.width  datatables  API
    public class DataTableConfig {
        public DataTableConfig() {
            this.Columns = new List<GridDataTableColumns>();
            this.Length = 10;
            this.PagingType = "full_numbers";
        }

        [JsonIgnore]
        public string TableClassName {
            get;
            set;
        }

        [JsonIgnore]
        public string DivClassName {
            get;
            set;
        }

        /// <summary>
        /// 请求次数计数器
        /// </summary>
        [JsonProperty(PropertyName = "draw")]
        public int Draw {
            get;
            set;
        }

        /// <summary>
        /// 分页每页大小
        /// </summary>
        [JsonProperty(PropertyName = "pageLength")]
        public int Length {
            get;
            set;
        }

        /// <summary>
        /// 异步服务器处理路径
        /// </summary>
        [JsonProperty(PropertyName = "ajax")]
        public string AjaxUrl {
            get;
            set;
        }

        /// <summary>
        /// 是否服务器分页
        /// </summary>
        [JsonProperty(PropertyName = "serverSide", DefaultValueHandling = DefaultValueHandling.Include)]
        public bool ServerSide {
            get;
            set;
        }

        /// <summary>
        /// 是否启用搜索功能
        /// </summary>
        [JsonProperty(PropertyName = "searching")]
        [DefaultValue(true)]
        public bool Searching {
            get;
            set;
        }

        /// <summary>
        /// 是否启用分页功能
        /// </summary>
        [JsonProperty(PropertyName = "ordering")]
        [DefaultValue(true)]
        public bool Ordering {
            get;
            set;
        }

        /// <summary>
        /// 是否启用客户端调整分页大小
        /// </summary>
        [JsonProperty(PropertyName = "lengthChange")]
        [DefaultValue(true)]
        public bool LengthChange {
            get;
            set;
        }
        /// <summary>
        /// 分页显示模式
        /// </summary>
        [JsonProperty(PropertyName = "pagingType")]
        [DefaultValue("simple_numbers")]
        public string PagingType {
            get;
            set;
        }

        /// <summary>
        /// Grid列
        /// </summary>
        [JsonProperty(PropertyName = "columns")]
        public List<GridDataTableColumns> Columns {
            get;
            set;
        }

    }

    public class GridDataTableColumns {

        //获取特性默认值
        //AttributeCollection attrColl = TypeDescriptor.GetProperties(new PrintInfo())["UserName"].Attributes;  
        //DefaultValueAttribute attr = attrColl[typeof(DefaultValueAttribute)] as DefaultValueAttribute;  
        //string _Value = attr.Value; 

        public GridDataTableColumns() {
            this.Visible = true;
            this.Width = "100";
        }

        /// <summary>
        /// 数据属性名称
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        [DefaultValue(null)]
        public string Data {
            get;
            set;
        }

        /// <summary>
        /// 默认值，如果有默认值则不绑定数据
        /// </summary>
        [JsonProperty(PropertyName = "defaultContent")]
        [DefaultValue(null)]
        public string DefaultContent {
            get;
            set;
        }

        /// <summary>
        /// 列标题
        /// </summary>
        [JsonProperty(PropertyName = "title")]
        public string Title {
            get;
            set;
        }

        [JsonProperty(PropertyName = "className")]
        [DefaultValue(null)]
        public string ClassName {
            get;
            set;
        }

        /// <summary>
        /// 是否显示
        /// </summary>
        [JsonProperty(PropertyName = "visible")]
        [DefaultValue(true)]
        public bool Visible {
            get;
            set;
        }

        /// <summary>
        /// 列宽，以百分比显示 20%
        /// </summary>
        [JsonProperty(PropertyName = "width")]
        [DefaultValue(true)]
        public string Width {
            get;
            set;
        }


        //public string cellType {
        //    get;
        //    set;
        //}
        //public string className {
        //    get;
        //    set;
        //}
        //public string contentPadding {
        //    get;
        //    set;
        //}
        //public string createdCell {
        //    get;
        //    set;
        //}

        //public string name {
        //    get;
        //    set;
        //}
        //public string orderable {
        //    get;
        //    set;
        //}
        //public string orderData {
        //    get;
        //    set;
        //}
        //public string orderDataType {
        //    get;
        //    set;
        //}
        //public string render {
        //    get;
        //    set;
        //}
        //public string searchable {
        //    get;
        //    set;
        //}

        //public string type {
        //    get;
        //    set;
        //}

    }

}
