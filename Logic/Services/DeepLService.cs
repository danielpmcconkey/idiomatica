using DeepL;
using Logic.Telemetry;
using Microsoft.Extensions.Logging;
using Model;
using Model.Enums;
using System.Net;

namespace Logic.Services
{
    public static class DeepLService
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public static async Task<string> TranslateAsync (
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
            string input, AvailableLanguageCode sourceLanguageCode, AvailableLanguageCode targetLanguageCode) 
        {
            string deeplCodeSource = ConvertCode(sourceLanguageCode);
            string deeplCodeTarget = ConvertCode(targetLanguageCode);
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
                    deeplCodeSource,
                    deeplCodeTarget);
            return translatedText.ToString();
#endif
        }

        public static string Translate(
            string input, AvailableLanguageCode sourceLanguageCode, AvailableLanguageCode targetLanguageCode)
        {
            return TranslateAsync(input, sourceLanguageCode, targetLanguageCode).Result;
        }

        private static string ConvertCode(AvailableLanguageCode code)
        {
            string codeString = code.ToString();
            return codeString.Replace('_', '-');
        }
    }
}
