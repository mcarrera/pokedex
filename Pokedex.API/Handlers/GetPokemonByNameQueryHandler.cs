using MediatR;
using PokeApiNet;
using Pokedex.API.Handlers.Dtos;
using Pokedex.API.Handlers.Queries;
using Pokedex.API.Services;

namespace Pokedex.API.Handlers
{
    public class GetPokemonByNameQueryHandler : IRequestHandler<GetPokemonByNameRequest, PokemonInfoDto>
    {
        private readonly ILogger<GetPokemonByNameQueryHandler> _logger;
        private readonly PokeApiClient _pokeApiClient;

        public GetPokemonByNameQueryHandler(ILogger<GetPokemonByNameQueryHandler> logger, PokeApiClient pokeApiClient)
        {
            _logger = logger;
            _pokeApiClient = pokeApiClient;

        }

        public async Task<PokemonInfoDto> Handle(GetPokemonByNameRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var pokeMon =  await _pokeApiClient.GetResourceAsync<Pokemon>(request.Name);
                var species = await _pokeApiClient.GetResourceAsync(pokeMon.Species);

                return new PokemonInfoDto(pokeMon.Name, GetDescriptionFromSpecies(species), 
                                          species.Habitat.Name, species.IsLegendary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Error in GetPokemonByNameQueryHandler.Handle ${0}");
                throw;
            }
        }

        private static string? GetDescriptionFromSpecies(PokemonSpecies species)
        {
            if (species == null|| !species.FlavorTextEntries.Any()) return null;
            return species.FlavorTextEntries.First().FlavorText.Replace("\r", " ").Replace("\n", " ").Replace("\f", " ");
        }
    }
}
