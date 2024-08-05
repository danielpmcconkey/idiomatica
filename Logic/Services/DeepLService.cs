using DeepL;
using Logic.Telemetry;
using Microsoft.Extensions.Logging;
using Model;
using System.Net;

namespace Logic.Services
{
    public static class DeepLService
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public static async Task<string> TranslateAsync (
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
            string input, string sourceLanguageCode, string targetLanguageCode) 
        {
#if DEBUG
            return "This is a stubbed result";
#else
            var authKey = Environment.GetEnvironmentVariable("DeepLApiKey");
            if (string.IsNullOrEmpty(authKey))
            {
                ErrorHandler.LogAndThrow();
                return string.Empty;
            }
            var translator = new Translator(authKey);
            var translatedText = await translator.TranslateTextAsync(
                    input,
                    sourceLanguageCode,
                    targetLanguageCode);
            return translatedText.ToString();
#endif
        }

        public static string Translate(
            string input, string sourceLanguageCode, string targetLanguageCode)
        {
            return TranslateAsync(input, sourceLanguageCode, targetLanguageCode).Result;
        }

    }
}
