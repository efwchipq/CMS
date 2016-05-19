using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.UI.WebControls;
using Mvc.Extensions.Clients;

namespace Mvc.Extensions.HtmlExtensions {
    public static class WebUploaderExtensions {

        public static MvcHtmlString WebUploader(this HtmlHelper htmlHelper, string name, Global_WebUploader webuploader = null) {
            if (webuploader == null) {
                webuploader = new Global_WebUploader();
            }
            //对象属性名称
            var webUploaderId = name + "WebUploaderId";
            var hideHtml = htmlHelper.Hidden(name);
            //最外层DIV
            var divBuilder = new TagBuilder("div");
            divBuilder.GenerateId(webUploaderId);
            divBuilder.AddCssClass(webuploader.Style);
            //按钮外层区域
            var buttonareaDivBuilder = new TagBuilder("div");
            buttonareaDivBuilder.AddCssClass("uploader-style-buttonarea");
            //上传按钮
            var buttonareaUploadBuilder = new TagBuilder("div");
            buttonareaUploadBuilder.AddCssClass("uploader-style-buttonarea-upload");
            buttonareaUploadBuilder.AddCssClass("WebUploader-upload");
            buttonareaUploadBuilder.SetInnerText(webuploader.UploadName);
            //选择图片按钮
            var buttonareaFilePickerBuilder = new TagBuilder("div");
            buttonareaFilePickerBuilder.AddCssClass("uploader-style-buttonarea-filePicker");
            buttonareaFilePickerBuilder.AddCssClass("WebUploader-filePicker");
            buttonareaFilePickerBuilder.SetInnerText(webuploader.FilePickerName);
            //缩略图展示区域外层
            var buttonareaFileListBuilder = new TagBuilder("div");
            buttonareaFileListBuilder.AddCssClass("uploader-style-fileList");
            buttonareaFileListBuilder.AddCssClass("WebUploader-fileList");

            //组装结构
            buttonareaDivBuilder.InnerHtml = (buttonareaUploadBuilder.ToString() + buttonareaFilePickerBuilder.ToString());
            divBuilder.InnerHtml = hideHtml + buttonareaDivBuilder.ToString() + buttonareaFileListBuilder.ToString();

            //注册样式文件、脚本文件和注册事件
            Client.RegistCss("/Scripts/Common/Upload/webuploader.css");
            Client.RegistScripts("/Scripts/Common/Upload/webuploader.js");
            Client.RegistScripts("/Scripts/Common/Upload/Extensions/webuploaderextensions.js", priority: 101);
            var defaultWebuploader = new Global_WebUploader();
            var options = "" +
                (defaultWebuploader.Width != webuploader.Width ? ("width:" + webuploader.Width) : "")
                + (defaultWebuploader.Height != webuploader.Height ? ("height:" + webuploader.Height) : "")
                + (webuploader.AutoUpload ? ("autoUpload:" + webuploader.AutoUpload.ToString().ToLower()) : "")
                + (webuploader.IsImage ? "" : ("isImage:" + webuploader.IsImage.ToString().ToLower()))
                + (defaultWebuploader.FileNumLimit != webuploader.FileNumLimit ? ("fileNumLimit:" + webuploader.FileNumLimit) : "")
                + (defaultWebuploader.FileSizeLimit != webuploader.FileSizeLimit ? ("fileSizeLimit:" + webuploader.FileSizeLimit) : "")
                + (defaultWebuploader.FileSingleSizeLimit != webuploader.FileSingleSizeLimit ? ("fileSingleSizeLimit:" + webuploader.FileSingleSizeLimit) : "")
                + (",server:" + "'"+webuploader.Server+"'");
            Client.RegistScriptBlock("window.registerWebUploader('#" + webUploaderId + "', { " + options + " });");
            return new MvcHtmlString(divBuilder.ToString());
        }

        public static MvcHtmlString WebUploaderFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Global_WebUploader webuploader = null) {
            if (webuploader == null) {
                webuploader = new Global_WebUploader();
            }
            //对象属性名称
            var name = ExpressionHelper.GetExpressionText(expression);
            var webUploaderId = name + "WebUploaderId";
            var hideHtml = htmlHelper.HiddenFor(expression, (IDictionary<string, object>)null);
            //最外层DIV
            var divBuilder = new TagBuilder("div");
            divBuilder.GenerateId(webUploaderId);
            divBuilder.AddCssClass(webuploader.Style);
            //按钮外层区域
            var buttonareaDivBuilder = new TagBuilder("div");
            buttonareaDivBuilder.AddCssClass("uploader-style-buttonarea");
            //上传按钮
            var buttonareaUploadBuilder = new TagBuilder("div");
            buttonareaUploadBuilder.AddCssClass("uploader-style-buttonarea-upload");
            buttonareaUploadBuilder.AddCssClass("WebUploader-upload");
            buttonareaUploadBuilder.SetInnerText(webuploader.UploadName);
            //选择图片按钮
            var buttonareaFilePickerBuilder = new TagBuilder("div");
            buttonareaFilePickerBuilder.AddCssClass("uploader-style-buttonarea-filePicker");
            buttonareaFilePickerBuilder.AddCssClass("WebUploader-filePicker");
            buttonareaFilePickerBuilder.SetInnerText(webuploader.FilePickerName);
            //缩略图展示区域外层
            var buttonareaFileListBuilder = new TagBuilder("div");
            buttonareaFileListBuilder.AddCssClass("uploader-style-fileList");
            buttonareaFileListBuilder.AddCssClass("WebUploader-fileList");

            //组装结构
            buttonareaDivBuilder.InnerHtml = (buttonareaUploadBuilder.ToString() + buttonareaFilePickerBuilder.ToString());
            divBuilder.InnerHtml = hideHtml + buttonareaDivBuilder.ToString() + buttonareaFileListBuilder.ToString();

            //注册样式文件、脚本文件和注册事件
            Client.RegistCss("/Scripts/Common/Upload/webuploader.css");
            Client.RegistScripts("/Scripts/Common/Upload/webuploader.js");
            Client.RegistScripts("/Scripts/Common/Upload/Extensions/webuploaderextensions.js", priority: 101);
            var defaultWebuploader = new Global_WebUploader();
            var options = "" +
                (defaultWebuploader.Width != webuploader.Width ? ("width:" + webuploader.Width) : "")
                + (defaultWebuploader.Height != webuploader.Height ? ("height:" + webuploader.Height) : "")
                + (webuploader.AutoUpload ? ("autoUpload:" + webuploader.AutoUpload.ToString().ToLower()) : "")
                + (webuploader.IsImage ? "" : ("isImage:" + webuploader.IsImage.ToString().ToLower()))
                + (defaultWebuploader.FileNumLimit != webuploader.FileNumLimit ? ("fileNumLimit:" + webuploader.FileNumLimit) : "")
                + (defaultWebuploader.FileSizeLimit != webuploader.FileSizeLimit ? ("fileSizeLimit:" + webuploader.FileSizeLimit) : "")
                + (defaultWebuploader.FileSingleSizeLimit != webuploader.FileSingleSizeLimit ? ("fileSingleSizeLimit:" + webuploader.FileSingleSizeLimit) : "")
                + ("server:" + "'"+webuploader.Server+"'");
            Client.RegistScriptBlock("window.registerWebUploader('#" + webUploaderId + "', { " + options + " });");
            return new MvcHtmlString(divBuilder.ToString());

            //特性操作
            // // Gets the attributes for the property.
            //AttributeCollection attributes = 
            //   TypeDescriptor.GetProperties(this)["MyProperty"].Attributes;
            // Prints the default value by retrieving the DefaultValueAttribute 
            //  from the AttributeCollection. /
            //DefaultValueAttribute myAttribute = 
            //   (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)];
            //Console.WriteLine("The default value is: " + myAttribute.Value.ToString());
        }


    }

    //    <div id="uploaderextensions" class="uploaderextensions-style">
    //    <input type="hidden" />
    //    <div class="uploaderextensions-style-buttonarea">
    //        <div class="WebUploader-upload uploaderextensions-style-upload">上传</div>
    //        <div class="WebUploader-filePicker uploaderextensions-style-filePicker">选择图片</div>
    //    </div>
    //    <!--用来存放item-->
    //    <div class="WebUploader-fileList"></div>
    //    <script type="text/javascript">
    //        $(function () {
    //            window.registerWebUploader("#uploaderextensions", {  });
    //        });
    //    </script>
    //   </div>

    public class Global_WebUploader {

        [DefaultValue("uploaderextensions-single-preview")]//单个文件预览
        public string Style {
            get;
            set;
        }

        [DefaultValue("上传")]
        public string UploadName {
            get;
            set;
        }

        [DefaultValue("选择图片")]
        public string FilePickerName {
            get;
            set;
        }

        /// <summary>
        /// 缩略图宽度
        /// </summary>
        [DefaultValue(300)]
        public int Width {
            get;
            set;
        }

        /// <summary>
        /// 缩略图高度
        /// </summary>
        [DefaultValue(200)]
        public int Height {
            get;
            set;
        }

        /// <summary>
        /// 是否自动上传
        /// </summary>
        [DefaultValue(true)]
        public bool AutoUpload {
            get;
            set;
        }

        /// <summary>
        /// 是否是图片
        /// </summary>
        [DefaultValue(true)]
        public bool IsImage {
            get;
            set;
        }

        /// <summary>
        /// 文件数量
        /// </summary>
        public int FileNumLimit {
            get;
            set;
        }

        /// <summary>
        /// 文件大小
        /// </summary>
        public int FileSizeLimit {
            get;
            set;
        }

        /// <summary>
        /// 单个文件大小
        /// </summary>
        public int FileSingleSizeLimit {
            get;
            set;
        }

        /// <summary>
        /// 服务器接收地址
        /// </summary>
        public string Server {
            get;
            set;
        }

        public Global_WebUploader() {
            this.Style = "uploader-multiple-preview";
            this.UploadName = "上传";
            this.FilePickerName = "选择图片";
            this.Width = 300;
            this.Height = 200;
            this.AutoUpload = false;
            this.IsImage = true;
            this.Server = "/Global/UpLoadProcess";
        }

        public void AddConfig() {

        }
    }

}
