(function ($) {
    $(function () {
        var year = $("[data-type=year]");
        if (year) {
            year.text(new Date().getFullYear());
        }
    });
    window.GridView = {
        getDataKeyValue: function (obj, dataKeyClassName) {
            var keyClass = ".datakeyclassname";
            if (dataKeyClassName) {
                keyClass = dataKeyClassName;
            }
            var $this = $(obj);
            var $tr = $this.parent().parent("tr");
            var $td = $tr.children(".datamember");
            var $input = $td.children(keyClass);
            return $input.val();
        }
    }



})(jQuery)