namespace Pokedex.API.Services
{
    public interface IFunTranslationService
    {
        Task<string> TranslateToYodaAsync(string text);
        Task<string> TranslateToShakespeareAsync(string text);
    }
}
