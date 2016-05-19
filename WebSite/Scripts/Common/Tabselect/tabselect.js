;
if (!jQuery) {
    throw new Error("插件依赖于jQuery库");
};

(function ($) {
    $.fn.tabselecter = function (options) {
        //如果选项卡没有就不注册
        if (this.lenght < 1) {
            return;
        }
        //注册多个选项卡
        //for (var j = 0; j < this.length; j++) {
            //扩展对象属性
            var extendOptions = $.extend({}, $.fn.tabselecter.options, options);
            //获取菜单栏集合
        //var menus = $(this[j]).children(extendOptions.menuChildSel);
            var menus = $(this).children(extendOptions.menuChildSel);
            if (!menus || menus == "undefined" || menus.length == 0) {
                return;
            }
            //获取目标集合
            var targetElements = [];
            for (var i = 0; i < menus.length; i++) {
                targetElements[i] = $($(menus[i]).attr(extendOptions.tabAttr));
            }

            //初始化显示
            for (var i = 0; i < menus.length; i++) {
                tabshow(menus, targetElements, extendOptions, i, extendOptions.beginIndex);
            }
            /*
              $menus:元素集合
              opts.tabEvent：时间名称
              $menus[ opts.tabEvent] 事件方法 mousemovefunction(a,c){c==null&&(c=a,a=null);return arguments.length>0?this.on(b,null,a,c):this.trigger(b)}
              $menus[ opts.tabEvent](function(){})  设置事件处理函数
             */
            //选项卡事件 
            menus.on(extendOptions.tabEvent, { cuMenu: menus, cuTargetElements: targetElements, cuOptions: extendOptions }, function (event) {
                var $this = $(this);
                var currentMenus = event.data.cuMenu;
                var currentTargetElements = event.data.cuTargetElements;
                var currentOptions = event.data.cuOptions;
                var index = currentMenus.index($this);
                for (var i = 0; i < currentMenus.length; i++) {
                    tabshow(currentMenus, currentTargetElements, currentOptions, i, index);
                }
            });
        //}
    };

    /*
    menus:菜单集合
    targetElements：目标区域集合
    currentOptions：扩展之后属性
    currentindex：当前循环元素索引
    targetindex：当前点击元素索引
    */
    //选项卡显示
    function tabshow(menus, targetElements, currentOptions, currentindex, targetindex) {
        var currentMenu = $(menus[currentindex]);
        var currentTargetElements = $(targetElements[currentindex]);

        if (currentindex != targetindex) {
            currentMenu.removeClass(currentOptions.checkMenuClass);
            currentTargetElements.css("display", "none");
            //去掉当前指向元素样式名
            if (currentTargetElements.hasClass(currentOptions.checkContentClass)) {
                currentTargetElements.removeClass(currentOptions.checkContentClass);
            }
        } else {
            currentMenu.addClass(currentOptions.checkMenuClass);
            currentTargetElements.css("display", "block");
            //给当前选中内容添加样式名
            if (!currentTargetElements.hasClass(currentOptions.checkContentClass)) {
                currentTargetElements.addClass(currentOptions.checkContentClass);
            }
        }
    }

    //默认属性
    $.fn.tabselecter.options = {
        beginIndex: 0,//初始化显示集合
        tabEvent: "mouseover",//菜单栏事件
        tabAttr: "data-href",//菜单栏指向显示区域 选择器
        checkMenuClass: "currentselect",//选中之后菜单栏添加样式名
        checkContentClass: "currentselectcontent",//选中之后菜单栏添加样式名
        menuChildSel: "*"//菜单栏元素 选择器
    };
})(jQuery);

//; (function ($) {
//    $(function () {
//        //注册默认选项卡
//        $("[data-type=cms-tabselect] ul").tabselecter({
//            menuChildSel: "li",
//            tabEvent: "click"
//        });

//        $("# ul").tabselecter({ beginIndex: 0, tabEvent: "click", checkMenuClass: "currentselect", checkContentClass: "" });

//    });
//})(jQuery);
