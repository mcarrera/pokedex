namespace Pokedex.API.Services
{
    public class TranslationResult
    {
        public Success Success { get; set; } = new Success();
        public Contents Contents { get; set; } = new Contents();
    }

    public class Success
    {
        public int Total { get; set; }
    }

    public class Contents
    {
        public string Translated { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string Translation { get; set; } = string.Empty;
    }

}
