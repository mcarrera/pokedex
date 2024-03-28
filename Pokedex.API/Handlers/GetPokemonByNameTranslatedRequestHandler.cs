using MediatR;
using PokeApiNet;
using Pokedex.API.Common;
using Pokedex.API.Handlers.Dtos;
using Pokedex.API.Handlers.Queries;
using Pokedex.API.Services;

namespace Pokedex.API.Handlers
{
    public class GetPokemonByNameTranslatedRequestHandler : IRequestHandler<GetPokemonByNameTranslatedRequest, PokemonInfoDto>
    {
        private readonly ILogger<GetPokemonByNameTranslatedRequestHandler> _logger;
        private readonly PokeApiClient _pokeApiClient;
        private readonly IFunTranslationService _translationService;

        public GetPokemonByNameTranslatedRequestHandler(ILogger<GetPokemonByNameTranslatedRequestHandler> logger, PokeApiClient pokeApiClient, IFunTranslationService funTranslationService)
        {
            _logger = logger;
            _pokeApiClient = pokeApiClient;
            _translationService = funTranslationService;
        }

        public async Task<PokemonInfoDto> Handle(GetPokemonByNameTranslatedRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var pokeMon = await _pokeApiClient.GetResourceAsync<Pokemon>(request.Name);
                var species = await _pokeApiClient.GetResourceAsync(pokeMon.Species);
                string description = await GetTranslatedDescription(species);
                if(description == string.Empty)
                {
                    description = Helpers.GetPokemonDescriptionFromSpecies(species)??string.Empty;
                }
                return new PokemonInfoDto(pokeMon.Name, description, species.Habitat.Name, species.IsLegendary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Error in GetPokemonByNameQueryHandler.Handle ${0}");
                throw;
            }
        }

        private async Task<string> GetTranslatedDescription(PokemonSpecies species)
        {
            var description = Helpers.GetPokemonDescriptionFromSpecies(species);

            if (species.Habitat.Name.Equals("cave", StringComparison.InvariantCultureIgnoreCase) || species.IsLegendary)
            {
                description = await _translationService.TranslateToYodaAsync(description);
            }
            else
            {
                description = await _translationService.TranslateToShakespeareAsync(description);
            }

            return description;
        }
    }
}
