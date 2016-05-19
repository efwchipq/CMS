(function () {
    $(function () {
        //$(".two-menu").hide();
        //$("#btn").click(function () {
        //    $(".two-menu").hide();
        //});
    });
    $(function () {
        $(document).on("click", ".first-menu-add", function () {
            $(".first-menu .two-menu").hide();
            $(this).parent().find("ul").show(500);
            $(this).parent().find(".first-menu-minus").show();
            $(this).hide();

        });
        $(document).on("click", ".first-menu-minus", function () {
            $(this).parent().find("ul").hide(500);
            $(this).parent().find(".first-menu-add").show();
            $(this).hide();
        });
    });
})(jQuery)