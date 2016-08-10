function ValidateSearch() {
    if (!require_txt_withoutms("txtKeyword")) {
        if (!require_txt("txtSearch", "Bạn chưa nhập từ khóa")) return false;
        key = removeHTMLTags("txtSearch");
        if (document.getElementById("rbtnAll") != null) {
            if (document.getElementById('rbtnTitle').checked)
                window.location = '/Pages/Search.aspx?key=' + key;
        }
        else if (document.getElementById("rbtnContent") != null) {
            if (document.getElementById("rbtnContent").checked)
                window.location = '/Search.aspx?key=' + key;
        }
        else
            window.location = '/Pages/Search.aspx?key=' + key;
        return false;
    }
    else {
        key = removeHTMLTags("txtKeyword");
        if (key.length == 0)
            key = removeHTMLTags("txtSearch");
        if (document.getElementById('rbtnTitle').checked)
            window.location = '/Pages/Search.aspx?key=' + key;
        else if (document.getElementById('rbtnContent').checked)
            window.location = '/Pages/Search.aspx?key=' + key;
        else
            window.location = '/Pages/Search.aspx?key=' + key;
        //window.location = '/Search.aspx?key=' + key;
        return false;
    }

}

function TDTEnterPressSearch(e) {
    var characterCode;
    if (e && e.which)
    { e = e; characterCode = e.which; }
    else
    { e = window.event; characterCode = e.keyCode; }
    if (characterCode == 13)
    { ValidateSearch(); return false; }
    return true;
}

function equalHeight(group) {
    var tallest = 0;
    group.each(function () {
        var thisHeight = $(this).height();
        if (thisHeight > tallest) {
            tallest = thisHeight;
        }
    });
    group.height(tallest + 20);
}

function vmcLoadScript(selector, scriptUrl) {
    $(selector).writeCapture().html("<script type='text/javascript' language='javascript' src='" + scriptUrl + "'><\/script>");
}

function LoadImage(id, src) {
    id.src = src;
    id.onerror = null;
};