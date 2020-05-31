declare var grecaptcha: any;

namespace My.reCAPTCHA {
    let scriptLoaded: Promise<void> | null = null;

    function waitScriptLoaded(resolve: () => void) {
        if (typeof (grecaptcha) !== 'undefined' && typeof (grecaptcha.render) !== 'undefined') resolve();
        else setTimeout(() => waitScriptLoaded(resolve), 100);
    }

    export function init() {

        const scripts = Array.from(document.getElementsByTagName('script'));
        if (!scripts.some(s => (s.src || '').startsWith('https://www.google.com/recaptcha/api.js'))) {
            const script = document.createElement('script');
            script.src = 'https://www.google.com/recaptcha/api.js?render=explicit';
            script.async = true;
            script.defer = true;
            document.head.appendChild(script);
        }

        if (scriptLoaded === null) scriptLoaded = new Promise(waitScriptLoaded);

        return scriptLoaded;
    }

    export function render(dotNetObj: any, selector: string, siteKey: string): number {
        return grecaptcha.render(selector, {
            'sitekey': siteKey,
            'callback': (response: string) => { dotNetObj.invokeMethodAsync('CallbackOnSuccess', response); },
            'expired-callback': () => { dotNetObj.invokeMethodAsync('CallbackOnExpired'); }
        });
    }

    export function getResponse(widgetId: number): string {
        return grecaptcha.getResponse(widgetId);
    }
}