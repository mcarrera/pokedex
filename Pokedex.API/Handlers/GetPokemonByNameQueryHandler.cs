using MediatR;
using Pokedex.API.Handlers.Dtos;
using Pokedex.API.Handlers.Queries;

namespace Pokedex.API.Handlers
{
    public class GetPokemonByNameQueryHandler : IRequestHandler<GetPokemonByNameRequest, PokemonInfoDto>
    {
        private readonly ILogger<GetPokemonByNameQueryHandler> _logger;

        public GetPokemonByNameQueryHandler(ILogger<GetPokemonByNameQueryHandler> logger)
        {
            _logger = logger;
        }

        public Task<PokemonInfoDto> Handle(GetPokemonByNameRequest request, CancellationToken cancellationToken)
        {
            try
            {
                return Task.FromResult(new PokemonInfoDto("fooo"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Error in GetPokemonByNameQueryHandler.Handle ${0}");
                throw;
            }
        }
    }
}
