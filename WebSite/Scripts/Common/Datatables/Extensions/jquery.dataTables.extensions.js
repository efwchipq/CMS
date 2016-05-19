;
(function () {
    window.registerDataTable = function (webUploaderId, options) {
        var defaultSettings = {
            //http://datatables.net/reference/option/language.info
            "language": {
                "info": "显示第 _PAGE_ 到 _PAGES_ 条 总条数:_MAX_",
                "infoEmpty": "没有可显示的数据",
                "paginate": {
                    "previous": "上一页",
                    "next": "下一页",
                    "first": "首页",
                    "last": "尾页"
                }
            }
        }
        var settings = jQuery.extend({}, defaultSettings, options);
        var datatable = $(webUploaderId).DataTable(settings);

        datatable.on('preXhr.dt', function (e, settings, data) {
            $.SetMaskingLayer(".dataTables_wrapper");
        });
        datatable.on('xhr.dt', function (e, settings, data) {
            $.RemoveMaskingLayer(".dataTables_wrapper");
        });

        jQuery.dataTableLode = function (currentHref, selecter) {
            if (selecter == webUploaderId) {
                //datatable.ajax.url(datatable.ajax.url()).load();
                //alert(datatable.ajax.url());
                //datatable.ajax.url("http://www.cms.com/news/admin/AjaxList?categoryId=2").load();//"/News/Admin?categoryId=2"
                //alert(datatable.ajax.url());
                //datatable.load();
                datatable.ajax.url(currentHref).load();
            }
        }

    }
})(jQuery, window);

//更改默认配置
//(function () {
//    $.extend($.fn.dataTable.defaults, {
//        "language": {
//            "info": "显示第 _PAGE_ 到 _PAGES_ 条",
//            "paginate": {
//                "previous": "上一页",
//                "next": "下一页"
//            }
//        }
//    });
//})(jQuery);
