using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Mvc.Extensions.Tools;
using Common.Utils.Reflect;

namespace Mvc.Extensions.HtmlExtensions {
    public static class GridViewExtensions {

        public static GridViewBuilder<T> GridView<T>(this HtmlHelper<IEnumerable<T>> htmlHelper) where T : class {
            return new GridViewBuilder<T>(htmlHelper);
        }
    }

    public class GridViewBuilder<T> : IHtmlString {

        public GridViewBuilder(HtmlHelper htmlHelper) {
            _htmlHelper = htmlHelper;
            _id = Guid.NewGuid().ToString("N");
        }

        private readonly HtmlHelper _htmlHelper;
        private readonly string _id;//唯一标识
        private string _dataKeyClassName = "datakeyclassname";//唯一键样式名
        private string _dataKeyPropertyName = "ID";//唯一键绑定数据属性

        //数据源
        private IEnumerable<T> _dataSorce;
        //表格文本列
        private readonly List<GridViewTextColumn> _textColumns = new List<GridViewTextColumn>();
        //表格功能列
        private readonly List<GridViewInputColumn> _inputColumns = new List<GridViewInputColumn>();

        private string _divClass = "table-responsive";

        private string _tableClass = "table table-bordered table-hover";

        /// <summary>
        /// 添加外层DIV样式
        /// </summary>
        /// <param name="divClass">最外层DIV样式</param>
        /// <param name="tableClass">table样式</param>
        /// <returns></returns>
        public GridViewBuilder<T> AddClass(string divClass = null, string tableClass = null) {
            _divClass = divClass ?? _divClass;
            _tableClass = tableClass ?? _tableClass;
            return this;
        }

        /// <summary>
        /// 安装标识数据
        /// </summary>
        /// <param name="className">最外层DIV样式</param>
        /// <param name="dataKey">table样式</param>
        /// <returns></returns>
        public GridViewBuilder<T> InstallDataKey(string className = null, string dataKey = null) {
            _dataKeyClassName = className ?? _dataKeyClassName;
            _dataKeyPropertyName = dataKey ?? _dataKeyPropertyName;
            return this;
        }

        /// <summary>
        /// 添加数据源
        /// </summary>
        /// <param name="dataSorce">泛型实例集合</param>
        /// <returns></returns>
        public GridViewBuilder<T> AddDataSorce(IEnumerable<T> dataSorce) {
            _dataSorce = dataSorce;
            return this;
        }

        /// <summary>
        /// 添加文本列
        /// </summary>
        /// <returns></returns>
        public GridViewBuilder<T> AddTextColumn(Action<GridViewTextColumn> columnAction) {
            var column = new GridViewTextColumn();
            if (columnAction != null) {
                columnAction(column);
            }
            _textColumns.Add(column);
            return this;
        }

        /// <summary>
        /// 添加功能列
        /// </summary>
        /// <param name="columnAction"></param>
        /// <returns></returns>
        public GridViewBuilder<T> AddInputColumn(Action<GridViewInputColumn> columnAction) {
            var column = new GridViewInputColumn();
            if (columnAction != null) {
                columnAction(column);
            }
            _inputColumns.Add(column);
            return this;
        }

        private string Builder() {
            var table = new DataTableFactory().CreateDataTable(_tableClass).AddDataTableRow(t => t.IsThead = false);

            //创建头部行
            table.AddDataTableRow(t => {
                t.IsThead = true;
            });

            //前置功能列
            foreach (var column in _inputColumns.Where(t => t.IsPrepose)) {
                table.AddDataTableColumn(t => {
                    t.Value = column.Title;
                    t.ClassName = column.ClassName;
                });
            }
            //文本列
            foreach (var column in _textColumns) {
                table.AddDataTableColumn(t => {
                    t.Value = column.Title;
                    t.ClassName = column.ClassName;
                });
            }
            //后置功能列
            foreach (var column in _inputColumns.Where(t => !t.IsPrepose)) {
                table.AddDataTableColumn(t => {
                    t.Value = column.Title;
                    t.ClassName = column.ClassName;
                });
            }
            //设置标识数据
            table.AddDataTableColumn(t => {
                t.Value = "";
                t.ClassName = "display_none";
            });

            //创建body
            var list = _dataSorce.ToList();
            foreach (var data in list) {
                table.AddDataTableRow();//创建行

                //前置功能列
                foreach (var column in _inputColumns.Where(t => t.IsPrepose)) {
                    table.AddDataTableColumn(t => {
                        t.Value = column.InputString;
                        t.ClassName = column.ClassName;
                    });
                }
                //文本列
                foreach (var column in _textColumns) {
                    table.AddDataTableColumn(t => {
                        t.Value = ReflectTools.GetValue(data, column.PropertyName, "");
                        t.ClassName = column.ClassName;
                    });
                }
                //后置功能列
                foreach (var column in _inputColumns.Where(t => !t.IsPrepose)) {
                    table.AddDataTableColumn(t => {
                        t.Value = column.InputString;
                        t.ClassName = column.ClassName;
                    });
                }
                //设置标识数据
                table.AddDataTableColumn(t => {
                    t.Value = string.Format("<input class=\"{0}\" type=\"hidden\" value=\"{1}\" />", _dataKeyClassName, ReflectTools.GetValue(data, _dataKeyPropertyName, ""));
                    t.ClassName = "display_none datamember";
                });
            }
            //table标签
            var tableStr = table.Render();

            //外层DIV
            var divBuilder = new TagBuilder("div");
            divBuilder.AddCssClass(_divClass);
            divBuilder.InnerHtml = tableStr;
            return divBuilder.ToString();
        }

        public string ToHtmlString() {

            return Builder();
        }
    }

    public class GridViewTextColumn {

        /// <summary>
        /// 表格标题
        /// </summary>
        public string Title {
            get;
            set;
        }

        /// <summary>
        /// 对象属性名称
        /// </summary>
        public string PropertyName {
            get;
            set;
        }

        public string ClassName {
            get;
            set;
        }

        public int Width {
            get;
            set;
        }

        public int Heigth {
            get;
            set;
        }



    }

    public class GridViewInputColumn {
        /// <summary>
        /// 表格标题
        /// </summary>
        public string Title {
            get;
            set;
        }

        public string ClassName {
            get;
            set;
        }

        public string InputString {
            get;
            set;
        }

        /// <summary>
        /// 是否前置，true:在文本列之前；false:在文本列之后
        /// </summary>
        public bool IsPrepose {
            get;
            set;
        }

        /// <summary>
        /// 是否替换标识数据
        /// </summary>
        public bool IsReplaceDataKeyValue {
            get;
            set;
        }

        /// <summary>
        /// 替换标识数据的占位字符串
        /// </summary>
        public string DataKeyValuePlaceholder {
            get;
            set;
        }

        public int Width {
            get;
            set;
        }

        public int Heigth {
            get;
            set;
        }


    }

    //HTML代码结构
    //    <div class="table-responsive">
    //    <table class="table table-bordered table-hover">
    //        <thead>
    //            <tr>
    //                <th>标题</th>
    //                <th>发布时间</th>
    //                <th>点击量</th>
    //                <th>过期时间</th>
    //            </tr>
    //        </thead>
    //        <tbody>
    //            <tr>
    //                <td>Title</td>
    //                <td>.PublishTime.ToString("yyyy-MM-dd")</td>
    //                <td>.Hits</td>
    //                <td>.ExpireDate.ToString("yyyy-MM-dd")</td>
    //        </tbody>
    //    </table>
    //  </div>


}
