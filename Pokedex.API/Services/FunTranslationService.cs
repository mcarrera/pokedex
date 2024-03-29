
using Newtonsoft.Json;

namespace Pokedex.API.Services
{
    public class FunTranslationService : IFunTranslationService
    {
        private readonly ILogger<FunTranslationService> _logger;
        private readonly HttpClient _httpClient;
        public FunTranslationService(ILogger<FunTranslationService> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }
        public async Task<string> TranslateToShakespeareAsync(string text)
        {
            var url = $"/translate/shakespeare.json?text={text}";
            return await TranslateAsync(url);
        }

        public async Task<string> TranslateToYodaAsync(string text)
        {
            var url = $"/translate/yoda.json?text={text}";
            return await TranslateAsync(url);
        }

        private async Task<string> TranslateAsync(string url)
        {
            var result = string.Empty;
            try
            {
                var response = await _httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var translationResult = JsonConvert.DeserializeObject<TranslationResult>(content);
                result = translationResult?.Contents.Translated ?? string.Empty;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request error occurred in FunTranslationService.TranslateAsync.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred in FunTranslationService.TranslateAsync.");
            }
            return result;
        }
    }
}
