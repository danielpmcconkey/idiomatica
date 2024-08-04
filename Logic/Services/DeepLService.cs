using DeepL;
using Logic.Telemetry;
using Microsoft.Extensions.Logging;
using Model;
using System.Net;

namespace Logic.Services
{
    public static class DeepLService
    {
        public static async Task<string> TranslateAsync (
            string input, string sourceLanguageCode, string targetLanguageCode) 
        {
            return await Task<string?>.Run(() =>
            {
                return Translate(input, sourceLanguageCode, targetLanguageCode);
            });
        }

        public static string Translate(
            string input, string sourceLanguageCode, string targetLanguageCode)
        {
            var authKey = Environment.GetEnvironmentVariable("DeepLApiKey");
            if (string.IsNullOrEmpty(authKey))
            {
                ErrorHandler.LogAndThrow();
                return string.Empty;
            }
            var translator = new Translator(authKey);
#if DEBUG
            return "This is a stubbed result";
#else

            var translatedText = translator.TranslateTextAsync(
                    input,
                    sourceLanguageCode,
                    targetLanguageCode).Result;
            return translatedText.ToString();
#endif
        }

    }
}
