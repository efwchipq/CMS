;(function($) {
    //后台左侧导航栏
    $(function() {
        //根据特性设置为菜单栏
        var menus = $("[data-type=menu]");
        if (menus) {
            $.each(menus, function(index, elment) {
                var $this = $(elment);
                var menuurl = $this.data("menuurl");
                $this.load(menuurl);
            });
        }

        //设置全选全不选
        $("checkcontainer input:checkbox").click(function () {
            //checkcontainer:选项容器，防止影响页面其他的checkbox
            var $this = $(this);
            var $checkall = $("checkcontainer input.checkall");
            var $checkself = $("checkcontainer input.checkself");
            if ($checkall.length == 0 || $checkself.length == 0) {
                return;
            }

        });
    });
})(jQuery);