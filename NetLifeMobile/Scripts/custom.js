$(document).ready(function () {
    $(".content").find("[style]").css("width", "100%");
    $(".content").find("[width]").css("width", "100%");

    $(".content-info img").removeAttr("style");
    $(".content-info img").removeAttr("height");
    $(".likefb").children().css("width", "100%");

    $("#abdplayer").hover(function () {
        $('.stickyads').css('visibility', 'visible');
    }, function () {
        $('.stickyads').css('visibility', 'hidden');
    });



});