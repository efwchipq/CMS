using System.Web.Mvc;
using System.Web.Script.Serialization;
using Modules.News.Models.DataModels;
using Modules.News.Models.ViewModels;
using Modules.News.Service.Interface;

namespace Modules.News.Controllers {
    public class CategoryAdminController : Controller {

        private readonly INewsCategoryService _service;

        public CategoryAdminController(INewsCategoryService service) {
            _service = service;
        }

        //
        // GET: /CategoryAdmin/
        public ActionResult Index() {
            return View();
        }

        public ActionResult List() {
            var list = _service.GetNewsCategoryList();
            return View(list);
        }

        public ActionResult Edit(int? id, int? parentId) {
            var categoryView = new NewsCategoryView();
            if (id == null) {
                categoryView.ParentID = parentId;
                return View(categoryView);
            } else {
                var category = _service.GetCategoryById(id.Value);
                categoryView.ID = category.ID;
                categoryView.ParentID = category.ParentID;
                categoryView.Name = category.Name;
                categoryView.DisplayName = category.DisplayName;
                categoryView.Order = category.Order;
                categoryView.Describe = category.Describe;
                return View(categoryView);
            }
        }

        [HttpPost]
        [ValidateInput(false)]//允许提交HTML内容处理
        [ValidateAntiForgeryToken]//防止跨站请求攻击CSRF
        [ActionName("Edit")]//重名处理
        public ActionResult EditCategory() {
            var category = new NewsCategory();
            //使用白名单更新实体，防止表单绑定分配漏洞
            var categoryWhite = new[] {"ParentID","Name","DisplayName","Order","Describe"
            };
            var isValid = TryUpdateModel(category, categoryWhite);
            if (isValid) {
                _service.AddNewsCategory(category);
                return RedirectToAction("List");
            } else {
                var categoryView = new NewsCategoryView();
                isValid = TryUpdateModel(categoryView, categoryWhite);
                return View(categoryView);//返回带错误信息的模型
            }
        }

        /// <summary>
        /// 新闻栏目树形数据
        /// </summary>
        /// <returns></returns>
        public ActionResult CategoryTree() {
            var url = this.Url.Action("AjaxList", "Admin", new {
                Area = "News", categoryId = "categoryIdPlaceholder"
            });
            var tree = _service.GetNewsCategoryBootstrap(url);
            //var js = new JavaScriptSerializer();
            //var json = js.Serialize(tree);
            return Json(tree, JsonRequestBehavior.AllowGet);
        }

    }
}