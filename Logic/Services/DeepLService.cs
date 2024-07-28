using DeepL;
using Logic.Telemetry;
using Microsoft.Extensions.Logging;
using Model;

namespace Logic.Services
{
    public static class DeepLService
    {
        
        public static async Task<string> TranslateAsync (
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
