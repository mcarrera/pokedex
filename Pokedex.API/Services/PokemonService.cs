using Newtonsoft.Json;
using Pokedex.API.Handlers.Dtos;

namespace Pokedex.API.Services
{
    public class PokemonService
    {
        private readonly HttpClient _httpClient;

        public PokemonService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PokemonInfoDto> GetPokemonByNameAsync(string pokemonName)
        {
            var response = await _httpClient.GetAsync($"/api/v2/pokemon/{pokemonName}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<PokemonInfoDto>(content);
            }

            return null;
        }
    }
}
