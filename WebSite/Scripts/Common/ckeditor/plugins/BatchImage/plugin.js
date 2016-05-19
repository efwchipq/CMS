(function () {
    CKEDITOR.plugins.add('BatchImage', {
        icons: 'BatchImage',
        init: function (editor) {
            var pluginName = 'BatchImage';
            var a = {
                exec: function(editor) {
                    //alert("这是自定义按钮");
                }
            };
            editor.addCommand(pluginName, a);
            editor.ui.addButton(pluginName, {
                label: '上传图片',
                command: pluginName,
                icon: this.path + '/icons/Downloader.png'
            });
        }
    });
})();


