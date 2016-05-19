;
(function () {
    $.extend({
        StringFormat: function () {
            if (arguments.length == 0)
                return null;
            var str = arguments[0];
            for (var i = 1; i < arguments.length; i++) {
                var re = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
                str = str.replace(re, arguments[i]);
            }
            return str;
        }
    });
    $.extend({
        SetMaskingLayer: function (selecter) {
            var $element = $(selecter);
            if ($element.length==0) {
                return;
            }
            $element.css("position", "absolute");
            var $maskingLayer1 = $(selecter + " > .MaskingLayer1");
            var $maskingLayer2 = $(selecter + " > .MaskingLayer2");
            if ($maskingLayer1.length == 0 || $maskingLayer2.length == 0) {
                $("<div id=\"bg\" class=\"MaskingLayer1\"></div> <div id=\"show\" class=\"MaskingLayer2\" >  <img src=\"/Files/SysFile/images/loading.gif\" style=\"display: inline-block\" />  </div> ").appendTo(selecter);
            }
            $maskingLayer1 = $(selecter + " > .MaskingLayer1");
            $maskingLayer2 = $(selecter + " > .MaskingLayer2");
            $maskingLayer1.show();
            $maskingLayer2.show();
        }
    });
    $.extend({
        RemoveMaskingLayer: function (selecter) {
            var $element = $(selecter);
            if ($element.length==0) {
                return;
            }
            $element.css("position", "static");
            var $maskingLayer1 = $(selecter + " > .MaskingLayer1");
            var $maskingLayer2 = $(selecter + " > .MaskingLayer2");
            if ($maskingLayer1.length != 0 || $maskingLayer2.length != 0) {
                $maskingLayer1.hide();
                $maskingLayer2.hide();
            }
        }
    });
})(jQuery);