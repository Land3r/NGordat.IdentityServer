function toggleReveal(elementId: string, revealBtnId: string) {
    let inputElm = <HTMLInputElement>document.getElementById(elementId);
    let iconElm = document.getElementById(revealBtnId);

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