using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc.Extensions.Clients;
using Mvc.Extensions.Tools;

namespace Mvc.Extensions.HtmlExtensions {
    public static class TabMenuExtensions {
        public static TabBuilder Tab(this HtmlHelper htmlHelper) {
            return new TabBuilder();
        }

        //选项卡结构
        //<div class="TabMenu_OrangeYellow" data-type="cms-tabselect">
        //    <ul>
        //        <li class="currentselect" hydli="hydli1" data-href="#div1">选项1</li>
        //        <li hydli="hydli2" data-href="#div2">选项2</li>
        //    </ul>
        //</div>
    }

    /// <summary>
    /// 选项卡构建器,不要把逻辑部分直接在extensions完成
    /// </summary>
    public class TabBuilder : IHtmlString {
        
        private readonly List<TabMenuOption> _options = new List<TabMenuOption>();
        private readonly TabMenuConfig _config = new TabMenuConfig();
        private readonly TabMenuAttribute _attribute = new TabMenuAttribute();

        /// <summary>
        /// 建议只配置选项的外层的容器id,通过js以及索引来切换,不必选项都配置id,太麻烦
        /// </summary>
        public TabBuilder() {
        }

        /// <summary>
        /// 设置选项卡配置
        /// </summary>
        /// <param name="configure"></param>
        /// <returns></returns>
        public TabBuilder Configure(Action<TabMenuConfig> configure) {
            if (configure != null) {
                configure(_config);
            }
            return this;
        }

        /// <summary>
        /// 设置选项卡属性
        /// </summary>
        /// <returns></returns>
        public TabBuilder Attribute(Action<TabMenuAttribute> attribute) {
            if (attribute != null) {
                attribute(_attribute);
            }
            return this;
        }

        /// <summary>
        /// 添加选项(lamda表达式)
        /// </summary>
        /// <param name="tabAction"></param>
        /// <returns></returns>
        public TabBuilder AddTab(Action<TabMenuOption> tabAction) {
            var tabMenuOption = new TabMenuOption();
            if (tabAction != null) {
                tabAction(tabMenuOption);
            }
            _options.Add(tabMenuOption);
            return this;
        }

        /// <summary>
        /// 添加选项
        /// </summary>
        /// <param name="title"></param>
        /// <param name="targetSelecter"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public TabBuilder AddTab(string title, string targetSelecter = null, object htmlAttributes = null) {
            _options.Add(new TabMenuOption() {
                Title = title,
                TargetSelecter = targetSelecter,
                HtmlAttributes = HtmlExtensionsTools.ObjectToHtmlAttributes(htmlAttributes)
            });
            return this;
        }

        /// <summary>
        /// 生成选项卡结构
        /// </summary>
        /// <returns></returns>
        private string BuildTabMenu() {
            //外层DIV
            var divBuilder = new TagBuilder("div");
            string styleString = _attribute.Style == TabMenuStyle.Custom ? _attribute.CustomStyle : Enum.GetName(typeof(TabMenuStyle), _attribute.Style);
            divBuilder.AddCssClass(styleString);
            //divBuilder.MergeAttribute("data-type", "cms-tabselect", true);
            divBuilder.MergeAttributes(HtmlExtensionsTools.ObjectToHtmlAttributes(_attribute.HtmlAttributes), true);
            var id = Guid.NewGuid().ToString("N");
            //divBuilder.GenerateId();有BUG，有时页面没有生成ID
            divBuilder.MergeAttribute("id", id, true);
            //UL
            var ulBuilder = new TagBuilder("ul");
            foreach (var option in _options) {
                if (string.IsNullOrEmpty(option.TargetSelecter)) {
                    throw new Exception(string.Format("第{0}个选项目标选择器不能为空", 1));
                }
                if (string.IsNullOrEmpty(option.Title)) {
                    throw new Exception(string.Format("第{0}个选项值不能为空", 1));
                }
                var optionBuilder = new TagBuilder("li");
                optionBuilder.MergeAttribute("data-href", option.TargetSelecter, true);
                optionBuilder.SetInnerText(option.Title);
                optionBuilder.MergeAttributes(option.HtmlAttributes, true);
                ulBuilder.InnerHtml += optionBuilder.ToString();
            }
            divBuilder.InnerHtml = ulBuilder.ToString();
            RegistScript(id);
            return (divBuilder.ToString());
        }

        /// <summary>
        /// 注册脚本
        /// </summary>
        private void RegistScript(string id) {
            Client.RegistScripts("/Scripts/Common/Tabselect/tabselect.js");
            var config = string.Format("beginIndex: {0}, tabEvent: \"{1}\", checkMenuClass: \"{2}\", checkContentClass: \"{3}\",menuChildSel: \"{4}\"", _config.BeginIndex, _config.TriggerEvent, _config.CheckMenuClass, _config.CheckContentClass, _config.MenuChildSel);
            Client.RegistScriptBlock("$(\"#" + id + " ul\").tabselecter({" + config + "});",priority:9999);
        }

        /// <summary>
        /// 实现IHtmlString  在Razor会自动调用此方法
        /// </summary>
        /// <returns></returns>
        public string ToHtmlString() {
            return BuildTabMenu();
        }
    }

    /// <summary>
    /// 选项卡配置
    /// </summary>
    public class TabMenuConfig {

        public TabMenuConfig() {
            this.BeginIndex = 0;
            this.TriggerEvent = "click";
            this.CheckMenuClass = "currentselect";
            this.CheckContentClass = "currentselectcontent";
        }

        /// <summary>
        /// 触发事件方式
        /// </summary>
        public string TriggerEvent {
            get;
            set;
        }

        /// <summary>
        /// 初始化显示选项
        /// </summary>
        public int BeginIndex {
            get;
            set;
        }

        /// <summary>
        /// 选中之后菜单栏添加样式名
        /// </summary>
        public string CheckMenuClass {
            get;
            set;
        }

        /// <summary>
        /// 选中之后菜单栏添加样式名
        /// </summary>
        public string CheckContentClass {
            get;
            set;
        }

        /// <summary>
        /// 菜单栏元素 选择器
        /// </summary>
        public string MenuChildSel = "li";

    }

    public class TabMenuAttribute {

        public TabMenuAttribute() {
            this.Style = TabMenuStyle.TabMenu_OrangeYellow;
        }

        public TabMenuStyle Style {
            get;
            set;
        }

        public string CustomStyle {
            get;
            set;
        }

        /// <summary>
        /// 选项卡选项属性
        /// </summary>
        public object HtmlAttributes {
            get;
            set;
        }
    }

    /// <summary>
    /// 选项卡选项
    /// </summary>
    public class TabMenuOption {

        /// <summary>
        /// 选项卡目标选择器 jQuery 选择器
        /// </summary>
        public string TargetSelecter {
            get;
            set;
        }

        /// <summary>
        /// 选项卡选项值
        /// </summary>
        public string Title {
            get;
            set;
        }

        /// <summary>
        /// 选项卡选项属性
        /// </summary>
        public IDictionary<string, object> HtmlAttributes {
            get;
            set;
        }
    }

    /// <summary>
    /// 选项卡风格
    /// </summary>
    public enum TabMenuStyle {
        Custom = 0,
        //橙黄色
        TabMenu_OrangeYellow = 1,
        //浅灰色
        TabMenu_LightGray = 2
    }

}
