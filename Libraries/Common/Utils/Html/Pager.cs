using System;
using System.Linq;
using System.Text;

namespace Common.Utils.Html {
    public class Pager {
        public Pager(int currentPageIndex, int totalItemCount, string updatePanle = "", int pagesize = 20, string pageurl = "javascript:void(0);", string lang = "zh-cn", int pagecount = 10, bool isShowPageNum = false) {
            TotalItemCount = totalItemCount;
            PageSize = pagesize;
            CurrentPageIndex = currentPageIndex > TotalPageCount ? 1 : currentPageIndex;
            UpdatePanle = updatePanle;
            PageUrl = pageurl;
            Lang = lang;
            PageCountNum = pagecount;
            IsShowPageNum = isShowPageNum;
        }

        /// <summary>
        /// 语言
        /// </summary>
        private string Lang {
            get;
            set;
        }

        /// <summary>
        /// 默认是ajax 的  这里是a标签连接显示的url
        /// </summary>
        public string PageUrl {
            get;
            set;
        }

        /// <summary>
        /// 当前第几页
        /// </summary>
        public int CurrentPageIndex {
            get;
            set;
        }

        /// <summary>
        /// 每页显示多少条
        /// </summary>
        public int PageSize {
            get;
            set;
        }

        /// <summary>
        /// 总共多少条记录
        /// </summary>
        public int TotalItemCount {
            get;
            set;
        }

        /// <summary>
        /// 总共多少页
        /// </summary>
        public int TotalPageCount {
            get {
                double pageCount = TotalItemCount / (double)PageSize;
                pageCount = Math.Ceiling(pageCount);
                return (int)pageCount;
            }
        }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow {
            get {
                return TotalPageCount > 1;
            }
        }

        /// <summary>
        /// 是否显示上一页
        /// </summary>
        public bool HasPreviousPage {
            get {
                return (CurrentPageIndex > 1);
            }
        }

        /// <summary>
        /// 是否显示下一页
        /// </summary>
        public bool HasNextPage {
            get {
                return (CurrentPageIndex < TotalPageCount);
            }
        }

        /// <summary>
        /// 页码数
        /// </summary>
        public int PageCountNum {
            get;
            set;
        }

        /// <summary>
        /// 开始页码数
        /// </summary>
        public int StartPageNum {
            get {
                return CurrentPageIndex % PageCountNum == 0 ? CurrentPageIndex / PageCountNum * PageCountNum - PageCountNum + 1 : CurrentPageIndex / PageCountNum * PageCountNum + 1;
            }
        }

        /// <summary>
        /// 结束页码数
        /// </summary>
        public int EndPageNum {
            get {
                return StartPageNum + (PageCountNum - 1) > TotalPageCount ? TotalPageCount : StartPageNum + (PageCountNum - 1);
            }
        }

        /// <summary>
        /// 页码数组
        /// </summary>
        public int[] Pagenum {
            get {
                return Enumerable.Range(StartPageNum, EndPageNum - StartPageNum + 1).ToArray();
            }
        }

        public string ReplaceNum {
            get {
                return "999999";
            }
        }

        /// <summary>
        /// 第一页文字描述
        /// </summary>
        public string FirstPageText {
            get {
                if (Lang == "zh-cn") {
                    return "首页";
                }
                if (Lang == "en") {
                    return "First";
                }
                return "";
            }
        }

        /// <summary>
        /// 上一页文字描述
        /// </summary>
        public string PreviousPageText {
            get {
                if (Lang == "zh-cn") {
                    return "上一页";
                }
                if (Lang == "en") {
                    return "Previous";
                }
                return "";
            }
        }

        /// <summary>
        /// 下一页文字描述
        /// </summary>
        public string NextPageText {
            get {
                if (Lang == "zh-cn") {
                    return "下一页";
                }
                if (Lang == "en") {
                    return "Next";
                }
                return "";
            }
        }


        /// <summary>
        /// 末页文字描述
        /// </summary>
        public string LastPageText {
            get {
                if (Lang == "zh-cn") {
                    return "末页";
                }
                if (Lang == "en") {
                    return "Last";
                }
                return "";
            }
        }

        /// <summary>
        /// 当前页
        /// </summary>
        public string CurrentPageText {
            get {
                if (Lang == "zh-cn") {
                    return "第";
                }
                if (Lang == "en") {
                    return "Current:";
                }
                return "";
            }
        }
        /// <summary>
        /// 总条数
        /// </summary>
        public string CountPageText {
            get {
                if (Lang == "zh-cn") {
                    return "总条数:";
                }
                if (Lang == "en") {
                    return "Total:";
                }
                return "";
            }
        }

        /// <summary>
        /// 是否展示分页数
        /// </summary>
        public bool IsShowPageNum {
            get;
            set;
        }

        /// <summary>
        /// AJAX更新的区域ID
        /// </summary>
        public string UpdatePanle {
            get;
            set;
        }


        public override string ToString() {
            var sb = new StringBuilder();
            sb.Append("<div class='i-pager'>");

            if (IsShow) {
                if (IsShowPageNum) {
                    sb.Append("<span>每页显示条数:<input type='text' value='" + PageSize + "'  name='pagesize'></span>");
                }
                if (HasPreviousPage) {
                    sb.Append("<a class='i-pager-first' name='gopage' href=" + PageUrl.Replace(ReplaceNum, "1") + "  page='1' title='" + FirstPageText + "'><span>" + FirstPageText + "</span></a>");
                    sb.Append("<a class='i-pager-prev' name='gopage' href=" + PageUrl.Replace(ReplaceNum, (CurrentPageIndex - 1).ToString()) + "  page='" + (CurrentPageIndex - 1) + "' title='" + PreviousPageText + "'><span>" + PreviousPageText + "</span></a>");
                }

                for (int i = StartPageNum; i <= EndPageNum; i++) {
                    if (i == CurrentPageIndex) {
                        sb.Append("<a href='javascript:void(0)' name='gopage'   page='" + i +
                                  "' class='i-pager-item i-pager-item-active'><span>" + i + "</span></a>");
                    } else {
                        sb.Append("<a href=" + PageUrl.Replace(ReplaceNum, i.ToString()) + " name='gopage'   page='" + i +
                                  "' class='i-pager-item'><span>" + i + "</span></a>");
                    }
                }

                if (HasNextPage) {
                    sb.Append("<a class='i-pager-next' name='gopage' href=" + PageUrl.Replace(ReplaceNum, (CurrentPageIndex + 1).ToString()) + " page='" + (CurrentPageIndex + 1) +
                              "' title='" + NextPageText + "'><span>" + NextPageText + "</span></a>");
                    sb.Append("<a class='i-pager-last' name='gopage' href=" + PageUrl.Replace(ReplaceNum, TotalPageCount.ToString()) + "  page='" + TotalPageCount +
                              "' title='" + LastPageText + "'><span>" + LastPageText + "</span></a>");
                }

                sb.Append("<span class='i-pager-info'>");

                sb.Append("<span class='i-pager-current'>" + CurrentPageText + "</span><span class='i-pager-info-c'>" + CurrentPageIndex + "</span>");
                sb.Append("<span class='i-pager-info-p'>/" + TotalPageCount + "</span><span class='i-pager-info-t'>" + CountPageText +
                          TotalItemCount + "</span>");
                sb.Append("</span>");
            }

            sb.Append("</div>");

            return sb.ToString();
        }
    }
}