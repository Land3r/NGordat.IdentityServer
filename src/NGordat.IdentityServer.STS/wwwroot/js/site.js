var ICON_REVEAL_PASSWORD_ON = 'fa-eye';
var ICON_REVEAL_PASSWORD_OFF = 'fa-eye-slash';
function toggleReveal(elementId, revealBtnId) {
    var inputElm = document.getElementById(elementId);
    var iconElm = document.getElementById(revealBtnId);
    if (inputElm.type == 'password') {
        inputElm.type = 'text';
        iconElm.classList.toggle(ICON_REVEAL_PASSWORD_ON);
        iconElm.classList.toggle(ICON_REVEAL_PASSWORD_OFF);
    }
    else if (inputElm.type == 'text') {
        inputElm.type = 'password';
        iconElm.classList.toggle(ICON_REVEAL_PASSWORD_ON);
        iconElm.classList.toggle(ICON_REVEAL_PASSWORD_OFF);
    }
}
//# sourceMappingURL=site.js.map