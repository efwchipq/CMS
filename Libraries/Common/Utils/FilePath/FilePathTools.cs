using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Utils.FilePath {
    public class FilePathTools {

        private static string webSiteRoot;

        /// <summary>
        /// 网站磁盘路径
        /// </summary>
        public static string WebSiteRoot {
            get {
                if (string.IsNullOrEmpty(webSiteRoot)) {
                    webSiteRoot = HttpRuntime.AppDomainAppPath;
                    //var ss= AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                }
                return webSiteRoot;
            }
        }

        /// <summary>
        /// 获取文件物理路径
        /// </summary>
        /// <param name="virtualPath">文件虚拟路径（最好是相对于网站根目录）</param>
        /// <param name="isRequest">是否是客户端请求</param>
        /// <returns></returns>
        public static string GetFilePhysicalPath(string virtualPath, bool isRequest = true) {
            //处理文件路径
            virtualPath = virtualPath.Replace(@"\", "/");
            //处理首个/以便拼接
            //virtualPath = virtualPath.StartsWith("/") ? virtualPath.Substring(1) : virtualPath;
            if (isRequest) {
                return HttpContext.Current.Server.MapPath(virtualPath);
            } else {
                //网站根目录
                //var webSiteRoot = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                var webSiteRoot = HttpRuntime.AppDomainAppPath;
                webSiteRoot = webSiteRoot.EndsWith("/") ? webSiteRoot.Substring(0, webSiteRoot.Length) : webSiteRoot;
                return Path.Combine(webSiteRoot, virtualPath);
            }
        }


    }
}
