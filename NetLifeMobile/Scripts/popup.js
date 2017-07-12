! function (o) {
    o.fn.extend({
        showPopup: function (e) {
            function n(n) {
                e.onClose instanceof Function && e.onClose(), o("html").css({
                    overflow: "auto"
                }), o("body").css({
                    overflow: "auto"
                }), o("#lean_overlay").fadeOut(200), o(n).css({
                    display: "none"
                })
            }
            var l = o("<div id='lean_overlay'></div>");
            return o("body").append(l), e = o.extend({
                top: 100,
                overlay: .5,
                scroll: !0,
                closeButton: null
            }, e), this.each(function () {
                var l = e;
                o(this).click(function (e) {
                    var t = o(this).attr("href");
                    o("#lean_overlay").click(function () {
                        n(t)
                    }), o(l.closeButton).click(function () {
                        n(t)
                    }), o(t).outerHeight();
                    o(t).outerWidth();
                    o("#lean_overlay").css({
                        display: "block",
                        opacity: 0
                    }), o("#lean_overlay").fadeTo(200, l.overlay), o(t).css({
                        display: "block",
                        position: "absolute",
                        opacity: 0,
                        "z-index": 110
                    }), l.scroll && (o("html").css({
                        overflow: "hidden"
                    }), o("body").css({
                        overflow: "hidden"
                    })), o(t).fadeTo(200, 1), e.preventDefault()
                })
            })
        }
    })
}(jQuery);