using System.Collections.Generic;
using Modules.News.Models.DataModels;

namespace Modules.News.Service.Interface {
    public interface INewsService {

        /// <summary>
        /// 添加新闻
        /// </summary>
        /// <param name="article"></param>
        /// <param name="relaInfo"></param>
        void AddNews(BasicInformation article, Relations relaInfo);

        /// <summary>
        /// 新闻列表
        /// </summary>
        List<BasicInformation> NewsList(int? categoryId,int pageIndex,int pageSize,out int count);

    }


}
