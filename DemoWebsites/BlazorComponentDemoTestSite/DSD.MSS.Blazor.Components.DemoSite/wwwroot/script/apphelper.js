function ToggleNavMenu(bool) {
    if (bool) {
        $('.left-nav-column .nav-link').removeClass("nav-tooltip");
        $('.top-nav-header .header-title').removeClass("nav-tooltip");
        $('.top-nav-header').css("max-width", "230px");
        $('.top-nav-header').css("min-width", "230px");
        $('.left-nav-column').css("min-width", "230px");
        $('.angle-double-left').show();
        $('.angle-double-right').hide();
    } else {
        $('.left-nav-column .nav-link').removeClass("nav-tooltip").addClass("nav-tooltip");
        $('.top-nav-header .header-title').removeClass("nav-tooltip").addClass("nav-tooltip");
        $('.top-nav-header').css("max-width", "58px");
        $('.top-nav-header').css("min-width", "58px");
        $('.left-nav-column').css("min-width", "58px");
        $('.angle-double-left').hide();
        $('.angle-double-right').show();
    }
}

function SetPageTitle(Title) {
    $(document).attr("title", Title);
}

function SetScrollBar() {
    $('.scrollable-checklist').scrollTop($('.scrollable-checklist')[0].scrollHeight);
}

