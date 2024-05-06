using DeepL;

namespace Logic.Services
{
    public class DeepLService
    {
        public DeepLService() { }
        public async Task<string> TranslateAsync (string input, string sourceLanguageCode, string targetLanguageCode) 
        {
            var authKey = ; // Replace with your key
            var translator = new Translator(authKey);

            var translatedText = await translator.TranslateTextAsync(
                  input,
                  sourceLanguageCode,
                  targetLanguageCode);
            return translatedText.ToString(); 
        }

        public string Translate(string input, string sourceLanguageCode, string targetLanguageCode)
        {
            var t = Task.Run(() => TranslateAsync(input, sourceLanguageCode, targetLanguageCode));
            //var user = Task.Run(() => UserService.GetLoggedInUserAsync()).GetAwaiter().GetResult();
            t.Wait();
            return t.Result;
        }

    }
}
