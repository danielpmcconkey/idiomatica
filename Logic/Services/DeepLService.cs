using DeepL;
using Logic.Telemetry;
using Microsoft.Extensions.Logging;
using Model;

namespace Logic.Services
{
    public class DeepLService
    {
        private ILogger<IdiomaticaLogger> _logger;

        public DeepLService(ILogger<IdiomaticaLogger> logger)
        {
            _logger = logger;
        }
        public async Task<string> TranslateAsync (string input, string sourceLanguageCode, string targetLanguageCode) 
        {
            try
            {
                var authKey = Environment.GetEnvironmentVariable("DeepLApiKey");
                var translator = new Translator(authKey);

                var translatedText = await translator.TranslateTextAsync(
                      input,
                      sourceLanguageCode,
                      targetLanguageCode);
                return translatedText.ToString();
            }
            catch (Exception ex)
            {
                string[] args = [
                    $"input = {input}",
                    $"sourceLanguageCode = {sourceLanguageCode}",
                    $"targetLanguageCode = {targetLanguageCode}",
                    ];
                ErrorHandler.LogAndThrow(3010, args, ex);
                throw; // you'll never get here
            } 
        }

        public string Translate(string input, string sourceLanguageCode, string targetLanguageCode)
        {
            try
            {
                var t = Task.Run(() => TranslateAsync(input, sourceLanguageCode, targetLanguageCode));
                t.Wait();
                return t.Result;
            }
            catch (Exception ex)
            {
                string[] args = [
                    $"input = {input}",
                    $"sourceLanguageCode = {sourceLanguageCode}",
                    $"targetLanguageCode = {targetLanguageCode}",
                    ];
                ErrorHandler.LogAndThrow(3010, args, ex);
                throw; // you'll never get here
            }
        }

    }
}
