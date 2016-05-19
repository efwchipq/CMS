using System.ComponentModel;

namespace Modules.News.Models {


    public enum NewsType {

        [Description("文本新闻")]
        TextNews = 0,
        [Description("图片新闻")]
        PhotosNews = 1,
        [Description("附件新闻")]
        AttachmentNews = 2,
        [Description("视频新闻")]
        VideoNews = 3

    }
}