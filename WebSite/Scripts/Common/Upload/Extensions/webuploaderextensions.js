; (function () {
    window.registerWebUploader = function (webUploaderId, options) {
        //默认配置参数
        var defaultSettings =
        {
            webUploaderId: webUploaderId,
            fileSelecter: webUploaderId + " .WebUploader-filePicker",//文件选择按钮
            uploadBtn: webUploaderId + " .WebUploader-upload",//上传按钮
            fileListSelecter: webUploaderId + " .WebUploader-fileList",//文件列表：存放上传文件信息
            width: 330,//缩略图宽度
            height: 200,//缩略图高度
            autoUpload: false,//是否自动上传
            isImage: true,//是否是图片
            fileNumLimit: 3,
            fileSizeLimit: 1 * 1024 * 1024,
            fileSingleSizeLimit: 1 * 1024 * 1024,
            swf: '/Script/Uploader.swf',//Flash上传插件地址
            server: '/Global/UpLoadProcess',//后台接收文件地址
            accept: {//图片上传筛选配置
                title: 'Images',
                extensions: 'gif,jpg,jpeg,bmp,png',
                mimeTypes: 'image/*'
            }
        }
        var settings = jQuery.extend({}, defaultSettings, options);

        //存放上传文件信息列表
        var $list = $(settings.fileListSelecter),
            // 缩放比列；优化retina, 在retina下这个值是2
            //ratio = window.devicePixelRatio || 1,
            ratio = 1,
            // 缩略图大小
            thumbnailWidth = settings.width * ratio,
            thumbnailHeight = settings.height * ratio;

        //创建上传插件uploader对象
        var uploader = WebUploader.create({
            auto: settings.autoUpload,
            swf: settings.swf,
            server: settings.server,
            pick: settings.fileSelecter,
            accept: settings.accept,
            chunked: true,
            fileNumLimit: settings.fileNumLimit,
            fileSizeLimit: settings.fileSizeLimit,
            fileSingleSizeLimit: settings.fileSingleSizeLimit,
            multiple:false
        });

        //根据配置修改页面信息
        if (settings.autoUpload) {
            $(settings.uploadBtn).hide();
        }
        if (settings.fileNumLimit == 1) {
            $(webUploaderId + " :file").removeAttr("multiple");
        }

        //文件添加进来的时候
        uploader.on('fileQueued', function (file) {
            //获取文件列表模板
            var listitem = "";
            $.ajax({
                type: "GET",
                url: "/Scripts/Common/Upload/Extensions/ListItem.html",
                dataType: "html",
                async: false,
                success: function (data) {
                    listitem = data;
                }
            });
            listitem = $.StringFormat(listitem, file.id, file.name);
            var $li = $(listitem);
            var $img = $li.find('.WebUploader-thumbnail');
            $list.append($li);

            // 创建缩略图
            if (settings.isImage) {
                uploader.makeThumb(file, function (error, src) {
                    if (error) {
                        $img.replaceWith('<span>生成缩略图失败</span>');
                        return;
                    }
                    $img.attr('src', src);
                }, thumbnailWidth, thumbnailHeight);
            }
        });

        //开始上传
        $(settings.uploadBtn).click(function () {
            uploader.upload();
        });

        //进度条实时显示。
        uploader.on('uploadProgress', function (file, percentage) {
            var $percent = $("#" + file.id + " .WebUploader-progress span");
            $percent.css('width', percentage * 100 + '%');
        });

        //暂停
        $(document).on("click", ".WebUploader-filepause", function () {
            var fileid = getFileId(this);
            var file = uploader.getFile(fileid);
            uploader.stop(file);
        });

        //续传
        $(document).on("click", ".WebUploader-filecontinue", function () {
            var fileid = getFileId(this);
            uploader.upload(fileid);
        });

        //文件上传成功
        uploader.on('uploadSuccess', function (file, response) {
            var value = $(settings.webUploaderId + " input:hidden").val();
            var currentFile = response.originalFileName + ":" + response.virtualPath + ";";
            $(settings.webUploaderId + " input:hidden").val(value + currentFile);
        });

        // 文件上传失败
        uploader.on('uploadError', function (file, reason) {
            //上传失败 提示
        });

        // 完成上传完了，成功或者失败，先删除进度条。
        uploader.on('uploadComplete', function (file) {
            $('#' + file.id).find('.WebUploader-progress').remove();
            $('#' + file.id).find(".WebUploader-filepause").remove();
            $('#' + file.id).find(".WebUploader-filecontinue").remove();
        });

        //取消上传、删除已上传文件
        $(document).on("click", ".WebUploader-filedelete", function () {
            var fileid = getFileId(this);
            var file = uploader.getFile(fileid);
            if (file.getStatus() == "complete") {
                alert("异步后台删除文件,删除hidden中数据");
            } else {
                alert("取消上传删除队列");
            }
            uploader.cancelFile(fileid);
            $('#' + fileid).remove();
        });

        uploader.on("error", function (message) {
            if (message == "Q_EXCEED_NUM_LIMIT") {
                alert("只能上传" + settings.fileNumLimit + "个文件");
                return;
            }
            if (message == "Q_EXCEED_SIZE_LIMIT") {
                alert("文件大小不能超过" + settings.fileSingleSizeLimit + "B");
                return;
            }
            if (message == "Q_TYPE_DENIED") {
                alert("此种类型的文件不被允许上传");
                return;
            }
            alert("上传发生错误");
        });

        uploader.on("statuschange", function (cur, prev) {

        });

        function getFileId(elment) {
            var fileid = $(elment).data("fileid");
            return fileid;
        }

    }
})(jQuery, window);