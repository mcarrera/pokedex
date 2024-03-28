using MediatR;
using Pokedex.API.Handlers.Dtos;

namespace Pokedex.API.Handlers.Queries
{
    public class GetPokemonByNameRequest : IRequest<PokemonInfoDto>
    {

        public GetPokemonByNameRequest(string pokemonName)
        {
            Name = pokemonName;
        }

        public string Name { get; }
    }
}
