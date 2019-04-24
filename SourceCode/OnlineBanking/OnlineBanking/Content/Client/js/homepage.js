$(document).ready(function () {
    $('#example').DataTable();

    //update status vertical-menu
    $(".vertical-menu a").click(function () {
        $(".vertical-menu a").removeClass('active');
        $(this).addClass('active');
        $(".vertical-menu a:not('.active')").fadeTo(100, 0.7);
        $(".vertical-menu a.active").fadeTo(100, 1);
    });

    //update status navbar
    $(".navbar-nav.mr-auto li a").click(function () {
        $(".navbar-nav li").removeClass('active');
        $(this).parent().addClass('active');
    });

    $(".vertical-menu").mouseenter(
        function () {
            $(this).fadeTo(200, 1);
            $(".vertical-menu a:not('.active')").fadeTo(200, 0.6);

        });

    $(".vertical-menu").mouseleave(
        function () {
            $(".vertical-menu a:not('.active')").fadeTo(100, 0.3);
            $(".vertical-menu a.active").fadeTo(1, 1);

        });

    $(".vertical-menu .dropdown").click(function () {
        $(".vertical-menu a").removeClass('active');
        $(".dropdown > a").addClass('active');
    });

});