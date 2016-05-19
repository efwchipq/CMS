using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules.News.Models.DataModels;
using Modules.News.Models.ViewModels;

namespace Modules.News.Service.Interface {
    public interface INewsCategoryService {

        #region 新闻栏目
        /// <summary>
        /// 新闻栏目列表
        /// </summary>
        List<NewsCategory> GetNewsCategoryList();

        /// <summary>
        /// 新闻栏目列表
        /// </summary>
        void AddNewsCategory(NewsCategory category);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <returns></returns>
        NewsCategory GetCategoryById(int id);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <returns></returns>
        List<NewsCategory> GetCategoryByParentId(int parentId);

        /// <summary>
        /// 查询新闻栏目树形数据
        /// </summary>
        /// <returns></returns>
        List<BootstrapTree> GetNewsCategoryBootstrap(string url);

        #endregion
    }
}
