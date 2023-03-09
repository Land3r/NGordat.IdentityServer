define("site", ["require", "exports", "typescript-cookie"], function (require, exports, typescript_cookie_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    function setApplicationTheme(themeName) {
        // The following value must match ThemeHelpers.CookieThemeKey
        var cookieName = "Application.Theme";
        typescript_cookie_1.setCookie(cookieName, themeName);
    }
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
});
//# sourceMappingURL=site.js.map