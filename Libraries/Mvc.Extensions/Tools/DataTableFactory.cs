using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Mvc.Extensions.Tools {

    public class DataTableFactory {

        public DataTableFactory() {
        }

        public DataTable _table;

        /// <summary>
        /// 创建表格实例
        /// </summary>
        /// <param name="className"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTableFactory CreateDataTable(string className = "", string id = "") {
            _table = new DataTable();
            if (!string.IsNullOrEmpty(className)) {
                _table.ClassName = className;
            }

            if (!string.IsNullOrEmpty(id)) {
                _table.ID = id;
            }
            return this;
        }

        /// <summary>
        /// 添加行
        /// </summary>
        /// <param name="rowAction"></param>
        /// <returns></returns>
        public DataTableFactory AddDataTableRow(Action<DataTableRow> rowAction = null) {
            var row = new DataTableRow();
            if (rowAction != null) {
                rowAction(row);
            }
            if (_table == null) {
                throw new Exception("请先创建Table实例");
            }
            _table.Rows.Add(row);
            return this;
        }

        /// <summary>
        /// 添加列
        /// </summary>
        /// <param name="rowAction"></param>
        /// <returns></returns>
        public DataTableFactory AddDataTableColumn(Action<DataTableColumn> rowAction) {
            var row = new DataTableColumn();
            if (rowAction != null) {
                rowAction(row);
            }
            if (!_table.Rows.Any()) {
                throw new Exception("没有发现你要添加Column的目标Row");
            }
            _table.Rows.Last().Columns.Add(row);
            return this;
        }

        /// <summary>
        /// 生成Table
        /// </summary>
        /// <returns></returns>
        public string Render() {
            var tableBuilder = new TagBuilder("table");
            if (!string.IsNullOrEmpty(_table.ID)) {
                tableBuilder.GenerateId(_table.ID);
            }
            //添加样式
            if (!string.IsNullOrEmpty(_table.ClassName)) {
                tableBuilder.AddCssClass(_table.ClassName);
            }
            //如果有表头列，添加表头
            if (_table.Rows.Any(t => t.IsThead)) {
                var theadBuilder = new TagBuilder("thead");
                foreach (var row in _table.Rows.Where(t => t.IsThead)) {
                    var rowStr = RenderRow(row);
                    theadBuilder.InnerHtml += rowStr;
                }
                tableBuilder.InnerHtml += theadBuilder;
            }

            //如果有表体列，添加表体
            if (_table.Rows.Any(t => !t.IsThead)) {
                var tbodyBuilder = new TagBuilder("tbody");
                foreach (var row in _table.Rows.Where(t => !t.IsThead)) {
                    var rowStr = RenderRow(row);
                    tbodyBuilder.InnerHtml += rowStr;
                }
                tableBuilder.InnerHtml += tbodyBuilder;
            }

            return tableBuilder.ToString();
        }

        /// <summary>
        /// 创建一行
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private string RenderRow(DataTableRow row) {
            if (!row.Columns.Any()) {
                return "";
            }
            var trBuilder = new TagBuilder("tr");
            var isThead = row.IsThead;
            foreach (var column in row.Columns) {
                var tdBuilder = isThead ? new TagBuilder("th") : new TagBuilder("td");
                tdBuilder.InnerHtml = column.Value;
                //给单元格添加样式名
                if (!string.IsNullOrEmpty(column.ClassName)) {
                    tdBuilder.AddCssClass(column.ClassName);
                }
                trBuilder.InnerHtml += tdBuilder;
            }

            return trBuilder.ToString();
        }

    }

    public class DataTable {

        public DataTable() {
            this.Rows = new List<DataTableRow>();
        }

        public string ID {
            get;
            set;
        }

        public string ClassName {
            get;
            set;
        }

        //public bool HasThead {
        //    get;
        //    set;
        //}

        public List<DataTableRow> Rows {
            get;
            set;
        }
    }

    public class DataTableRow {

        public DataTableRow() {
            this.Columns = new List<DataTableColumn>();
        }

        /// <summary>
        /// 表格的表头Thead
        /// </summary>
        public bool IsThead {
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

        public List<DataTableColumn> Columns {
            get;
            set;
        }
    }

    public class DataTableColumn {
        /// <summary>
        /// 数据值
        /// </summary>
        public string Value {
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

        public string ClassName {
            get;
            set;
        }
    }

}
