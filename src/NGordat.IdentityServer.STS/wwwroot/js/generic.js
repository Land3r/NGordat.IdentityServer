function toggleReveal(elementId, revealBtnId) {
    var inputElm = document.getElementById(elementId);
    var iconElm = document.getElementById(revealBtnId);
    if (inputElm.type == 'password') {
        inputElm.type = 'text';
        iconElm.classList.toggle("io-eye");
        iconElm.classList.toggle("io-eye-off");
    }
    else if (inputElm.type == 'text') {
        inputElm.type = 'password';
        iconElm.classList.toggle("io-eye");
        iconElm.classList.toggle("io-eye-off");
    }
}
//# sourceMappingURL=generic.js.map