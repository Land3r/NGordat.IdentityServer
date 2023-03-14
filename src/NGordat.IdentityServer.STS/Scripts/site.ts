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