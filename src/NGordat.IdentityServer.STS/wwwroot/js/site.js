function toggleReveal(elementId, revealBtnId) {
    var iconOn = "io-eye";
    var iconOff = "io-eye-off";
    var inputElm = document.getElementById(elementId);
    var iconElm = document.getElementById(revealBtnId);
    if (inputElm.type == 'password') {
        inputElm.type = 'text';
        iconElm.classList.toggle(iconOn);
        iconElm.classList.toggle(iconOff);
    }
    else if (inputElm.type == 'text') {
        inputElm.type = 'password';
        iconElm.classList.toggle(iconOn);
        iconElm.classList.toggle(iconOff);
    }
}
var Cookie = /** @class */ (function () {
    function Cookie() {
    }
    Cookie.set = function (name, value, options) {
        options = options || {};
        var expires = options.expires;
        if (typeof expires == "number" && expires) {
            var d = new Date();
            d.setTime(d.getTime() + expires * 1000);
            expires = options.expires = d;
        }
        if (expires && expires.toUTCString) {
            options.expires = expires.toUTCString();
        }
        value = encodeURIComponent(value);
        var updatedCookie = name + "=" + value;
        var propName;
        for (propName in options) {
            updatedCookie += "; " + propName;
            var propValue = options[propName];
            if (propValue !== true) {
                updatedCookie += "=" + propValue;
            }
        }
        document.cookie = updatedCookie;
    };
    Cookie.get = function (name) {
        var matches = document.cookie.match(new RegExp("(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"));
        return matches ? decodeURIComponent(matches[1]) : '';
    };
    return Cookie;
}());
function setCookie(name, value, options) {
    Cookie.set(name, value, options);
}
function getCookie(name) {
    return Cookie.get(name);
}
function deleteCookie(name) {
    setCookie(name, "", {
        expires: -1
    });
}
//# sourceMappingURL=site.js.map