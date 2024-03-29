namespace Pokedex.API.Handlers
{
    using System.Threading.Tasks;
    using PokeApiNet;

    public interface IPokeApiClientWrapper
    {
        Task<T> GetResourceAsync<T>(string name) where T : NamedApiResource;
    }

    public class PokeApiClientWrapper : IPokeApiClientWrapper
    {
        private readonly PokeApiClient _pokeApiClient;

        public PokeApiClientWrapper(PokeApiClient pokeApiClient)
        {
            _pokeApiClient = pokeApiClient;
        }

        public Task<T> GetResourceAsync<T>(string name) where T : NamedApiResource
        {
            return _pokeApiClient.GetResourceAsync<T>(name);
        }
    }

}
