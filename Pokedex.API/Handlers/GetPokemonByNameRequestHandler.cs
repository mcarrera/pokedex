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
                // I am well aware we are doing 2 requests here. Altought not ideal for perfomances
                // this is convenient for readibility. In a production scenario, I would consider
                // creating my own service to consume pokeapi.com
                // (similar to how I did for the funtranslation.com api)

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
