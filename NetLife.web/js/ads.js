   

function getDomainByName(domain, url) {

    return (url == "m." + domain || url == domain || url == ("www." + domain));
}
 
  

var logLocation = "http://netlife.vn";
var adVnn = [];
/*gan them logo vads*/
 
var vadslogo = '';
var addCss = "";
var FlashDetect = new function () {
    var self = this; self.installed = false; self.raw = ""; self.major = -1; self.minor = -1; self.revision = -1; self.revisionStr = ""; var activeXDetectRules = [{ "name": "ShockwaveFlash.ShockwaveFlash.7", "version": function (obj) { return getActiveXVersion(obj); } }, {
        "name": "ShockwaveFlash.ShockwaveFlash.6", "version": function (obj) {
            var version = "6,0,21"; try { obj.AllowScriptAccess = "always"; version = getActiveXVersion(obj); } catch (err) { }
            return version;
        }
    }, { "name": "ShockwaveFlash.ShockwaveFlash", "version": function (obj) { return getActiveXVersion(obj); } }]; var getActiveXVersion = function (activeXObj) {
        var version = -1; try { version = activeXObj.GetVariable("$version"); } catch (err) { }
        return version;
    }; var getActiveXObject = function (name) {
        var obj = -1; try { obj = new ActiveXObject(name); } catch (err) { obj = { activeXError: true }; }
        return obj;
    }; var parseActiveXVersion = function (str) { var versionArray = str.split(","); return { "raw": str, "major": parseInt(versionArray[0].split(" ")[1], 10), "minor": parseInt(versionArray[1], 10), "revision": parseInt(versionArray[2], 10), "revisionStr": versionArray[2] }; }; var parseStandardVersion = function (str) { var descParts = str.split(/ +/); var majorMinor = descParts[2].split(/\./); var revisionStr = descParts[3]; return { "raw": str, "major": parseInt(majorMinor[0], 10), "minor": parseInt(majorMinor[1], 10), "revisionStr": revisionStr, "revision": parseRevisionStrToInt(revisionStr) }; }; var parseRevisionStrToInt = function (str) { return parseInt(str.replace(/[a-zA-Z]/g, ""), 10) || self.revision; }; self.majorAtLeast = function (version) { return self.major >= version; }; self.minorAtLeast = function (version) { return self.minor >= version; }; self.revisionAtLeast = function (version) { return self.revision >= version; }; self.versionAtLeast = function (major) { var properties = [self.major, self.minor, self.revision]; var len = Math.min(properties.length, arguments.length); for (i = 0; i < len; i++) { if (properties[i] >= arguments[i]) { if (i + 1 < len && properties[i] == arguments[i]) { continue; } else { return true; } } else { return false; } } }; self.FlashDetect = function () { if (navigator.plugins && navigator.plugins.length > 0) { var type = 'application/x-shockwave-flash'; var mimeTypes = navigator.mimeTypes; if (mimeTypes && mimeTypes[type] && mimeTypes[type].enabledPlugin && mimeTypes[type].enabledPlugin.description) { var version = mimeTypes[type].enabledPlugin.description; var versionObj = parseStandardVersion(version); self.raw = versionObj.raw; self.major = versionObj.major; self.minor = versionObj.minor; self.revisionStr = versionObj.revisionStr; self.revision = versionObj.revision; self.installed = true; } } else if (navigator.appVersion.indexOf("Mac") == -1 && window.execScript) { var version = -1; for (var i = 0; i < activeXDetectRules.length && version == -1; i++) { var obj = getActiveXObject(activeXDetectRules[i].name); if (!obj.activeXError) { self.installed = true; version = activeXDetectRules[i].version(obj); if (version != -1) { var versionObj = parseActiveXVersion(version); self.raw = versionObj.raw; self.major = versionObj.major; self.minor = versionObj.minor; self.revision = versionObj.revision; self.revisionStr = versionObj.revisionStr; } } } } }();
};
FlashDetect.JS_RELEASE = "1.0.4";

function createCookie(name, value, days) {
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 30 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    }
    else var expires = "";
    document.cookie = name + "30=" + value + expires + "; path=/";
}

function readCookie(name) {
    var nameEQ = name + "30=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) {
            return c.substring(nameEQ.length, c.length);
        }
    }
    return null;
}

function isSupportFlash() {
    return FlashDetect.installed;
}
var hoverBannerTimeOut;
function vmc_ExpandBanner(a, b, c, d) {
    var obj = $("#" + a);
    if (obj != null) {
        hoverBannerTimeOut = setTimeout(function () {
            obj.attr("style", "z-index:999999999;overflow:visible; position:relative;width:" + obj.attr("w") + "px;height:" + obj.attr("h") + "px;");
        }, 3000);

    }
}

function vmc_Minimize_Ballon(d, w, h) {
    $("#" + d).css({ "width": w, "height": h });
}

function vmc_GetPositionExpand(d) {
    console.log('d=' + d);
    if (typeof (d) == 'undefined') { d = 2; }
    var result = "";
    switch (d) {
        case 0:
            result = "top:0;left:0;position: absolute;";
            break;
        case 1:
            result = "bottom:0;left:0;position: absolute;";
            break;
        case 2:
            result = "top:0px;right:0;position: absolute;";
            break;
        case 3:
            result = "bottom:0;right:0;position: absolute;";
            break;
        case 5:
            result = "top:-145px;right:0;position: absolute;";
            break;
    }
    return result;
}

function GetFlashObject(filePath, target, width, height) {
    var param = "<param name=\"FlashVars\" value=\"path=" + filePath + "&target="  +  encodeURIComponent(target) + "&w=" + width + "&h=" + height + "\">";
    var fileSwf = "/Images/Adv/wrapper.swf";
    return "<object classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\" width=\"" + width + "\" height=\"" + height + "\" id=\"rotator\" align=\"middle\">" +
                "<param name=\"movie\" value=\"" + fileSwf + "\" />" +
                "<param name=\"quality\" value=\"high\" />" +
                "<param name=\"bgcolor\" value=\"#ffffff\" />" +
                "<param name=\"play\" value=\"true\" />" +
                "<param name=\"loop\" value=\"true\" />" +
                "<param name=\"wmode\" value=\"transparent\" />" +
                "<param name=\"scale\" value=\"noscale\" />" +
                "<param name=\"menu\" value=\"true\" />" +
                "<param name=\"devicefont\" value=\"false\" />" +
                "<param name=\"salign\" value=\"tl\" />" +
                "<param name=\"allowScriptAccess\" value=\"sameDomain\" />" +
                param +
                "<!--[if !IE]>-->" +
                "<object type=\"application/x-shockwave-flash\" data=\"" + fileSwf + "\" width=\"" + width + "\" height=\"" + height + "\">" +
                    "<param name=\"movie\" value=\"" + fileSwf + "\" />" +
                    "<param name=\"quality\" value=\"high\" />" +
                    "<param name=\"bgcolor\" value=\"#ffffff\" />" +
                    "<param name=\"play\" value=\"true\" />" +
                    "<param name=\"loop\" value=\"true\" />" +
                    "<param name=\"wmode\" value=\"transparent\" />" +
                    "<param name=\"scale\" value=\"noscale\" />" +
                    "<param name=\"menu\" value=\"true\" />" +
                    "<param name=\"devicefont\" value=\"false\" />" +
                    "<param name=\"salign\" value=\"tl\" />" +
                    "<param name=\"allowScriptAccess\" value=\"sameDomain\" />" +
                    param +
                "<!--<![endif]-->" +
                    "<a href=\"http://www.adobe.com/go/getflash\">" +
                        "<img src=\"http://www.adobe.com/images/shared/download_buttons/get_flash_player.gif\" alt=\"Get Adobe Flash player\" />" +
                    "</a>" +
                "<!--[if !IE]>-->" +
                "</object>" +
                "<!--<![endif]-->" +
            "</object>";
}

function vmc_ResizeBanner(a, b, c, d) {
    var obj = $("#" + a);
    if (obj != null) {
        obj.attr("style", "overflow:hidden; position:relative;width:" + obj.attr("w") + "px;height:" + obj.attr("h") + "px;");
        obj.find("div").attr("style", "width:" + b + "px;height:" + c + "px;" + vmc_GetPositionExpand(d));

    }
}

function vmc_CollapseBanner(a) {
    var obj = $("#" + a);
    clearTimeout(hoverBannerTimeOut);
    if (obj != null)
        obj.attr("style", "z-index:0;overflow:hidden; position:relative;width:" + obj.attr("w") + "px;height:" + obj.attr("h") + "px;");
}

function RunBanner(rawdata, zone_id) {
    this.current = 0;
    this.zoneId = zone_id;
    this.zoneName = '';
    this.len = rawdata.length; // > 3 ? rawdata.length : 3;
    this.data = rawdata;

    if (this.len > 0) {
        var cc = readCookie(zone_id);
        if (cc != null && cc.length > 0) {
            this.current = eval(cc) + 1;
            if (this.current >= this.len) this.current = 0;
        }
        else {
            this.current = Math.floor(Math.random() * this.len);
        }
        createCookie(zone_id, this.current, 1);

    }


    this.Show = function () {
        if (this.len > 0)
            this.ChangeBanner();
    };

    this.ChangeBanner = function () {
        try {

            this.AdsvnRenderBanner(this.data[this.current]);
        }
        catch (e) {
            console.log(e.message + ':loi');
        }
    };

    this.AdsvnRenderBanner = function (obj) {

        if (typeof obj == undefined) return;
        if (obj.Link.length == 0) obj.Link = location.href;
        /*track impression*/
        adVnn.push(obj.ID);
        arrayAdsInPage.push(obj);
        /*het track impression*/

        if (obj.Type == 1) {
            document.write('<div class="vmc_ads_viewport" advId="' + obj.ID + '" style="z-index:555; margin:0 0 5px 0; padding:0;width:' + obj.Width + 'px;' + obj.Style + '"><a href="' + AdsvnRenderLink(obj.ID, obj.Link) + '" target="_blank"><img style="border:none; padding:0; margin:0;" src="' + obj.src + '" width="' + obj.Width + 'px" height="' + obj.Height + '"/></a>' + vadslogo + '</div>');
            if (obj.Img.length > 0) {
                document.write('<iframe src="' + obj.Img + (obj.Img.indexOf('?') != -1 ? "&r=" : "?r=") + Math.random() + '" width="0" style="display:none" height="0"></iframe>');
            }
        }
        else if (obj.Type == 2) {
            if (isSupportFlash())
                document.write('<div class="vmc_ads_viewport" advId="' + obj.ID + '" style="z-index:555; margin:0 0 5px 0; padding:0;width:' + obj.Width + 'px;' + obj.Style + '">' + GetFlashObject(obj.src, AdsvnRenderLink(obj.ID, obj.Link), obj.Width, obj.Height) + vadslogo + '</div>');
            else {
                
                document.write('<div class="vmc_ads_viewport" advId="' + obj.ID + '"  style="z-index:555; margin:0 0 5px 0; padding:0;width:' + obj.Width + 'px;' + obj.Style + '"><a href="' + AdsvnRenderLink(obj.ID, obj.Link) + '" target="_blank"><img style="border:none; padding:0; margin:0;" src="' + obj.Img + '" width="' + obj.Width + 'px" height="' + obj.Height + '"/></a></div>');
            }
        }
        else if (obj.Type == 3 || (obj.Type == 4 && !isIE())) {
            document.writeln('<div class="vmc_ads_viewport" advId="' + obj.ID + '"   style="z-index:555; margin:0 0 0px 0; padding:0;width:' + obj.Width + 'px;' + obj.Style + '">' + obj.src + '</div>');
        }
        else if (obj.Type == 5) {
            //TVC
        }
        else if (obj.Type == 6) {
            var divExpand = "divExpand" + (new Date()).getTime();
            if (isSupportFlash()) {
                document.write('<div class="vmc_ads_viewport" advId="' + obj.ID + '"   id="' + divExpand + '" style="z-index:555; width: 980px; height: 50px; overflow: hidden;"><embed quality="high"  allowscriptaccess="always" flashvars="div_id=' + divExpand + '&stringURL=' + encodeURIComponent(obj.Link) + '" wmode="transparent" pluginspage="http://www.macromedia.com/go/getflashplayer" type="application/x-shockwave-flash" src="' + obj.src + '" height="480" width="980">' + vadslogo + '</div>');
            }
            else {
                document.write('<div class="vmc_ads_viewport" advId="' + obj.ID + '"   style="z-index:555; margin:0 0 5px 0; padding:0;width:980px;' + obj.Style + '"><a href="' + AdsvnRenderLink(obj.ID, obj.Link) + '" target="_blank"><img style="border:none; padding:0; margin:0;" src="' + obj.Img + '" width="980px" height="50px"/></a></div>');
            }
        }
        else if (obj.Type == 7) {

            var divID = "div_" + obj.Zone_ID + "_" + (new Date().getTime());
            if (isSupportFlash()) {
                document.write(addCss + '<div class="vmc_ads_viewport"  advId="' + obj.ID + '"  style="z-index:555; margin:0 0 5px 0; padding:0;"><div id="' + divID + '" onmouseover="AdsvnMouseOver(this)" w="' + obj.Width + '" h="' + obj.Height + '"  style="position:relative; overflow:hidden; padding:0;width:' + obj.Width + 'px;height:' + obj.Height + 'px;' + obj.Style + '"><div style="position:absolute;left:0;top:0; background-color:#fff;"><embed src="' + obj.src + '"  ' + (obj.Link.length > 0 ? 'flashvars="zoneid=' + divID + '&amp;stringURL=' + encodeURIComponent(obj.Link) + '"' : '') + ' width="100%" height=100%" pluginspage="http://www.macromedia.com/go/getflashplayer" type="application/x-shockwave-flash" allowscriptaccess="always" wmode="transparent" quality="high" ></embed></div></div>' + vadslogo + '</div>');
            }
            else {
                document.write('<div class="vmc_ads_viewport"  advId="' + obj.ID + '"  w="' + obj.Width + '" h="' + obj.Height + '"  style="z-index:555; margin:0 0 5px 0; padding:0;width:' + obj.Width + 'px; height:' + obj.Height + 'px;' + obj.Style + '"><a href="' + AdsvnRenderLink(obj.ID, obj.Link) + '" target="_blank"><img style="border:none; padding:0; margin:0;" src="' + obj.Img + '" width="100%" height="100%"/></a>' + vadslogo + '</div>');
            }
        }
        else if (obj.Type == 9) {
            var divID = "div_" + obj.Zone_ID + "_" + (new Date().getTime());
            if (isSupportFlash()) {
                document.write(addCss + '<div class="vmc_ads_viewport"  advId="' + obj.ID + '"  style="z-index:555; margin:0 0 5px 0; padding:0;"><div id="' + divID + '"  style="position:fixed; overflow:hidden; padding:0; right:5px; bottom:5px;"><embed src="' + obj.src + '"  flashvars="div_id=' + divID + '&amp;stringURL=' + escape(AdsvnRenderLink(obj.ID, obj.Link)) + '" width="' + obj.Width + 'px" height="' + obj.Height + 'px" pluginspage="http://www.macromedia.com/go/getflashplayer" type="application/x-shockwave-flash" allowscriptaccess="always" wmode="transparent" quality="high" ></embed>' + vadslogo + '</div>');
            }
        }
        else if (obj.Type == 10) //Banner Catfish
        {
            setTimeout(function () { $("#catfish").hide(); }, 10000);
            document.write('<div id="catfish" style="width: 100%;position:fixed;bottom:0;left:0;"><div style="width:300px;height:50px;margin:0 auto"><img src="' + obj.src + '" style="display:none"/><a href="' + obj.Link + '"> <img src="' + obj.src + '" style="display: block;margin: 0 auto" /></a></div></div>');
        }
        else if (obj.Type == 11) //Banner Popup
        {
            var a = '<div id="vmcFullbg" style="z-index:10000;background:#000;position:fixed;top:0; left:0; height:100%; width:100%;"><span></span></div><div id="adv_ttv_mobile_3000" style="z-index:10000;width: 100%;height: 100%;position:absolute;top:0; left:0;"><div style="position:relative;clear:both;width: 100%;height: 100%;"><div id="ban_pop" style="clear:both;position: relative"><div id="cl_pop_bt" style="overflow-y: hidden; position: absolute; top: 20px; right: 0px;"><a id="adv_ttv_close" href="javascript:void(0)" onclick="close_avt()" class="adv_ttv_close_adv" style="height:32px;display:inline-block;"><span style="display: block; text-align: center; font-family: arial; float: right; font-size: 13px; border-radius: 3px 0px 0px 3px; font-weight: bold; background-color:#ffffff; color: #ff0000; padding: 0px 10px; height: 31px; line-height: 31px;">\u0110\u00f3ng <i style="font-weight: bold; display: inline-block; height: 100%; vertical-align: top; font-size: 21px; padding-top: 1px;">\u00d7</i></span></a></div><a id="log-ads" style="position: absolute;top: 0;right: 0;"><img src="http://img.vietnamnetad.vn/Images/logo/vads-logo.png" height="17"></a>';
            a += '<img src="' + obj.Link + '" style="display:none"/><a rel="nofollow" href="' + obj.Link + '" target="_blank" id="adv_ttv_mobile_3000_a" style="clear:both; display:block; text-decoration:none;border:0;"><img border="0" width="100%" style="display:block;margin:0 auto" id="img_adv_ttv_mobile_3000" vspace="0" hspace="0" src="' + obj.src + '" alt="Qu\u1ea3ng c\u00e1o" /></a></div></div></div>';
            vmcTimeout = setTimeout('close_avt()', 7000);
            document.write(a);
        }
        else if (obj.Type == 12) //Banner Background Inpage
        {
            renderBannerBackground(obj);
        }
    }
}

var vmcTimeout;


function close_avt() {
    document.getElementById("adv_ttv_mobile_3000").style.display = "none";
    document.getElementById("vmcFullbg").style.display = "none";
    vmcTimeout && window.clearTimeout(vmcTimeout);
}

function AdsvnMouseOver(obj) {
    obj.style.overflow = 'visible';
}

function AdsvnMouseOut(obj) {
    obj.style.overflow = 'hidden';
}

function AdsvnRenderClick(itemId) {
    if (itemId <= 0) return;
    var img = new Image();
    img.src = logLocation + "/Pages/Ads/log.ashx?type=click&itemId=" + itemId + '&location=' + encodeURIComponent(location.href);
}

function isIE() {
    return (navigator.appName == 'Microsoft Internet Explorer');
}

function AdsvnRenderLink(itemId, link) {
    return logLocation + "/Pages/Ads/log.ashx?type=click&itemId=" + itemId + '&isLink=1&location=' + encodeURIComponent(location.href) + (link ? ("&nextUrl=" + encodeURIComponent(link)) : "");
}

 

var jsTimeout = new Number();
var jsTimeDelay = 60;
var jsAcceleration = 0.2;
var jsVelocity = 3;

function jsAnimate(div_id, attr, value) {
    var _originValue = new Number();
    var _splitArr = new Array();

    _splitArr = String(document.getElementById(div_id).style[attr]).split("px");
    _originValue = parseInt(_splitArr[0]);
    clearTimeout(jsTimeout);
    if (parseInt(value) > _originValue) {
        jsTimeout = setTimeout('animateProcess("' + div_id + '", "' + attr + '", "increase", "' + _originValue + '", "' + value + '", 1)', jsTimeDelay);
    } else if (parseInt(value) < _originValue) {
        jsTimeout = setTimeout('animateProcess("' + div_id + '", "' + attr + '", "increase", "' + _originValue + '", "' + value + '", 1)', jsTimeDelay);
    }
}

function animateProcess(div_id, attr, type, begin, end, timing) {
    clearTimeout(jsTimeout);
    var _distance = (jsAcceleration * Math.pow(parseInt(timing), 2)) / 2 + jsVelocity * parseInt(timing);
    var _s = new Number();
    switch (type) {
        case "increase":
            _s = parseInt(begin) + parseInt(_distance);
            if (_s >= parseInt(end)) {
                document.getElementById(div_id).style[attr] = end + "px";
            } else {
                document.getElementById(div_id).style[attr] = _s + "px";
                timing++;
                jsTimeout = setTimeout('animateProcess("' + div_id + '", "' + attr + '", "' + type + '", "' + _s + '", "' + end + '", ' + timing + ')', jsTimeDelay);
            }
            break;
        case "decrease":
            _s = parseInt(begin) - parseInt(_distance);
            if (_s <= parseInt(end)) {
                document.getElementById(div_id).style[attr] = end + "px";
            } else {
                document.getElementById(div_id).style[attr] = _s + "px";
                timing++;
                jsTimeout = setTimeout('animateProcess("' + div_id + '", "' + attr + '", "' + type + '", "' + _s + '", "' + end + '", ' + timing + ')', jsTimeDelay);
            }
            break;
    }
}

function hookExpand(objID, val, isExpand) {
    var obj = document.getElementById(objID);
    if (obj == null) return;

    if (isExpand) {
        // Scroll to this Obj
        var desty = obj.offsetTop;
        var thisNode = obj;
        while (thisNode.offsetParent &&
			  (thisNode.offsetParent != document.body)) {
            thisNode = thisNode.offsetParent;
            desty += thisNode.offsetTop;
        }
        window.scrollTo(0, desty);
    }

    obj.style.clip = val;
}

function hookExpandOverflow(objID, val, isExpand) {
    var obj = document.getElementById(objID);
    if (obj == null) return;
    obj.style.clip = val;
    if (isExpand) {
        // Scroll to this Obj
        var desty = obj.offsetTop;
        var thisNode = obj;
        while (thisNode.offsetParent &&
			  (thisNode.offsetParent != document.body)) {
            thisNode = thisNode.offsetParent;
            desty += thisNode.offsetTop;
        }
        window.scrollTo(0, desty);

        // Remove parent overflow: hidden
        var pNode = obj.parentNode;
        while (pNode.parentNode &&
			  (pNode.parentNode != document.body)) {
            if (pNode.style.overflow != 'visible') pNode.style.overflow = 'visible';
            if (parseInt(pNode.offsetHeight) >= 500) break;
            pNode = pNode.parentNode;
        }
    }
}


jQuery.fn.isOnScreen = function () {
    var win = jQuery(window);
    var viewport = {
        top: win.scrollTop(),
        left: win.scrollLeft()
    };
    viewport.right = viewport.left + win.width();
    viewport.bottom = viewport.top + win.height();
    var bounds = this.offset();
    bounds.right = bounds.left + this.outerWidth();
    bounds.bottom = bounds.top + this.outerHeight();
    return (!(viewport.right < bounds.left || viewport.left > bounds.right || viewport.bottom < bounds.top || viewport.top > bounds.bottom));
};

var arrayIsInviewport = new Array();
/*Nhung quang cao co tren trang*/
var arrayAdsInPage = new Array();

var vmcScrollTimeout = null;
var vmcScrollendDelay = 500;

function vmcAppendFrame(url) {
    var iframe = document.createElement('iframe');
    iframe.style.display = "none";
    iframe.src = url;
    document.body.appendChild(iframe);
}

function scrollbeginHandler() { }

function scrollendHandler() {
     vmcScrollTimeout = null;
}

var isVnnLoaded = false;
window.onload = function() {
    if (adVnn.length > 0 && !isVnnLoaded) {
        isVnnLoaded = true;
        console.log('log call view');
        (new Image()).src = logLocation + "/Pages/Ads/log.ashx?type=impression&itemIds=" + adVnn.join(",") + '&location=' + encodeURIComponent(location.href) + "&trueImpression=0&t=" + new Date();

    }
};

window.onbeforeunload = function () {
    if (adVnn.length > 0 && !isVnnLoaded) {
        isVnnLoaded = true;
        console.log('log call view');
        (new Image()).src = logLocation + "/Pages/Ads/log.ashx?type=impression&itemIds=" + adVnn.join(",") + '&location=' + encodeURIComponent(location.href) + "&trueImpression=0&t=" + new Date();

    }
};

/*Sticky*/
jQuery.fn.extend({
    VADSSticky: function (e, t, n) {
        var r = $(this);
        if ($("#" + t).length == 0 || $("#" + e).length == 0 || $("#" + n).length == 0) return;
        var ot = $("#" + t);
        var oe = $("#" + e);
        var o = $("#" + n);
		 
        r.scroll(function () {
            var i = ot.offset().top;
            var s = oe.offset().top;
            var u = o.height();
            var e = r.scrollTop();
            if (s <= e) {
                if (e + u >= i) {
                    o.css({
                        top: -1 * (u - (i - e) + 20) + "px",
                        position: "fixed"
                    })
                } else {
                    o.css({
                        top: "5px",
                        position: "fixed"
                    });
                }
            } else {
                o.css({
                    top: "",
                    position: ""
                })
            }
        })
    }
});
 
/*background banner*/
var imgHost = '';
function renderBannerBackground(obj) {
     
    return "";
};

function vmcloadJs(c, b) {
    var a = document.createElement("script");
    a.type = "text/javascript";
    a.src = c;
    2 <= arguments.length && (a.onload = b, a.onreadystatechange = function () {
        4 != a.readyState && "complete" != a.readyState || b();
    });
    document.getElementsByTagName("head")[0].appendChild(a);
};
