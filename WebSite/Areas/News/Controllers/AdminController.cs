using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.Mvc;
using Common.BaseClass;
using Common.Utils;
using Modules.News.Models.DataModels;
using Modules.News.Models.ViewModels;
using Modules.News.Service.Interface;
using Newtonsoft.Json;

namespace Modules.News.Controllers {


    public class AdminController : Controller {

        private readonly INewsService _service;

        public AdminController(INewsService service)
        {
            _service = service;
        }

        public ActionResult List(int? categoryId)
        {
            ViewBag.Title = "新闻列表";
            //var list = _service.NewsList(categoryId);
            //return View(list);
            return View();
        }

        public ActionResult AjaxList(int? categoryId)
        {
            ViewBag.Title = "新闻列表";
            var draw = Request["draw"];
            var drawInt = ConvertUtils.To(draw, 1);
            var length = Request["length"];
            var lengthInt = ConvertUtils.To(length, 0);
            var start = Request["start"];
            var startInt = ConvertUtils.To(start, 0);
            int count;
            var list = _service.NewsList(categoryId, startInt, lengthInt, out count);
            var data = new DataTables<BasicInformation>() {
                Draw = drawInt,
                RecordsTotal = count,
                RecordsFiltered = count,
                Data = list
            };
            var jsetting = new JsonSerializerSettings {
                DefaultValueHandling = DefaultValueHandling.Ignore
            };
            string jsonStr = JsonConvert.SerializeObject(data, Formatting.Indented, jsetting);
            Thread.Sleep(2000);
            return Content(jsonStr);
        }

        //[ChildActionOnly]//限制只能在本站中显示的局部
        public ActionResult Edit()
        {
            ViewBag.Title = "新闻编辑";
            return View();
        }


        [HttpPost]
        [ValidateInput(false)]//允许提交HTML内容处理
        [ValidateAntiForgeryToken]//防止跨站请求攻击CSRF
        [ActionName("Edit")]//重名处理
        public ActionResult EditArticle()
        {

            var basicInformation = new BasicInformation();
            var relations = new Relations();

            //使用白名单更新实体，防止表单绑定分配漏洞
            var basicInformationWhite = new[] {"PublishTime","ExpireDate","Title","Description","Content","Source","KeyWords","IsExtURL",
                "ExtURL","Author","AuthorFirst","AuthorPhoto","AuthorCompany","Image","AttachFile","Hits","SubTitle"
                //Hits Status  Type
            };
            var relationsWhite = new[] {"IsSortTop","SortTopExpireDate","Order"
                //NewsId 
            };

            var isValid = TryUpdateModel(basicInformation, basicInformationWhite);
            isValid = isValid && TryUpdateModel(relations, relationsWhite);
            if (isValid) {
                _service.AddNews(basicInformation, relations);
                return RedirectToAction("List");
            } else {
                var newsViewWhite = new[] {"PublishTime","ExpireDate","Title","Description","Content","Source","KeyWords","IsExtURL",
                "ExtURL","Author","AuthorFirst","AuthorPhoto","AuthorCompany","Image","AttachFile","Hits","SubTitle",
                "IsSortTop","SortTopExpireDate","Order"
                //Hits Status  Type
            };
                var news = new NewsView();
                isValid = TryUpdateModel(news, newsViewWhite);
                return View(news);//返回带错误信息的模型
            }
            //TryValidateModel(basicInformation);
        }




        [NonAction]
        public string SS()
        {
            var name = EncodeBase64("010000010278");
            var pwd = EncodeBase64("pass123word");
            return "用户名：" + name + "  密码：" + pwd;
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [NonAction]
        public string EncodeBase64(string str)
        {
            byte[] toEncodeAsBytes = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(toEncodeAsBytes);
        }

    }
}