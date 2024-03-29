using Microsoft.Extensions.Logging;
using Moq;
using PokeApiNet;
using Pokedex.API.Handlers;
using Pokedex.API.Handlers.Queries;
using Pokedex.API.Services;

namespace Pokedex.API.Tests.Handlers
{
    public class GetPokemonByNameTranslatedRequestHandlerTests
    {
        private readonly Mock<ILogger<GetPokemonByNameTranslatedRequestHandler>> loggerMock;
        private readonly Mock<IPokeApiClientWrapper> pokeApiClientMock;
        private readonly Mock<IFunTranslationService> funTranslationServiceMock;

        public GetPokemonByNameTranslatedRequestHandlerTests()
        {
            loggerMock = new Mock<ILogger<GetPokemonByNameTranslatedRequestHandler>>();
            pokeApiClientMock = new Mock<IPokeApiClientWrapper>();
            funTranslationServiceMock = new Mock<IFunTranslationService>();
        }

        [Fact]
       
        public async Task Handle_ValidRequest_ReturnsPokemonInfoDto_WithYodaTranslation()
        {
            // Arrange

            var pokemonName = "pikachu";
            var expectedSpecies = new PokemonSpecies { Name = "pikachu", Habitat = new NamedApiResource<PokemonHabitat> { Name = "cave" }, IsLegendary = false, FlavorTextEntries = new List<PokemonSpeciesFlavorTexts> { new PokemonSpeciesFlavorTexts { FlavorText = "some description" } } };
            var expectedPokemon = new Pokemon { Name = "pikachu", Species = new NamedApiResource<PokemonSpecies> { Name = "pikachu" } };
            var expectedTranslation = "Some Description This Is";

            pokeApiClientMock.Setup(client => client.GetResourceAsync<Pokemon>(pokemonName)).ReturnsAsync(expectedPokemon);
            pokeApiClientMock.Setup(client => client.GetResourceAsync<PokemonSpecies>(expectedPokemon.Species.Name)).ReturnsAsync(expectedSpecies);

            funTranslationServiceMock.Setup(client => client.TranslateToYodaAsync(It.IsAny<string>())).ReturnsAsync(expectedTranslation);

            var handler = new GetPokemonByNameTranslatedRequestHandler(loggerMock.Object, pokeApiClientMock.Object, funTranslationServiceMock.Object);
            var request = new GetPokemonByNameTranslatedRequest(pokemonName);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedPokemon.Name, result.Name);
            Assert.Equal(expectedSpecies.Habitat.Name, result.Habitat);
            Assert.Equal(expectedSpecies.IsLegendary, result.IsLegendary);
            Assert.Equal(expectedTranslation, result.Description);
        }

        [Fact]

        public async Task Handle_ValidRequest_ReturnsPokemonInfoDto_WithShakespeareTranslation()
        {
            // Arrange

            var pokemonName = "pikachu";
            var expectedSpecies = new PokemonSpecies { Name = "pikachu", Habitat = new NamedApiResource<PokemonHabitat> { Name = "anything" }, IsLegendary = false, FlavorTextEntries = new List<PokemonSpeciesFlavorTexts> { new PokemonSpeciesFlavorTexts { FlavorText = "some description" } } };
            var expectedPokemon = new Pokemon { Name = "pikachu", Species = new NamedApiResource<PokemonSpecies> { Name = "pikachu" } };
            var expectedTranslation = "some description Shakespeare style";

            pokeApiClientMock.Setup(client => client.GetResourceAsync<Pokemon>(pokemonName)).ReturnsAsync(expectedPokemon);
            pokeApiClientMock.Setup(client => client.GetResourceAsync<PokemonSpecies>(expectedPokemon.Species.Name)).ReturnsAsync(expectedSpecies);

            funTranslationServiceMock.Setup(client => client.TranslateToShakespeareAsync(It.IsAny<string>())).ReturnsAsync(expectedTranslation);

            var handler = new GetPokemonByNameTranslatedRequestHandler(loggerMock.Object, pokeApiClientMock.Object, funTranslationServiceMock.Object);
            var request = new GetPokemonByNameTranslatedRequest(pokemonName);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedPokemon.Name, result.Name);
            Assert.Equal(expectedSpecies.Habitat.Name, result.Habitat);
            Assert.Equal(expectedSpecies.IsLegendary, result.IsLegendary);
            Assert.Equal(expectedTranslation, result.Description);
        }
    }
}
