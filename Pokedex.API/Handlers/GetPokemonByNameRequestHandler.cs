using MediatR;
using PokeApiNet;
using Pokedex.API.Common;
using Pokedex.API.Handlers.Dtos;
using Pokedex.API.Handlers.Queries;


namespace Pokedex.API.Handlers
{
    public class GetPokemonByNameRequestHandler : IRequestHandler<GetPokemonByNameRequest, PokemonInfoDto>
    {
        private readonly ILogger<GetPokemonByNameRequestHandler> _logger;
        private readonly IPokeApiClientWrapper _pokeApiClient;

        public GetPokemonByNameRequestHandler(ILogger<GetPokemonByNameRequestHandler> logger, IPokeApiClientWrapper pokeApiClient)
        {
            _logger = logger;
            _pokeApiClient = pokeApiClient;
        }

        public async Task<PokemonInfoDto> Handle(GetPokemonByNameRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var pokeMon = await _pokeApiClient.GetResourceAsync<Pokemon>(request.Name);
                var species = await _pokeApiClient.GetResourceAsync<PokemonSpecies>(pokeMon.Species.Name);
                return new PokemonInfoDto(pokeMon.Name, Helpers.GetPokemonDescriptionFromSpecies(species), species.Habitat.Name, species.IsLegendary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Error in GetPokemonByNameQueryHandler.Handle ${0}");
                throw;
            }
        }

       
    }
}
