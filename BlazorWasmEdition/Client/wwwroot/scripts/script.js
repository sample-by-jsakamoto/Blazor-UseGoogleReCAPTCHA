var My;
(function (My) {
    var reCAPTCHA;
    (function (reCAPTCHA) {
        let scriptLoaded = null;
        function waitScriptLoaded(resolve) {
            if (typeof (grecaptcha) !== 'undefined' && typeof (grecaptcha.render) !== 'undefined')
                resolve();
            else
                setTimeout(() => waitScriptLoaded(resolve), 100);
        }
        function init() {
            const scripts = Array.from(document.getElementsByTagName('script'));
            if (!scripts.some(s => (s.src || '').startsWith('https://www.google.com/recaptcha/api.js'))) {
                const script = document.createElement('script');
                script.src = 'https://www.google.com/recaptcha/api.js?render=explicit';
                script.async = true;
                script.defer = true;
                document.head.appendChild(script);
            }
            if (scriptLoaded === null)
                scriptLoaded = new Promise(waitScriptLoaded);
            return scriptLoaded;
        }
        reCAPTCHA.init = init;
        function render(dotNetObj, selector, siteKey) {
            return grecaptcha.render(selector, {
                'sitekey': siteKey,
                'callback': (response) => { dotNetObj.invokeMethodAsync('CallbackOnSuccess', response); },
                'expired-callback': () => { dotNetObj.invokeMethodAsync('CallbackOnExpired'); }
            });
        }
        reCAPTCHA.render = render;
        function getResponse(widgetId) {
            return grecaptcha.getResponse(widgetId);
        }
        reCAPTCHA.getResponse = getResponse;
    })(reCAPTCHA = My.reCAPTCHA || (My.reCAPTCHA = {}));
})(My || (My = {}));
//# sourceMappingURL=script.js.map