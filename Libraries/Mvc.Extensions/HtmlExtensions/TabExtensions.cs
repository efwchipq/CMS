using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Mvc.Extensions.HtmlExtensions {
    public static class TabExtensions {
        public static TabBuilder Tab(this HtmlHelper htmlHelper, string containerId) {
            return new TabBuilder(containerId);
        }

    }
    /// <summary>
    /// 选项卡构建器,不要把逻辑部分直接在extensions完成
    /// </summary>
    public class TabBuilder : IHtmlString {
        private readonly string _containerId;
        private readonly List<TabItem> _options = new List<TabItem>();
        private readonly TabOption _option = new TabOption();
        /// <summary>
        /// 建议只配置选项的外层的容器id,通过js以及索引来切换,不比选项都配置id,太麻烦
        /// </summary>
        /// <param name="containerId"></param>
        public TabBuilder(string containerId) {
            _containerId = containerId;
        }
        /// <summary>
        /// 设置选项卡属性
        /// </summary>
        /// <param name="configure"></param>
        /// <returns></returns>
        public TabBuilder Configure(Action<TabOption> configure) {
            if (configure != null) {
                configure(_option);
            }
            return this;
        }
        /// <summary>
        /// 添加选项(完整)
        /// </summary>
        /// <param name="tabAction"></param>
        /// <returns></returns>
        public TabBuilder AddTab(Action<TabItem> tabAction) {
            var tabItem = new TabItem();
            if (tabAction != null) {
                tabAction(tabItem);
            }
            _options.Add(tabItem);
            return this;
        }
        /// <summary>
        /// 添加选项
        /// </summary>
        /// <param name="title"></param>
        /// <param name="tabId"></param>
        /// <returns></returns>
        public TabBuilder AddTab(string title, string tabId = null) {
            _options.Add(new TabItem() {
                Title = title,
                TabId = tabId
            });
            return this;
        }
        //构建代码
        private IHtmlString Builder() {
            //这里可以进行封装一下
            var stringBuinder = new StringBuilder();
            var textWriter = new StringWriter(stringBuinder);
            var writer = new HtmlTextWriter(textWriter);

            writer.AddAttribute("id", Guid.NewGuid().ToString("N"));
            writer.AddAttribute("class", "TabMenu_OrangeYellow");
            writer.AddAttribute("data-type", "cms-tabselect");
            writer.RenderBeginTag("div");
            writer.RenderBeginTag("ul");
            foreach (var option in _options) {
                writer.RenderBeginTag("li");

                if (!string.IsNullOrEmpty(option.TabId)) {
                    writer.AddAttribute("data-href", "#" + option.TabId);
                }
                if (option.IsSelected) {
                    writer.AddAttribute("class", "currentselect");
                }
                writer.RenderBeginTag("a");
                writer.Write(option.Title);
                writer.RenderEndTag();

                writer.RenderEndTag();
            }
            writer.RenderEndTag();
            writer.RenderEndTag();
            return new MvcHtmlString(stringBuinder.ToString());
        }
        /// <summary>
        /// 实现IHtmlString  在Razor会自动调用此方法
        /// </summary>
        /// <returns></returns>
        public string ToHtmlString() {
            return Builder().ToHtmlString();
        }
    }
    /// <summary>
    /// 选项卡配置
    /// </summary>
    public class TabOption {
        /// <summary>
        /// 触发事件方式
        /// </summary>
        public string TriggerEvent {
            get;
            set;
        }
    }
    /// <summary>
    /// 选项信息
    /// </summary>
    public class TabItem {
        public string Title {
            get;
            set;
        }
        /// <summary>
        /// 这个项做为可选项,未填写的使用索引的位置来判断
        /// </summary>
        public string TabId {
            get;
            set;
        }
        public bool IsSelected {
            get;
            set;
        }
    }
}
