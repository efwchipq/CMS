using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Data.EntityFramework;
using Modules.News.Models.DataModels;
using Modules.News.Models.ViewModels;
using Modules.News.Service.Interface;

namespace Modules.News.Service.Implementations {
    public class NewsCategoryService : INewsCategoryService {

        private readonly IEFDb _efDb;

        public NewsCategoryService(IEFDb efDb)
        {
            _efDb = efDb;
        }

        #region 新闻栏目

        /// <summary>
        /// 新闻栏目列表
        /// </summary>
        public List<NewsCategory> GetNewsCategoryList()
        {
            return _efDb.GetEntities<NewsCategory>(t => t.IsDeleted == false).ToList();
        }

        /// <summary>
        /// 新闻栏目列表
        /// </summary>
        public void AddNewsCategory(NewsCategory category)
        {
            if (category.ParentID == null) {
                category.CategoryPath = category.Name;
            } else {
                var parentCategory = GetCategoryById(category.ParentID.Value);
                if (parentCategory == null) {
                    throw new Exception("未找到父级栏目");
                }
                category.CategoryPath = parentCategory.CategoryPath + ">>" + category.Name;
            }
            _efDb.Add(category);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <returns></returns>
        public NewsCategory GetCategoryById(int id)
        {
            return _efDb.GetEntity<NewsCategory>(id);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <returns></returns>
        public List<NewsCategory> GetCategoryByParentId(int parentId)
        {
            return _efDb.GetEntities<NewsCategory>(t => t.ParentID == parentId).ToList();
        }

        /// <summary>
        /// 获取新闻栏目查询表达式
        /// </summary>
        private IQueryable<NewsCategory> GetNewsCategoryQueryable()
        {
            return _efDb.GetEntities<NewsCategory>(t => t.IsDeleted == false);
        }

        /// <summary>
        /// 查询新闻栏目树形数据
        /// </summary>
        /// <returns></returns>
        public List<BootstrapTree> GetNewsCategoryBootstrap(string url)
        {
            var queryable = GetNewsCategoryQueryable();

            var categoryTree = queryable.Select(t => new NewsCategoryTreeView() {
                ID = t.ID, ParentID = t.ParentID, Name = t.Name
            }).ToList();

            return CreateBootstrapTree(categoryTree, url);
        }

        /// <summary>
        /// 创建BootstrapTree所需树形结构数据
        /// </summary>
        /// <param name="newsCategoryTree">数据</param>
        /// <param name="url"></param>
        /// <param name="bootstrapTrees">节点子节点集合</param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private List<BootstrapTree> CreateBootstrapTree(List<NewsCategoryTreeView> newsCategoryTree, string url, List<BootstrapTree> bootstrapTrees = null, int? parentId = null)
        {
            if (bootstrapTrees == null) {
                bootstrapTrees = new List<BootstrapTree>();
            }
            //筛选父级为指定ID的数据
            foreach (var tree in newsCategoryTree.Where(t => t.ParentID == parentId)) {
                var treeNode = new BootstrapTree() {
                    text = tree.Name + "<input type=\"hidden\" value=\"" + tree.ID + "\"/>",
                    href = url.Replace("categoryIdPlaceholder", tree.ID.ToString()),
                    nodes = new List<BootstrapTree>()
                };
                CreateBootstrapTree(newsCategoryTree, url, treeNode.nodes, tree.ID);
                bootstrapTrees.Add(treeNode);
                if (!treeNode.nodes.Any()) {
                    treeNode.nodes = null;
                }
            }
            return bootstrapTrees;
        }


        #endregion
    }
}