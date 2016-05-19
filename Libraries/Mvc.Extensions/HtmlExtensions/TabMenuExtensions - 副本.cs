using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc.Extensions.Clients;

namespace Mvc.Extensions.HtmlExtensions {
    public static class TabMenuExtensions {

        /// <summary>
        /// 选项卡
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="menus"></param>
        /// <param name="style"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString TabMenu(this HtmlHelper htmlHelper, Global_TabMenu menus, TabMenuStyle style = TabMenuStyle.TabMenu_OrangeYellow, object htmlAttributes = null) {
            if (menus.Options.Count < 2) {
                throw new Exception("选项卡至少包含两个选项");
            }
            string styleString = Enum.GetName(typeof(TabMenuStyle), style);
            return new TabBuilder().TabMenu(menus, styleString, htmlAttributes);
        }

        //选项卡结构
        //<div class="TabMenu_OrangeYellow" data-type="cms-tabselect">
        //    <ul>
        //        <li class="currentselect" hydli="hydli1" data-href="#div1">选项1</li>
        //        <li hydli="hydli2" data-href="#div2">选项2</li>
        //    </ul>
        //</div>
    }

    public class TabBuilder : IHtmlString {

        /// <summary>
        /// 生成选项卡结构
        /// </summary>
        /// <param name="styleName"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public MvcHtmlString TabMenu(Global_TabMenu menus, string styleName, object htmlAttributes = null) {
            //外层DIV
            var divBuilder = new TagBuilder("div");
            divBuilder.AddCssClass(styleName);
            divBuilder.MergeAttribute("data-type", "cms-tabselect", true);
            divBuilder.MergeAttributes(HtmlExtensionsTools.ObjectToHtmlAttributes(htmlAttributes), true);
            var id = Guid.NewGuid().ToString("N");
            //divBuilder.GenerateId();有BUG，有时页面没有生成ID
            divBuilder.MergeAttribute("id", id, true);
            //UL
            var ulBuilder = new TagBuilder("ul");
            var options = menus.Options.ToArray();
            for (int i = 0; i < options.Length; i++) {
                var option = options[i];
                if (string.IsNullOrEmpty(option.TargetSelecter)) {
                    throw new Exception(string.Format("第{0}个选项目标选择器不能为空", i + 1));
                }
                if (string.IsNullOrEmpty(option.Value)) {
                    throw new Exception(string.Format("第{0}个选项值不能为空", i + 1));
                }
                var optionBuilder = new TagBuilder("li");
                optionBuilder.MergeAttribute("data-href", option.TargetSelecter, true);
                optionBuilder.SetInnerText(option.Value);
                optionBuilder.MergeAttributes(option.HtmlAttributes, true);
                ulBuilder.InnerHtml += optionBuilder.ToString();
            }
            divBuilder.InnerHtml = ulBuilder.ToString();
            RegistScript(id, menus);
            return new MvcHtmlString(divBuilder.ToString());
        }

        /// <summary>
        /// 注册脚本
        /// </summary>
        private void RegistScript(string id, Global_TabMenu menus) {
            Client.RegistScripts("/Scripts/Common/Tabselect/tabselect.js");
            var config = string.Format("beginIndex: {0}, tabEvent: \"{1}\", checkMenuClass: \"{2}\", checkContentClass: \"{3}\",menuChildSel: \"{4}\"", menus.BeginIndex, menus.TabEvent, menus.CheckMenuClass, menus.CheckContentClass, menus.MenuChildSel);
            Client.RegistScriptBlock("$(\"#" + id + " ul\").tabselecter({" + config + "});");
        }

        public string ToHtmlString() {
            return "";
        }
    }

    public class Global_TabMenu {

        /// <summary>
        /// 初始化显示选项
        /// </summary>
        public int BeginIndex {
            get;
            set;
        }

        /// <summary>
        /// 选项卡切换事件
        /// </summary>
        public string TabEvent {
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

        public Global_TabMenu() {
            this.BeginIndex = 0;
            this.TabEvent = "click";
            this.CheckMenuClass = "currentselect";
            this.CheckContentClass = "currentselectcontent";
        }

        /// <summary>
        /// 选项集合
        /// </summary>
        public List<TabMenuOption> Options {
            get;
            set;
        }

        /// <summary>
        /// 添加选项
        /// </summary>
        /// <param name="targetSelecter">选项卡目标选择器 jQuery 选择器</param>
        /// <param name="value">选项卡选项值</param>
        /// <param name="htmlAttributes">选项卡选项属性</param>
        /// <returns></returns>
        public Global_TabMenu AddOption(string targetSelecter, string value, object htmlAttributes = null) {
            var option = new TabMenuOption() {
                TargetSelecter = targetSelecter,
                Value = value,
                HtmlAttributes = HtmlExtensionsTools.ObjectToHtmlAttributes(htmlAttributes)
            };
            if (this.Options == null) {
                this.Options = new List<TabMenuOption>();
            }
            this.Options.Add(option);
            return this;
        }

        /// <summary>
        /// 添加配置
        /// </summary>
        /// <param name="beginIndex">初始化显示选项索引</param>
        /// <param name="tabEvent">选项卡切换事件</param>
        /// <param name="checkMenuClass">选中之后菜单栏添加样式名</param>
        /// <param name="checkContentClass">选中之后菜单栏添加样式名</param>
        /// <returns></returns>
        public Global_TabMenu AddConfig(int beginIndex = 0, string tabEvent = "click", string checkMenuClass = "currentselect", string checkContentClass = "currentselectcontent") {
            this.BeginIndex = beginIndex;
            this.TabEvent = tabEvent;
            this.CheckMenuClass = checkMenuClass;
            this.CheckContentClass = checkContentClass;
            return this;
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
        public string Value {
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
        //橙黄色
        TabMenu_OrangeYellow = 0,
        //浅灰色
        TabMenu_LightGray = 1,
    }
}
