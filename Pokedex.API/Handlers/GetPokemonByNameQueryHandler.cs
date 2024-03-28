using MediatR;
using Pokedex.API.Handlers.Dtos;
using Pokedex.API.Handlers.Queries;
using Pokedex.API.Services;

namespace Pokedex.API.Handlers
{
    public class GetPokemonByNameQueryHandler : IRequestHandler<GetPokemonByNameRequest, PokemonInfoDto>
    {
        private readonly ILogger<GetPokemonByNameQueryHandler> _logger;
        private readonly PokemonService _pokemonService;

        public GetPokemonByNameQueryHandler(ILogger<GetPokemonByNameQueryHandler> logger, PokemonService pokemonService)
        {
            _logger = logger;
            _pokemonService = pokemonService;

        }

        public async Task<PokemonInfoDto> Handle(GetPokemonByNameRequest request, CancellationToken cancellationToken)
        {
            try
            {
                return await _pokemonService.GetPokemonByNameAsync(request.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Error in GetPokemonByNameQueryHandler.Handle ${0}");
                throw;
            }
        }
    }
}
