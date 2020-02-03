$(document).ready(function () {
    $(".first").click(function () {
        $(".firstcard").show("slow");
        $(".secondcard").hide();
        $(".thirdcard").hide();
        $(".fourthcard").hide();

    });
    $(".second").click(function () {
        $(".secondcard").show("slow");
        $(".firstcard").hide();
        $(".thirdcard").hide();
        $(".fourthcard").hide();
    });
    $(".third").click(function () {
        $(".thirdcard").show("slow");
        $(".secondcard").hide();
        $(".firstcard").hide();
        $(".fourthcard").hide();
    });
    $(".fourth").click(function () {
        $(".fourthcard").show("slow");
        $(".firstcard").hide();
        $(".thirdcard").hide();
        $(".secondcard").hide();
    });
});