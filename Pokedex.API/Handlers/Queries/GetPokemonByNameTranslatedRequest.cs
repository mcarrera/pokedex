using MediatR;
using Pokedex.API.Handlers.Dtos;

namespace Pokedex.API.Handlers.Queries
{
    public class GetPokemonByNameTranslatedRequest : IRequest<PokemonInfoDto>
    {
        public GetPokemonByNameTranslatedRequest(string pokemonName)
        {
            Name = pokemonName;
        }

        public string Name { get; }

    }
}
