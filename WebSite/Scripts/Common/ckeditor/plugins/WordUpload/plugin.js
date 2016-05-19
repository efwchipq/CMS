(function () {



    CKEDITOR.plugins.add('WordUpload', {
        icons: 'WordUpload',
        init: function (editor) {
            var pluginName = 'WordUpload';
            //alert("WordUpload");
            return;
            var bpath = this.path + "downloadHandler.ashx";
            //CKEDITOR.dialog.add(pluginName, this.path + 'dialogs/Downloader.js');  $.colorbox({ href: bpath });
            editor.addCommand(pluginName, {
                exec: function (editor) {
                    
                    var myhtml = editor.getData();
                    if (myhtml == "" || myhtml == null) {
                        return;
                    }

                    $.blockUI({
                        message: "<h1>正在努力为您下载图片到本地.....</h1>"
                    });
                    
                    var $container = $("#i-action-content");
                    var PortalId = parseInt($container.attr("portalid"));//当前网站ID

                    $.post(bpath, { content: myhtml, PortalId: PortalId }, function (data) {
                        $.unblockUI();
                        editor.setData(data);
                        $.dnnAlert({
                            okText: '确认',
                            text: '下载远程图片到本地成功',
                            title: '消息'
                        });

                    }).error(function () {
                        $.unblockUI();
                        $.dnnAlert({
                            okText: '确认',
                            text: '非常抱歉,由于网络原因,提交失败,请重新提交',
                            title: '消息'
                        });
                    });
                }
            });
            editor.ui.addButton(pluginName,
            {
                label: '远程图片下载',
                command: pluginName
            });
        }
    });


})();


