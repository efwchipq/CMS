(function () {
    CKEDITOR.plugins.add('Downloader', {
        icons: 'Downloader',
        init: function (editor) {
            var name = editor.name;//获取传过来的编辑器唯一标识
            var pluginName = 'Downloader';
            var apluginFunction = {
                exec: function () {
                    $.colorbox({
                        iframe: true,
                        width: 600,
                        height: 400,
                        href: "/Global/BatchImage?name=" + name,
                        onClosed: function () {  }
                    });
                    //var element = window.parent.CKEDITOR.dom.element.createFromHtml("<p>1111111111111111111111111</p>");
                    //editor.insertElement(element);
                }
            };
            editor.addCommand(pluginName, apluginFunction);
            editor.ui.addButton(pluginName, {
                label: '远程下载',
                command: pluginName,
                icon: this.path + '/icons/Downloader.png'
            });
        }
    });
})();


