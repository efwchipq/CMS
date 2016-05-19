/**
 * @license Copyright (c) 2003-2015, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function (config) {
    // Define changes to default configuration here. For example:
    // config.language = 'fr';
    // config.uiColor = '#AADC6E';

    // config.toolbar_Mine = [
    //['Source', '-', 'Save', 'NewPage', 'Preview', '-', 'Templates'],
    //['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Print', 'SpellChecker', 'Scayt'],
    //['Undo', 'Redo', '-', 'Find', 'Replace', '-', 'SelectAll', 'RemoveFormat'],
    //['Form', 'Checkbox', 'Radio', 'TextField', 'Textarea', 'Select', 'Button', 'ImageButton', 'HiddenField'],
    // '/',
    //['Bold', 'Italic', 'Underline', 'Strike', '-', 'Subscript', 'Superscript'],
    // ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', 'Blockquote'],
    // ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
    // ['Link', 'Unlink', 'Anchor'],
    //['Image', 'Flash', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar', 'PageBreak'],
    //'/',
    //  ['Styles', 'Format', 'Font', 'FontSize'],
    // ['TextColor'],
    //  ['Downloader', 'WordUpload', 'BatchImage', 'FileUpload', 'VideoUpload']
    // ];
    // config.toolbar = 'Mine';
    // config.extraPlugins += (config.extraPlugins ? ',Downloader' : 'Downloader');//Զ������
    // config.extraPlugins += (config.extraPlugins ? ',WordUpload' : 'WordUpload');//word�ϴ�
    // config.extraPlugins += (config.extraPlugins ? ',BatchImage' : 'BatchImage');//ͼƬ�ϴ�
    // config.extraPlugins += (config.extraPlugins ? ',FileUpload' : 'FileUpload');//�����ϴ�
    // config.extraPlugins += (config.extraPlugins ? ',VideoUpload' : 'VideoUpload');//��Ƶ�ϴ�

    config.toolbar_Mine =
    [
        { name: 'document', items: ['Source', '-', 'Save', 'NewPage', 'DocProps', 'Preview', 'Print', '-', 'Templates'] },
        { name: 'clipboard', items: ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'] },
        { name: 'editing', items: ['Find', 'Replace', '-', 'SelectAll', '-', 'SpellChecker', 'Scayt'] },
        {
            name: 'forms', items: ['Form', 'Checkbox', 'Radio', 'TextField', 'Textarea', 'Select', 'Button', 'ImageButton',
              'HiddenField']
        },
        '/',
        { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-', 'RemoveFormat'] },
        {
            name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote', 'CreateDiv',
            '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', '-', 'BidiLtr', 'BidiRtl']
        },
        { name: 'links', items: ['Link', 'Unlink', 'Anchor'] },
        { name: 'insert', items: ['Image', 'Flash', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar', 'PageBreak', 'Iframe'] },
        '/',
        { name: 'styles', items: ['Styles', 'Format', 'Font', 'FontSize'] },
        { name: 'colors', items: ['TextColor', 'BGColor'] },
        { name: 'tools', items: ['Maximize', 'ShowBlocks', '-', 'About'] },
        { name: 'extensions', items: ['Downloader', 'WordUpload', 'BatchImage', 'FileUpload', 'VideoUpload'] }
    ];
    config.toolbar = 'Mine';
    config.extraPlugins += (config.extraPlugins ? ',Downloader' : 'Downloader');//Զ������
    config.extraPlugins += (config.extraPlugins ? ',WordUpload' : 'WordUpload');//word�ϴ�
    config.extraPlugins += (config.extraPlugins ? ',BatchImage' : 'BatchImage');//ͼƬ�ϴ�
    config.extraPlugins += (config.extraPlugins ? ',FileUpload' : 'FileUpload');//�����ϴ�
    config.extraPlugins += (config.extraPlugins ? ',VideoUpload' : 'VideoUpload');//��Ƶ�ϴ�
};
