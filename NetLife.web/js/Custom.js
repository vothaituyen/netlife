function ValidateSearch() {
    return require_txt_withoutms("txtKeyword") ? (key = removeHTMLTags("txtKeyword"), 0 == key.length && (key = removeHTMLTags("txtSearch")), document.getElementById("rbtnTitle").checked ? window.location = "/Pages/Search.aspx?key=" + key : document.getElementById("rbtnContent").checked ? window.location = "/Pages/Search.aspx?key=" + key : window.location = "/Pages/Search.aspx?key=" + key, !1) : require_txt("txtSearch", "Bạn chưa nhập từ khóa") ? (key = removeHTMLTags("txtSearch"), null != document.getElementById("rbtnAll") ? document.getElementById("rbtnTitle").checked && (window.location = "/Pages/Search.aspx?key=" + key) : null != document.getElementById("rbtnContent") ? document.getElementById("rbtnContent").checked && (window.location = "/Search.aspx?key=" + key) : window.location = "/Pages/Search.aspx?key=" + key, !1) : !1
}

function TDTEnterPressSearch(e) {
    var t;
    return e && e.which ? (e = e, t = e.which) : (e = window.event, t = e.keyCode), 13 == t ? (ValidateSearch(), !1) : !0
}

function equalHeight(e) {
    var t = 0;
    e.each(function () {
        var e = $(this).height();
        e > t && (t = e)
    }), e.height(t + 20)
}

function vmcLoadScript(e, t) {
    $(e).writeCapture().html("<script type='text/javascript' language='javascript' src='" + t + "'></script>")
}

function LoadImage(e, t) {
    e.src = t, e.onerror = null
}