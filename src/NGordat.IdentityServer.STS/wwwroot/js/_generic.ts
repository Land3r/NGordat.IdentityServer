const ICON_REVEAL_PASSWORD_ON: string = 'fa-eye';
const ICON_REVEAL_PASSWORD_OFF: string = 'fa-eye-slash';


function toggleReveal(elementId: string, revealBtnId: string) {
    let inputElm = <HTMLInputElement>document.getElementById(elementId);
    let iconElm = document.getElementById(revealBtnId);

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