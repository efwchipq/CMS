using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utils {
    public static class HttpTools {

        /// <summary>
        /// 判断资源是否存在,如果是DNS没有指向，返回页面是域名纠错页面。HttpStatusCode状态也为OK
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool UrlIsExist(String url) {
            Uri u = null;
            try {
                u = new Uri(url);
            } catch {
                return false;
            }
            bool isExist = false;
            var r = WebRequest.Create(u) as HttpWebRequest;
            r.Method = "HEAD";
            try {
                var s = r.GetResponse() as HttpWebResponse;
                if (s.StatusCode == HttpStatusCode.OK) {
                    isExist = true;
                }
            } catch (WebException x) {
                try {
                    var httpWebResponse = x.Response as HttpWebResponse;
                    if (httpWebResponse != null) {
                        isExist = (httpWebResponse.StatusCode != HttpStatusCode.NotFound);
                    } else {
                        isExist = false;
                    }
                } catch {
                    isExist = (x.Status == WebExceptionStatus.Success);
                }
            }
            return isExist;
        }

    }
}
