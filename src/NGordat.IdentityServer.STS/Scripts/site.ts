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

type Options = {
    expires?: Date | number | string,
    path?: string,
    domain?: string,
    secure?: boolean,
}

class Cookie {
    static set(name: string, value: string, options?: Options) {
        options = options || {};

        let expires = options.expires;

        if (typeof expires == "number" && expires) {
            let d = new Date();
            d.setTime(d.getTime() + expires * 1000);
            expires = options.expires = d;
        }
        if (expires && (<Date>expires).toUTCString) {
            options.expires = (<Date>expires).toUTCString();
        }

        value = encodeURIComponent(value);

        let updatedCookie = name + "=" + value;

        let propName: keyof Options;
        for (propName in options) {
            updatedCookie += "; " + propName;
            let propValue = options[propName];
            if (propValue !== true) {
                updatedCookie += "=" + propValue;
            }
        }

        document.cookie = updatedCookie;
    }

    static get(name: string): string {
        let matches = document.cookie.match(
            new RegExp("(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"));
        return matches ? decodeURIComponent(matches[1]) : '';
    }
}

function setCookie(name: string, value: string, options?: Options) {
    Cookie.set(name, value, options)
}

function getCookie(name: string) {
    return Cookie.get(name);
}

function deleteCookie(name: string) {
    setCookie(name, "", {
        expires: -1
    })
}

function setApplicationTheme(themeName: string) {
    // Warning; The following name must match the one in ThemeHelpers.CookieThemeKey
    const cookieName: string = 'Application.Theme';
    // Warning; The following value must match the one in ThemeHelpers.NoThemeKey
    const noThemeKey: string = 'no-theme';
    const instantUpdate: boolean = true;
    const themeLinkId: string = "theme-link";

    setCookie(cookieName, themeName, {
        path: '/',
        secure: true,
    });

    if (instantUpdate) {
        const url = themeName === noThemeKey ?
            `https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.2.3/css/bootstrap.min.css` :
            `https://cdnjs.cloudflare.com/ajax/libs/bootswatch/5.2.3/${themeName}/bootstrap.min.css`;
        const onErrorUrl = themeName === noThemeKey ?
            "this.onerror=null;this.href='/lib/bootstrap/dist/css/bootstrap.min.css';" :
            `this.onerror=null;this.href='/lib/bootswatch/{$themeName}/bootstrap.min.css';`;
        let elm = <HTMLLinkElement>document.getElementById(themeLinkId);
        elm.href = url;
        elm.crossOrigin = null;
        //elm.onerror = onErrorUrl;
    }
}