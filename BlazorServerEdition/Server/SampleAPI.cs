using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace BlazorReCaptchaSample.Server
{
    public class SampleAPI
    {
        private IHttpClientFactory HttpClientFactory { get; }

        private IOptionsMonitor<reCAPTCHAVerificationOptions> reCAPTCHAVerificationOptions { get; }

        public SampleAPI(IHttpClientFactory httpClientFactory, IOptionsMonitor<reCAPTCHAVerificationOptions> reCAPTCHAVerificationOptions)
        {
            this.HttpClientFactory = httpClientFactory;
            this.reCAPTCHAVerificationOptions = reCAPTCHAVerificationOptions;
        }

        public async Task<(bool Success, string[] ErrorCodes)> Post(string reCAPTCHAResponse)
        {
            var url = "https://www.google.com/recaptcha/api/siteverify";
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"secret", this.reCAPTCHAVerificationOptions.CurrentValue.Secret},
                {"response", reCAPTCHAResponse}
            });

            var httpClient = this.HttpClientFactory.CreateClient();
            var response = await httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var verificationResponse = await response.Content.ReadAsAsync<reCAPTCHAVerificationResponse>();
            if (verificationResponse.Success) return (Success: true, ErrorCodes: new string[0]);

            return (
                Success: false,
                ErrorCodes: verificationResponse.ErrorCodes.Select(err => err.Replace('-', ' ')).ToArray());
        }
    }
}
