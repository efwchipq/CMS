using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Mvc.Extensions.Clients {

    public static class Client {

        private class ClientResource {
            public ClientType ClientType {
                get;
                set;
            }
            public int Priority {
                get;
                set;
            }
            public string Url {
                get;
                set;
            }
        }

        private class ClientScriptBlockResource {

            public string ScriptBlock {
                get;
                set;
            }
            public bool IsWaitReadyComplete {
                get;
                set;
            }
            public int Priority {
                get;
                set;
            }
        }

        public enum ClientType {
            Css,
            JavaScript
        }

        private static readonly List<ClientResource> ClientResources = new List<ClientResource>();

        private static readonly List<ClientScriptBlockResource> ClientScriptBlocks = new List<ClientScriptBlockResource>();

        /// <summary>
        /// 注册脚本文件
        /// </summary>
        /// <param name="resUrl"></param>
        /// <param name="priority"></param>
        public static void RegistScripts(string resUrl, int priority = 100) {
            if (ClientResources.Any(t => t.ClientType == ClientType.JavaScript && t.Url == resUrl)) {
                return;
            }
            ClientResources.Add(new ClientResource() {
                ClientType = ClientType.JavaScript, Priority = priority, Url = resUrl
            });
        }

        /// <summary>
        /// 注册CSS文件
        /// </summary>
        /// <param name="resUrl"></param>
        /// <param name="priority"></param>
        public static void RegistCss(string resUrl, int priority = 100) {
            if (ClientResources.Any(t => t.ClientType == ClientType.Css && t.Url == resUrl)) {
                return;
            }
            ClientResources.Add(new ClientResource() {
                ClientType = ClientType.Css, Priority = priority, Url = resUrl
            });
        }

        /// <summary>
        /// 注册脚本块
        /// </summary>
        /// <param name="scriptBlock">脚本块</param>
        /// <param name="isWaitReadyComplete">是否等待文档准备完成</param>
        /// <param name="priority">注册顺序(0-9999)</param>
        public static void RegistScriptBlock(string scriptBlock, bool isWaitReadyComplete = true, int priority = 100) {
            ClientScriptBlocks.Add(new ClientScriptBlockResource() {
                ScriptBlock = scriptBlock, IsWaitReadyComplete = isWaitReadyComplete, Priority = priority
            });
        }

        /// <summary>
        /// 清除
        /// </summary>
        public static void Clear() {
            ClientResources.Clear();
        }

        /// <summary>
        /// 呈现注册脚本文件
        /// </summary>
        /// <returns></returns>
        public static MvcHtmlString RenderRegistScripts() {
            var scriptRegistStr = "";
            var scripts = ClientResources.Where(t => t.ClientType == ClientType.JavaScript).OrderBy(t => t.Priority);
            foreach (var resource in scripts) {
                var scriptBuilder = new TagBuilder("script");
                scriptBuilder.MergeAttribute("src", resource.Url);
                scriptRegistStr += scriptBuilder.ToString();
            }
            ClientResources.RemoveAll(t => t.ClientType == ClientType.JavaScript);
            return new MvcHtmlString(scriptRegistStr);
        }

        /// <summary>
        /// 呈现注册CSS文件
        /// </summary>
        /// <returns></returns>
        public static HtmlString RenderRegistCss() {
            var cssRegistStr = "";
            var csses = ClientResources.Where(t => t.ClientType == ClientType.Css).OrderBy(t => t.Priority);
            foreach (var resource in csses) {
                var scriptBuilder = new TagBuilder("link");
                scriptBuilder.MergeAttribute("rel", "stylesheet");
                scriptBuilder.MergeAttribute("href", resource.Url);
                cssRegistStr += scriptBuilder.ToString();
            }
            ClientResources.RemoveAll(t => t.ClientType == ClientType.Css);
            return new MvcHtmlString(cssRegistStr);
        }

        /// <summary>
        /// 输出脚本块
        /// </summary>
        /// <returns></returns>
        public static MvcHtmlString RenderScriptBlocks() {
            if (ClientScriptBlocks.Count == 0) {
                return null;
            }
            var scriptBuilder = new TagBuilder("script");
            scriptBuilder.MergeAttribute("type", "text/javascript");
            var beforeReadyScripts = ClientScriptBlocks.Where(t => t.IsWaitReadyComplete == false).OrderBy(t => t.Priority).ToList();
            var afterReadyScripts = ClientScriptBlocks.Where(t => t.IsWaitReadyComplete == true).OrderBy(t => t.Priority).ToList();
            var beforeReadyScriptsStr = string.Join(";", beforeReadyScripts.Select(t => t.ScriptBlock));
            var afterReadyScriptsStr = "$(function() { " + string.Join(";", afterReadyScripts.Select(t => t.ScriptBlock)) + "  });";
            ClientScriptBlocks.Clear();
            scriptBuilder.InnerHtml = (beforeReadyScriptsStr + ";" + afterReadyScriptsStr);
            return new MvcHtmlString(scriptBuilder.ToString());
        }

    }
}
