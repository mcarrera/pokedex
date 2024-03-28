using PokeApiNet;

namespace Pokedex.API.Common
{
    public static class Helpers
    {
        public static string? GetPokemonDescriptionFromSpecies(PokemonSpecies species)
        {
            if (species == null || !species.FlavorTextEntries.Any()) return null;
            return species.FlavorTextEntries.First().FlavorText.Replace("\r", " ").Replace("\n", " ").Replace("\f", " ");
        }
    }
}
