using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Mvc.Extensions.Tools {
    public class HtmlExtensionsTools {

        /// <summary>
        /// 向HTML属性集合添加Class
        /// </summary>
        /// <param name="htmlAttributes"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static IDictionary<string, object> AddClass(object htmlAttributes, string className) {
            if (htmlAttributes == null) {
                var attributes = new Dictionary<string, object>();
                attributes.Add("class", className);
                return attributes;
            } else {
                var attributes = ObjectToHtmlAttributes(htmlAttributes);
                if (attributes.Keys.Contains("class")) {
                    var classNameA = attributes["class"];
                    attributes["class"] = classNameA + " " + className;
                } else {
                    attributes.Add("class", className);
                }
                return attributes;
            }
        }

        /// <summary>
        /// 把object类型的HTML属性集合转换为Dictionary
        /// </summary>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ObjectToHtmlAttributes(object htmlAttributes) {
            var result = new Dictionary<string, object>();
            if (htmlAttributes != null) {
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(htmlAttributes)) {
                    result.Add(property.Name.Replace('_', '-'), property.GetValue(htmlAttributes));
                }
            }
            return result;
        }

    }
}
