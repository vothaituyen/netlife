﻿function getDomainByName(e, t) {
    return t == "m." + e || t == e || t == "www." + e
}

function createCookie(e, t, i) {
    if (i) {
        var n = new Date;
        n.setTime(n.getTime() + 30 * i * 60 * 1e3);
        var a = "; expires=" + n.toGMTString()
    } else var a = "";
    document.cookie = e + "30=" + t + a + "; path=/"
}

function readCookie(e) {
    for (var t = e + "30=", i = document.cookie.split(";"), n = 0; n < i.length; n++) {
        for (var a = i[n];
            " " == a.charAt(0);) a = a.substring(1, a.length);
        if (0 == a.indexOf(t)) return a.substring(t.length, a.length)
    }
    return null
}

function isSupportFlash() {
    return FlashDetect.installed
}

function vmc_ExpandBanner(e, t, i, n) {
    var a = $("#" + e);
    null != a && (hoverBannerTimeOut = setTimeout(function () {
        a.attr("style", "z-index:999999999;overflow:visible; position:relative;width:" + a.attr("w") + "px;height:" + a.attr("h") + "px;")
    }, 3e3))
}

function vmc_Minimize_Ballon(e, t, i) {
    $("#" + e).css({
        width: t,
        height: i
    })
}

function vmc_GetPositionExpand(e) {
    console.log("d=" + e), "undefined" == typeof e && (e = 2);
    var t = "";
    switch (e) {
        case 0:
            t = "top:0;left:0;position: absolute;";
            break;
        case 1:
            t = "bottom:0;left:0;position: absolute;";
            break;
        case 2:
            t = "top:0px;right:0;position: absolute;";
            break;
        case 3:
            t = "bottom:0;right:0;position: absolute;";
            break;
        case 5:
            t = "top:-145px;right:0;position: absolute;"
    }
    return t
}

function GetFlashObject(e, t, i, n) {
    var a = '<param name="FlashVars" value="path=' + e + "&target=" + encodeURIComponent(t) + "&w=" + i + "&h=" + n + '">',
        o = "/Images/Adv/wrapper.swf";
    return '<object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" width="' + i + '" height="' + n + '" id="rotator" align="middle"><param name="movie" value="' + o + '" /><param name="quality" value="high" /><param name="bgcolor" value="#ffffff" /><param name="play" value="true" /><param name="loop" value="true" /><param name="wmode" value="transparent" /><param name="scale" value="noscale" /><param name="menu" value="true" /><param name="devicefont" value="false" /><param name="salign" value="tl" /><param name="allowScriptAccess" value="sameDomain" />' + a + '<!--[if !IE]>--><object type="application/x-shockwave-flash" data="' + o + '" width="' + i + '" height="' + n + '"><param name="movie" value="' + o + '" /><param name="quality" value="high" /><param name="bgcolor" value="#ffffff" /><param name="play" value="true" /><param name="loop" value="true" /><param name="wmode" value="transparent" /><param name="scale" value="noscale" /><param name="menu" value="true" /><param name="devicefont" value="false" /><param name="salign" value="tl" /><param name="allowScriptAccess" value="sameDomain" />' + a + '<!--<![endif]--><a href="http://www.adobe.com/go/getflash"><img src="http://www.adobe.com/images/shared/download_buttons/get_flash_player.gif" alt="Get Adobe Flash player" /></a><!--[if !IE]>--></object><!--<![endif]--></object>'
}

function vmc_ResizeBanner(e, t, i, n) {
    var a = $("#" + e);
    null != a && (a.attr("style", "overflow:hidden; position:relative;width:" + a.attr("w") + "px;height:" + a.attr("h") + "px;"), a.find("div").attr("style", "width:" + t + "px;height:" + i + "px;" + vmc_GetPositionExpand(n)))
}

function vmc_CollapseBanner(e) {
    var t = $("#" + e);
    clearTimeout(hoverBannerTimeOut), null != t && t.attr("style", "z-index:0;overflow:hidden; position:relative;width:" + t.attr("w") + "px;height:" + t.attr("h") + "px;")
}

function RunBanner(rawdata, zone_id) {
    if (this.current = 0, this.zoneId = zone_id, this.zoneName = "", this.len = rawdata.length, this.data = rawdata, this.len > 0) {
        var cc = readCookie(zone_id);
        null != cc && cc.length > 0 ? (this.current = eval(cc) + 1, this.current >= this.len && (this.current = 0)) : this.current = Math.floor(Math.random() * this.len), createCookie(zone_id, this.current, 1)
    }
    this.Show = function () {
        this.len > 0 && this.ChangeBanner()
    }, this.ChangeBanner = function () {
        try {
            this.AdsvnRenderBanner(this.data[this.current])
        } catch (e) {
            console.log(e.message + ":loi")
        }
    }, this.AdsvnRenderBanner = function (e) {
        if (void 0 != typeof e)
            if (0 == e.Link.length && (e.Link = location.href), adVnn.push(e.ID), arrayAdsInPage.push(e), 1 == e.Type) document.write('<div class="vmc_ads_viewport" advId="' + e.ID + '" style="z-index:555; margin:0 0 5px 0; padding:0;width:' + e.Width + "px;" + e.Style + '"><a href="' + AdsvnRenderLink(e.ID, e.Link) + '" target="_blank"><img style="border:none; padding:0; margin:0;" src="' + e.src + '" width="' + e.Width + 'px" height="' + e.Height + '"/></a>' + vadslogo + "</div>"), e.Img.length > 0 && document.write('<iframe src="' + e.Img + (-1 != e.Img.indexOf("?") ? "&r=" : "?r=") + Math.random() + '" width="0" style="display:none" height="0"></iframe>');
            else if (2 == e.Type) isSupportFlash() ? document.write('<div class="vmc_ads_viewport" advId="' + e.ID + '" style="z-index:555; margin:0 0 5px 0; padding:0;width:' + e.Width + "px;" + e.Style + '">' + GetFlashObject(e.src, AdsvnRenderLink(e.ID, e.Link), e.Width, e.Height) + vadslogo + "</div>") : document.write('<div class="vmc_ads_viewport" advId="' + e.ID + '"  style="z-index:555; margin:0 0 5px 0; padding:0;width:' + e.Width + "px;" + e.Style + '"><a href="' + AdsvnRenderLink(e.ID, e.Link) + '" target="_blank"><img style="border:none; padding:0; margin:0;" src="' + e.Img + '" width="' + e.Width + 'px" height="' + e.Height + '"/></a></div>');
            else if (3 == e.Type || 4 == e.Type && !isIE()) document.writeln('<div class="vmc_ads_viewport" advId="' + e.ID + '"   style="z-index:555; margin:0 0 0px 0; padding:0;width:' + e.Width + "px;" + e.Style + '">' + e.src + "</div>");
            else if (5 == e.Type);
            else if (6 == e.Type) {
                var t = "divExpand" + (new Date).getTime();
                isSupportFlash() ? document.write('<div class="vmc_ads_viewport" advId="' + e.ID + '"   id="' + t + '" style="z-index:555; width: 980px; height: 50px; overflow: hidden;"><embed quality="high"  allowscriptaccess="always" flashvars="div_id=' + t + "&stringURL=" + encodeURIComponent(e.Link) + '" wmode="transparent" pluginspage="http://www.macromedia.com/go/getflashplayer" type="application/x-shockwave-flash" src="' + e.src + '" height="480" width="980">' + vadslogo + "</div>") : document.write('<div class="vmc_ads_viewport" advId="' + e.ID + '"   style="z-index:555; margin:0 0 5px 0; padding:0;width:980px;' + e.Style + '"><a href="' + AdsvnRenderLink(e.ID, e.Link) + '" target="_blank"><img style="border:none; padding:0; margin:0;" src="' + e.Img + '" width="980px" height="50px"/></a></div>')
            } else if (7 == e.Type) {
                var i = "div_" + e.Zone_ID + "_" + (new Date).getTime();
                isSupportFlash() ? document.write(addCss + '<div class="vmc_ads_viewport"  advId="' + e.ID + '"  style="z-index:555; margin:0 0 5px 0; padding:0;"><div id="' + i + '" onmouseover="AdsvnMouseOver(this)" w="' + e.Width + '" h="' + e.Height + '"  style="position:relative; overflow:hidden; padding:0;width:' + e.Width + "px;height:" + e.Height + "px;" + e.Style + '"><div style="position:absolute;left:0;top:0; background-color:#fff;"><embed src="' + e.src + '"  ' + (e.Link.length > 0 ? 'flashvars="zoneid=' + i + "&amp;stringURL=" + encodeURIComponent(e.Link) + '"' : "") + ' width="100%" height=100%" pluginspage="http://www.macromedia.com/go/getflashplayer" type="application/x-shockwave-flash" allowscriptaccess="always" wmode="transparent" quality="high" ></embed></div></div>' + vadslogo + "</div>") : document.write('<div class="vmc_ads_viewport"  advId="' + e.ID + '"  w="' + e.Width + '" h="' + e.Height + '"  style="z-index:555; margin:0 0 5px 0; padding:0;width:' + e.Width + "px; height:" + e.Height + "px;" + e.Style + '"><a href="' + AdsvnRenderLink(e.ID, e.Link) + '" target="_blank"><img style="border:none; padding:0; margin:0;" src="' + e.Img + '" width="100%" height="100%"/></a>' + vadslogo + "</div>")
            } else if (9 == e.Type) {
                var i = "div_" + e.Zone_ID + "_" + (new Date).getTime();
                isSupportFlash() && document.write(addCss + '<div class="vmc_ads_viewport"  advId="' + e.ID + '"  style="z-index:555; margin:0 0 5px 0; padding:0;"><div id="' + i + '"  style="position:fixed; overflow:hidden; padding:0; right:5px; bottom:5px;"><embed src="' + e.src + '"  flashvars="div_id=' + i + "&amp;stringURL=" + escape(AdsvnRenderLink(e.ID, e.Link)) + '" width="' + e.Width + 'px" height="' + e.Height + 'px" pluginspage="http://www.macromedia.com/go/getflashplayer" type="application/x-shockwave-flash" allowscriptaccess="always" wmode="transparent" quality="high" ></embed>' + vadslogo + "</div>")
            } else if (10 == e.Type) setTimeout(function () {
                $("#catfish").hide()
            }, 1e4), document.write('<div id="catfish" style="width: 100%;position:fixed;bottom:0;left:0;"><div style="width:300px;height:50px;margin:0 auto"><img src="' + e.src + '" style="display:none"/><a href="' + e.Link + '"> <img src="' + e.src + '" style="display: block;margin: 0 auto" /></a></div></div>');
            else if (11 == e.Type) {
                var n = '<div id="vmcFullbg" style="z-index:10000;background:#000;position:fixed;top:0; left:0; height:100%; width:100%;"><span></span></div><div id="adv_ttv_mobile_3000" style="z-index:10000;width: 100%;height: 100%;position:absolute;top:0; left:0;"><div style="position:relative;clear:both;width: 100%;height: 100%;"><div id="ban_pop" style="clear:both;position: relative"><div id="cl_pop_bt" style="overflow-y: hidden; position: absolute; top: 20px; right: 0px;"><a id="adv_ttv_close" href="javascript:void(0)" onclick="close_avt()" class="adv_ttv_close_adv" style="height:32px;display:inline-block;"><span style="display: block; text-align: center; font-family: arial; float: right; font-size: 13px; border-radius: 3px 0px 0px 3px; font-weight: bold; background-color:#ffffff; color: #ff0000; padding: 0px 10px; height: 31px; line-height: 31px;">Đóng <i style="font-weight: bold; display: inline-block; height: 100%; vertical-align: top; font-size: 21px; padding-top: 1px;">×</i></span></a></div><a id="log-ads" style="position: absolute;top: 0;right: 0;"><img src="http://img.vietnamnetad.vn/Images/logo/vads-logo.png" height="17"></a>';
                n += '<img src="' + e.Link + '" style="display:none"/><a rel="nofollow" href="' + e.Link + '" target="_blank" id="adv_ttv_mobile_3000_a" style="clear:both; display:block; text-decoration:none;border:0;"><img border="0" width="100%" style="display:block;margin:0 auto" id="img_adv_ttv_mobile_3000" vspace="0" hspace="0" src="' + e.src + '" alt="Quảng cáo" /></a></div></div></div>', vmcTimeout = setTimeout("close_avt()", 7e3), document.write(n)
            } else 12 == e.Type && renderBannerBackground(e)
    }
}

function close_avt() {
    document.getElementById("adv_ttv_mobile_3000").style.display = "none", document.getElementById("vmcFullbg").style.display = "none", vmcTimeout && window.clearTimeout(vmcTimeout)
}

function AdsvnMouseOver(e) {
    e.style.overflow = "visible"
}

function AdsvnMouseOut(e) {
    e.style.overflow = "hidden"
}

function AdsvnRenderClick(e) {
    if (!(0 >= e)) {
        var t = new Image;
        t.src = logLocation + "/Pages/Ads/log.ashx?type=click&itemId=" + e + "&location=" + encodeURIComponent(location.href)
    }
}

function isIE() {
    return "Microsoft Internet Explorer" == navigator.appName
}

function AdsvnRenderLink(e, t) {
    return logLocation + "/Pages/Ads/log.ashx?type=click&itemId=" + e + "&isLink=1&location=" + encodeURIComponent(location.href) + (t ? "&nextUrl=" + encodeURIComponent(t) : "")
}

function jsAnimate(e, t, i) {
    var n = new Number,
        a = new Array;
    a = String(document.getElementById(e).style[t]).split("px"), n = parseInt(a[0]), clearTimeout(jsTimeout), parseInt(i) > n ? jsTimeout = setTimeout('animateProcess("' + e + '", "' + t + '", "increase", "' + n + '", "' + i + '", 1)', jsTimeDelay) : parseInt(i) < n && (jsTimeout = setTimeout('animateProcess("' + e + '", "' + t + '", "increase", "' + n + '", "' + i + '", 1)', jsTimeDelay))
}

function animateProcess(e, t, i, n, a, o) {
    clearTimeout(jsTimeout);
    var r = jsAcceleration * Math.pow(parseInt(o), 2) / 2 + jsVelocity * parseInt(o),
        s = new Number;
    switch (i) {
        case "increase":
            s = parseInt(n) + parseInt(r), s >= parseInt(a) ? document.getElementById(e).style[t] = a + "px" : (document.getElementById(e).style[t] = s + "px", o++ , jsTimeout = setTimeout('animateProcess("' + e + '", "' + t + '", "' + i + '", "' + s + '", "' + a + '", ' + o + ")", jsTimeDelay));
            break;
        case "decrease":
            s = parseInt(n) - parseInt(r), s <= parseInt(a) ? document.getElementById(e).style[t] = a + "px" : (document.getElementById(e).style[t] = s + "px", o++ , jsTimeout = setTimeout('animateProcess("' + e + '", "' + t + '", "' + i + '", "' + s + '", "' + a + '", ' + o + ")", jsTimeDelay))
    }
}

function hookExpand(e, t, i) {
    var n = document.getElementById(e);
    if (null != n) {
        if (i) {
            for (var a = n.offsetTop, o = n; o.offsetParent && o.offsetParent != document.body;) o = o.offsetParent, a += o.offsetTop;
            window.scrollTo(0, a)
        }
        n.style.clip = t
    }
}

function hookExpandOverflow(e, t, i) {
    var n = document.getElementById(e);
    if (null != n && (n.style.clip = t, i)) {
        for (var a = n.offsetTop, o = n; o.offsetParent && o.offsetParent != document.body;) o = o.offsetParent, a += o.offsetTop;
        window.scrollTo(0, a);
        for (var r = n.parentNode; r.parentNode && r.parentNode != document.body && ("visible" != r.style.overflow && (r.style.overflow = "visible"), !(parseInt(r.offsetHeight) >= 500));) r = r.parentNode
    }
}

function vmcAppendFrame(e) {
    var t = document.createElement("iframe");
    t.style.display = "none", t.src = e, document.body.appendChild(t)
}

function scrollbeginHandler() { }

function scrollendHandler() {
    vmcScrollTimeout = null
}

function renderBannerBackground(e) {
    return ""
}

function vmcloadJs(e, t) {
    var i = document.createElement("script");
    i.type = "text/javascript", i.src = e, 2 <= arguments.length && (i.onload = t, i.onreadystatechange = function () {
        4 != i.readyState && "complete" != i.readyState || t()
    }), document.getElementsByTagName("head")[0].appendChild(i)
}
var logLocation = "http://netlife.vn",
    adVnn = [],
    vadslogo = "",
    addCss = "",
    FlashDetect = new function () {
        var e = this;
        e.installed = !1, e.raw = "", e.major = -1, e.minor = -1, e.revision = -1, e.revisionStr = "";
        var t = [{
            name: "ShockwaveFlash.ShockwaveFlash.7",
            version: function (e) {
                return n(e)
            }
        }, {
            name: "ShockwaveFlash.ShockwaveFlash.6",
            version: function (e) {
                var t = "6,0,21";
                try {
                    e.AllowScriptAccess = "always", t = n(e)
                } catch (i) { }
                return t
            }
        }, {
            name: "ShockwaveFlash.ShockwaveFlash",
            version: function (e) {
                return n(e)
            }
        }],
            n = function (e) {
                var t = -1;
                try {
                    t = e.GetVariable("$version")
                } catch (i) { }
                return t
            },
            a = function (e) {
                var t = -1;
                try {
                    t = new ActiveXObject(e)
                } catch (i) {
                    t = {
                        activeXError: !0
                    }
                }
                return t
            },
            o = function (e) {
                var t = e.split(",");
                return {
                    raw: e,
                    major: parseInt(t[0].split(" ")[1], 10),
                    minor: parseInt(t[1], 10),
                    revision: parseInt(t[2], 10),
                    revisionStr: t[2]
                }
            },
            r = function (e) {
                var t = e.split(/ +/),
                    i = t[2].split(/\./),
                    n = t[3];
                return {
                    raw: e,
                    major: parseInt(i[0], 10),
                    minor: parseInt(i[1], 10),
                    revisionStr: n,
                    revision: s(n)
                }
            },
            s = function (t) {
                return parseInt(t.replace(/[a-zA-Z]/g, ""), 10) || e.revision
            };
        e.majorAtLeast = function (t) {
            return e.major >= t
        }, e.minorAtLeast = function (t) {
            return e.minor >= t
        }, e.revisionAtLeast = function (t) {
            return e.revision >= t
        }, e.versionAtLeast = function (t) {
            var n = [e.major, e.minor, e.revision],
                a = Math.min(n.length, arguments.length);
            for (i = 0; i < a; i++) {
                if (n[i] >= arguments[i]) {
                    if (i + 1 < a && n[i] == arguments[i]) continue;
                    return !0
                }
                return !1
            }
        }, e.FlashDetect = function () {
            if (navigator.plugins && navigator.plugins.length > 0) {
                var i = "application/x-shockwave-flash",
                    n = navigator.mimeTypes;
                if (n && n[i] && n[i].enabledPlugin && n[i].enabledPlugin.description) {
                    var s = n[i].enabledPlugin.description,
                        l = r(s);
                    e.raw = l.raw, e.major = l.major, e.minor = l.minor, e.revisionStr = l.revisionStr, e.revision = l.revision, e.installed = !0
                }
            } else if (-1 == navigator.appVersion.indexOf("Mac") && window.execScript)
                for (var s = -1, d = 0; d < t.length && -1 == s; d++) {
                    var c = a(t[d].name);
                    if (!c.activeXError && (e.installed = !0, s = t[d].version(c), -1 != s)) {
                        var l = o(s);
                        e.raw = l.raw, e.major = l.major, e.minor = l.minor, e.revision = l.revision, e.revisionStr = l.revisionStr
                    }
                }
        }()
    };
FlashDetect.JS_RELEASE = "1.0.4";
var hoverBannerTimeOut, vmcTimeout, jsTimeout = new Number,
    jsTimeDelay = 60,
    jsAcceleration = .2,
    jsVelocity = 3;
jQuery.fn.isOnScreen = function () {
    var e = jQuery(window),
        t = {
            top: e.scrollTop(),
            left: e.scrollLeft()
        };
    t.right = t.left + e.width(), t.bottom = t.top + e.height();
    var i = this.offset();
    return i.right = i.left + this.outerWidth(), i.bottom = i.top + this.outerHeight(), !(t.right < i.left || t.left > i.right || t.bottom < i.top || t.top > i.bottom)
};
var arrayIsInviewport = new Array,
    arrayAdsInPage = new Array,
    vmcScrollTimeout = null,
    vmcScrollendDelay = 500,
    isVnnLoaded = !1;
window.onload = function () {
    adVnn.length > 0 && !isVnnLoaded && (isVnnLoaded = !0, console.log("log call view"), (new Image).src = logLocation + "/Pages/Ads/log.ashx?type=impression&itemIds=" + adVnn.join(",") + "&location=" + encodeURIComponent(location.href) + "&trueImpression=0&t=" + new Date)
}, window.onbeforeunload = function () {
    adVnn.length > 0 && !isVnnLoaded && (isVnnLoaded = !0, console.log("log call view"), (new Image).src = logLocation + "/Pages/Ads/log.ashx?type=impression&itemIds=" + adVnn.join(",") + "&location=" + encodeURIComponent(location.href) + "&trueImpression=0&t=" + new Date)
}, jQuery.fn.extend({
    VADSSticky: function (e, t, i) {
        var n = $(this);
        if (0 != $("#" + t).length && 0 != $("#" + e).length && 0 != $("#" + i).length) {
            var a = $("#" + t),
                o = $("#" + e),
                r = $("#" + i);
            n.scroll(function () {
                var e = a.offset().top,
                    t = o.offset().top,
                    i = r.height(),
                    s = n.scrollTop();
                s >= t ? s + i >= e ? r.css({
                    top: -1 * (i - (e - s) + 20) + "px",
                    position: "fixed"
                }) : r.css({
                    top: "5px",
                    position: "fixed"
                }) : r.css({
                    top: "",
                    position: ""
                })
            })
        }
    }
});
var imgHost = "";