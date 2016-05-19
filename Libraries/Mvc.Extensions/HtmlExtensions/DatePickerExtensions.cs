using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Mvc.Extensions.Clients;
using Mvc.Extensions.Tools;

namespace Mvc.Extensions.HtmlExtensions {
    public static class DatePickerExtensions {

        /// <summary>
        /// 生成日期选择控件
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="showHHmmss"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString DatePickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null,bool showHHmmss = false ) {
            if (expression == null) {
                throw new Exception("属性表达式不能为空");
            }
            return DatePickerFor(htmlHelper, expression, null, showHHmmss, htmlAttributes, null);
        }

        /// <summary>
        /// 生成时间段格式的日期选择
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="beginExpression"></param>
        /// <param name="endExpression"></param>
        /// <param name="showHHmmss"></param>
        /// <param name="beginHtmlAttributes"></param>
        /// <param name="endHtmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString DatePickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> beginExpression, Expression<Func<TModel, TProperty>> endExpression, bool showHHmmss = false, object beginHtmlAttributes = null, object endHtmlAttributes = null) {
            if (beginExpression == null) {
                throw new Exception("第一个属性表达式不能为空");
            }
            //第一个日期
            var beginExpressionId = ExpressionHelper.GetExpressionText(beginExpression);
            var beginTextBoxHtml = htmlHelper.TextBoxFor(beginExpression, HtmlExtensionsTools.AddClass(beginHtmlAttributes, "Wdate"));
            //是否是单个日期
            var singleDatePicker = endExpression == null;
            var endTextBoxHtml = new MvcHtmlString("");
            string endExpressionId = "";
            if (!singleDatePicker) {
                //第二个日期
                endExpressionId = ExpressionHelper.GetExpressionText(endExpression);
                endTextBoxHtml = htmlHelper.TextBoxFor(endExpression, HtmlExtensionsTools.AddClass(endHtmlAttributes, "Wdate"));
                if (beginExpressionId == endExpressionId) {
                    throw new Exception("开始时间属性不能和结束时间属性相同");
                }
            }
            MvcHtmlString html = singleDatePicker ? beginTextBoxHtml : new MvcHtmlString(beginTextBoxHtml.ToString() + "-" + endTextBoxHtml.ToString());
            RegistScript(singleDatePicker, showHHmmss, beginExpressionId, endExpressionId);
            return html;
        }

        /// <summary>
        /// 注册日期选择脚本
        /// </summary>
        /// <param name="singleDatePicker">是否是选择两个日期（时间段）</param>
        /// <param name="showHHmmss">是否显示时分秒</param>
        /// <param name="beginExpressionId">开始日期文本框ID</param>
        /// <param name="endExpressionId">结束日期文本框ID</param>
        private static void RegistScript(bool singleDatePicker, bool showHHmmss, string beginExpressionId, string endExpressionId) {
            Client.RegistScripts("/Scripts/Common/My97DatePicker/WdatePicker.js");
            if (singleDatePicker) {
                var scriptBlock = showHHmmss ? "document.getElementById('" + beginExpressionId + "').onclick = function() {WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' });};"
                    : "document.getElementById('" + beginExpressionId + "').onclick = function() {WdatePicker();};";
                Client.RegistScriptBlock(scriptBlock);
            } else {
                if (showHHmmss) {
                    var beginScriptBlock = "document.getElementById('" + beginExpressionId + "').onclick = function() {WdatePicker({dateFmt: 'yyyy-MM-dd HH:mm:ss',maxDate:'#F{$dp.$D(\\'" + endExpressionId + "\\',{s:-0});}'});};";
                    var endScriptBlock = "document.getElementById('" + endExpressionId + "').onclick = function() {WdatePicker({dateFmt: 'yyyy-MM-dd HH:mm:ss',minDate:'#F{$dp.$D(\\'" + beginExpressionId + "\\',{s:0});}'});};";
                    Client.RegistScriptBlock(beginScriptBlock + endScriptBlock);
                } else {
                    var beginScriptBlock = "document.getElementById('" + beginExpressionId + "').onclick = function() {WdatePicker({maxDate:'#F{$dp.$D(\\'" + endExpressionId + "\\',{d:-0});}'});};";
                    var endScriptBlock = "document.getElementById('" + endExpressionId + "').onclick = function() {WdatePicker({minDate:'#F{$dp.$D(\\'" + beginExpressionId + "\\',{d:0});}'});};";
                    Client.RegistScriptBlock(beginScriptBlock + endScriptBlock);
                }
            }
        }
    }
}
