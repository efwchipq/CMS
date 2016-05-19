using System;
using System.Collections.Generic;
using System.Linq;
using Data.EntityFramework;
using Modules.News.Models;
using Modules.News.Models.DataModels;
using Modules.News.Service.Interface;

namespace Modules.News.Service.Implementations {
    public class NewsService : INewsService {

        private readonly IEFDb _efDb;

        public NewsService(IEFDb efDb)
        {
            _efDb = efDb;
        }

        /// <summary>
        /// 添加新闻
        /// </summary>
        /// <param name="article"></param>
        /// <param name="relaInfo"></param>
        public void AddNews(BasicInformation article, Relations relaInfo)
        {
            _efDb.Add(article);
            article.Type = NewsType.TextNews;
            relaInfo.NewsId = article.ID;
            _efDb.Add(relaInfo);
        }

        /// <summary>
        /// 新闻列表
        /// </summary>
        public List<BasicInformation> NewsList(int? categoryId, int pageIndex, int pageSize, out int count)
        {
            var newsQuerybale = _efDb.GetEntities<BasicInformation>(t => t.IsDeleted == false);
            List<BasicInformation> newsList;
            if (categoryId == null) {
                count = newsQuerybale.Count();
                newsList = newsQuerybale.OrderBy(t=>t.PublishTime).Skip(pageIndex).Take(pageSize).ToList();
            } else {
                newsQuerybale = newsQuerybale.Where(t => t.CategoryID == categoryId);
                count = newsQuerybale.Count();
                newsList = newsQuerybale.OrderBy(t=>t.PublishTime).Skip(pageIndex).Take(pageSize).ToList();
            }
            return newsList;
        }

    }


}