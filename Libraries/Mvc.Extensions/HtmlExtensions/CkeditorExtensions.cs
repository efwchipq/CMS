using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Mvc.Extensions.Clients;
using Mvc.Extensions.Tools;

namespace Mvc.Extensions.HtmlExtensions {
    public static class CkeditorExtensions {

        public static MvcHtmlString TextEditorFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, bool useDefaultStyle=true, object htmlAttributes = null) {
            var id = ExpressionHelper.GetExpressionText(expression);
            var textArea = htmlHelper.TextAreaFor(expression);
            var divBuilder = new TagBuilder("div");
            divBuilder.MergeAttributes(HtmlExtensionsTools.ObjectToHtmlAttributes(htmlAttributes), true);
            if (useDefaultStyle) {
                divBuilder.AddCssClass("ckeditor");
            }
            divBuilder.InnerHtml=textArea.ToString();
            RegistScript(id);
            return new MvcHtmlString(divBuilder.ToString());
        }

        private static void RegistScript(string name) {
            Client.RegistScripts("/Scripts/Common/ckeditor/ckeditor.js");
            Client.RegistScriptBlock("CKEDITOR.replace('" + name + "');");
        }
    }
}
