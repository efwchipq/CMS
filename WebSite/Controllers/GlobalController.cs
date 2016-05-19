using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Utils.FilePath;
using WebSite.Models.Service;

namespace WebSite.Controllers {
    public class GlobalController : Controller {

        static string _urlPath = string.Empty;
        public GlobalController() {
            _urlPath = Path.Combine(FilePathTools.WebSiteRoot, @"Files\BatchImgs");

        }

        private static string _tempFile = "/Files/Temp";

        #region 上传附件
        public ActionResult UpLoadProcess(string id, string name, string type, string lastModifiedDate, int size, HttpPostedFileBase file) {
            string filePathName = string.Empty;

            string localPath = Path.Combine(HttpRuntime.AppDomainAppPath, @"Files\BatchImgs");
            if (Request.Files.Count == 0) {
                return Json(new {
                    jsonrpc = 2.0, error = new {
                        code = 102, message = "保存失败"
                    }, id = "id"
                });
            }

            string ex = Path.GetExtension(file.FileName);
            filePathName = Guid.NewGuid().ToString("N") + ex;
            if (!System.IO.Directory.Exists(localPath)) {
                System.IO.Directory.CreateDirectory(localPath);
            }
            file.SaveAs(Path.Combine(localPath, filePathName));

            return Json(new {
                jsonrpc = "2.0",
                id = id,
                originalFileName = filePathName,
                virtualPath = _urlPath + "/" + filePathName
            });

        }
        #endregion

        #region

        public ActionResult BatchImage() {
            return View();
        }

        public ActionResult TempFileUpLoadProcess(string id, string name, string type, string lastModifiedDate, int size, HttpPostedFileBase file) {
            string filePathName = string.Empty;

            string localPath = Path.Combine(HttpRuntime.AppDomainAppPath, @"Files\Temp");
            if (Request.Files.Count == 0) {
                return Json(new {
                    jsonrpc = 2.0, error = new {
                        code = 102, message = "保存失败"
                    }, id = "id"
                });
            }

            string ex = Path.GetExtension(file.FileName);
            filePathName = Guid.NewGuid().ToString("N") + ex;
            if (!System.IO.Directory.Exists(localPath)) {
                System.IO.Directory.CreateDirectory(localPath);
            }
            file.SaveAs(Path.Combine(localPath, filePathName));

            return Json(new {
                jsonrpc = "2.0",
                id = id,
                originalFileName = filePathName,
                virtualPath = _tempFile + "/" + filePathName
            });

        }

        #endregion

        #region 菜单

        /// <summary>
        /// 前台菜单
        /// </summary>
        /// <returns></returns>
        public ActionResult FrontMenus() {
            var service = new HomeService();
            var menus = service.GetMenus("/Files/Menu/FrontPageMenu.xml");
            //查@functions @section  @helper
            return PartialView(menus);
        }

        /// <summary>
        /// 后台菜单
        /// </summary>
        /// <returns></returns>
        public ActionResult BackMenus() {
            var service = new HomeService();
            var menus = service.GetMenus("/Files/Menu/BsckPageMenu.xml");
            //查@functions @section  @helper
            return PartialView(menus);
        }
        #endregion
    }
}