using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Mvc.Extensions.HtmlExtensions {

    //这个方法导致引用程序池死亡
    //    @Html.Action("List", "Admin", new {
    //    Area = "News"
    //}).ToString()

    //    @Url.Action("Edit", "Admin", new {
    //    Area = "News",
    //    CurrentIndex = 1
    //})


    public static class PagerExtensions {
        public static PagerBuilder PagerView(this HtmlHelper htmlHelper) {
            return new PagerBuilder(htmlHelper);
        }
    }

    public class PagerBuilder : IHtmlString {

        private Pager _pager;
        private readonly HtmlHelper _htmlHelper;

        public PagerBuilder(HtmlHelper htmlHelper) {
            _htmlHelper = htmlHelper;
        }

        /// <summary>
        /// 安装分页配置
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public PagerBuilder InstallConfig(Action<Pager> action) {
            _pager = new Pager();
            if (action != null) {
                action(_pager);
            }
            return this;
        }

        private string Builder() {
            if (_pager == null) {
                throw new Exception("尚未安装分页配置");
            }
            var urlHelper = new UrlHelper(_htmlHelper.ViewContext.RequestContext);
            RouteValueDictionary newValues = new RouteValueDictionary(_pager.RouteValues);
            if (!string.IsNullOrEmpty(_pager.AreaName)) {
                newValues.Add("Area", _pager.AreaName);
            }
            newValues.Add("CurrentIndex", _pager.ReplaceNum);
            var url = urlHelper.Action(_pager.ActionName, _pager.ControllerName, newValues);
            var sb = new StringBuilder();
            sb.Append("<div class='i-pager'>");

            //if (_pager.IsShowPageNum) {
            //sb.Append("<span>每页显示条数:<input type='text' value='" + _pager.PageSize + "'  name='pagesize'></span>");
            //}
            if (_pager.HasPreviousPage) {
                sb.Append("<a class='i-pager-first' name='gopage' href=" + url.Replace(_pager.ReplaceNum, "1") + "  page='1' title='" + _pager.FirstPageText + "'><span>" + _pager.FirstPageText + "</span></a>");
                sb.Append("<a class='i-pager-prev' name='gopage' href=" + url.Replace(_pager.ReplaceNum, (_pager.CurrentPageIndex - 1).ToString()) + "  page='" + (_pager.CurrentPageIndex - 1) + "' title='" + _pager.PreviousPageText + "'><span>" + _pager.PreviousPageText + "</span></a>");
            }

            for (int i = _pager.StartPageNum; i <= _pager.EndPageNum; i++) {
                if (i == _pager.CurrentPageIndex) {
                    sb.Append("<a href='javascript:void(0)' name='gopage'   page='" + i +
                              "' class='i-pager-item i-pager-item-active'><span>" + i + "</span></a>");
                } else {
                    sb.Append("<a href=" + url.Replace(_pager.ReplaceNum, i.ToString()) + " name='gopage'   page='" + i +
                              "' class='i-pager-item'><span>" + i + "</span></a>");
                }
            }

            if (_pager.HasNextPage) {
                sb.Append("<a class='i-pager-next' name='gopage' href=" + url.Replace(_pager.ReplaceNum, (_pager.CurrentPageIndex + 1).ToString()) + " page='" + (_pager.CurrentPageIndex + 1) +
                          "' title='" + _pager.NextPageText + "'><span>" + _pager.NextPageText + "</span></a>");
                sb.Append("<a class='i-pager-last' name='gopage' href=" + url.Replace(_pager.ReplaceNum, _pager.PageCount.ToString()) + "  page='" + _pager.PageCount +
                          "' title='" + _pager.LastPageText + "'><span>" + _pager.LastPageText + "</span></a>");
            }

            sb.Append("<span class='i-pager-info'>");

            sb.Append("<span class='i-pager-current'>" + _pager.CurrentPageText + "</span><span class='i-pager-info-c'>" + _pager.CurrentPageIndex + "</span>");
            sb.Append("<span class='i-pager-info-p'>/" + _pager.PageCount + "</span><span class='i-pager-info-t'>" + _pager.CountPageText +
                      _pager.ItemCount + "</span>");
            sb.Append("</span>");
            sb.Append("</div>");

            return sb.ToString();
        }

        public string ToHtmlString() {
            return Builder();
        }

    }

    public class Pager {

        /// <summary>
        /// 控制器方法名称
        /// </summary>
        public string ActionName {
            get;
            set;
        }

        /// <summary>
        /// 控制器名称
        /// </summary>
        public string ControllerName {
            get;
            set;
        }

        /// <summary>
        /// 区域名称
        /// </summary>
        public string AreaName {
            get;
            set;
        }

        /// <summary>
        /// 路由参数
        /// </summary>
        public object RouteValues {
            get;
            set;
        }

        private int _currentPageIndex = 1;
        /// <summary>
        /// 当前第几页
        /// </summary>
        public int CurrentPageIndex {
            get {
                return _currentPageIndex;
            }
            set {
                _currentPageIndex = value;
            }
        }

        private int _pageSize = 10;
        /// <summary>
        /// 每页显示多少条
        /// </summary>
        public int PageSize {
            get {
                return _pageSize;
            }
            set {
                _pageSize = value;
            }
        }

        /// <summary>
        /// 总共多少条记录
        /// </summary>
        public int ItemCount {
            get;
            set;
        }

        private int _pageCountNum = 5;
        /// <summary>
        /// 页码数
        /// </summary>
        public int PageCountNum {
            get {
                return _pageCountNum;
            }
            set {
                _pageCountNum = value;
            }
        }

        #region 程序集保护属性
        /// <summary>
        /// 总共多少页
        /// </summary>
        internal int PageCount {
            get {
                double pageCount = ItemCount / (double)PageSize;
                pageCount = Math.Ceiling(pageCount);
                return (int)pageCount;
            }
        }

        /// <summary>
        /// 是否显示上一页
        /// </summary>
        internal bool HasPreviousPage {
            get {
                return (CurrentPageIndex > 1);
            }
        }

        /// <summary>
        /// 是否显示下一页
        /// </summary>
        internal bool HasNextPage {
            get {
                return (CurrentPageIndex < PageCount);
            }
        }

        /// <summary>
        /// 开始页码数
        /// </summary>
        internal int StartPageNum {
            get {
                return CurrentPageIndex % PageCountNum == 0 ? CurrentPageIndex / PageCountNum * PageCountNum - PageCountNum + 1 : CurrentPageIndex / PageCountNum * PageCountNum + 1;
            }
        }

        /// <summary>
        /// 结束页码数
        /// </summary>
        internal int EndPageNum {
            get {
                return StartPageNum + (PageCountNum - 1) > PageCount ? PageCount : StartPageNum + (PageCountNum - 1);
            }
        }

        /// <summary>
        /// 当前A标签 页码占位符
        /// </summary>
        internal string ReplaceNum {
            get {
                return "PageIndexPlaceholder";
            }
        }

        /// <summary>
        /// 第一页文字描述
        /// </summary>
        internal string FirstPageText {
            get {
                return "首页";
            }
        }

        /// <summary>
        /// 上一页文字描述
        /// </summary>
        internal string PreviousPageText {
            get {
                return "上一页";
            }
        }

        /// <summary>
        /// 下一页文字描述
        /// </summary>
        internal string NextPageText {
            get {
                return "下一页";
            }
        }

        /// <summary>
        /// 末页文字描述
        /// </summary>
        internal string LastPageText {
            get {
                return "末页";
            }
        }

        /// <summary>
        /// 当前页
        /// </summary>
        internal string CurrentPageText {
            get {
                return "第";
            }
        }

        /// <summary>
        /// 总条数
        /// </summary>
        internal string CountPageText {
            get {
                return "总条数:";
            }
        }
        #endregion

    }
}
