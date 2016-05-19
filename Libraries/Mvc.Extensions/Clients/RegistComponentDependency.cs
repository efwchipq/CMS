using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.Extensions.Clients {
    public class RegistComponentDependency {

        /// <summary>
        /// 注册客户端脚本文件依赖，样式文件依赖
        /// </summary>
        /// <param name="componentName"></param>
        public static void RegistComponent(ComponentDependency componentName) {
            switch (componentName) {
                case ComponentDependency.ColorBox:
                    Client.RegistScripts("/Scripts/Common/ColorBox/jquery.colorbox.js");
                    Client.RegistCss("/Scripts/Common/ColorBox/colorbox.css");
                    return;
                default:
                    return;
            }
        }
    }

    public enum ComponentDependency {
        ColorBox = 1
    }
}
