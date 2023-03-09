import { getCookie, setCookie } from 'typescript-cookie'

function setApplicationTheme(themeName: string) {
    // The following value must match ThemeHelpers.CookieThemeKey
    const cookieName: string = "Application.Theme"
    setCookie(cookieName, themeName);
}

function toggleReveal(elementId: string, revealBtnId: string) {
    const iconOn: string = "io-eye";
    const iconOff: string = "io-eye-off";

    let inputElm = <HTMLInputElement>document.getElementById(elementId);
    let iconElm = document.getElementById(revealBtnId);

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