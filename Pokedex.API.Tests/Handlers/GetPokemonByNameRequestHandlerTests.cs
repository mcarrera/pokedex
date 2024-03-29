using Microsoft.Extensions.Logging;
using Moq;
using PokeApiNet;
using Pokedex.API.Handlers;
using Pokedex.API.Handlers.Queries;

namespace Pokedex.API.Tests.Handlers
{
    public class GetPokemonByNameRequestHandlerTests
    {
        private readonly Mock<ILogger<GetPokemonByNameRequestHandler>> loggerMock;
        private readonly Mock<IPokeApiClientWrapper> pokeApiClientMock;

        public GetPokemonByNameRequestHandlerTests()
        {
            loggerMock = new Mock<ILogger<GetPokemonByNameRequestHandler>>();
            pokeApiClientMock = new Mock<IPokeApiClientWrapper>();
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsPokemonInfoDto()
        {
            // Arrange

            var pokemonName = "pikachu";
            var expectedSpecies = new PokemonSpecies { Name = "pikachu", Habitat = new NamedApiResource<PokemonHabitat> { Name = "forest" }, IsLegendary = false, FlavorTextEntries = new List<PokemonSpeciesFlavorTexts> { new PokemonSpeciesFlavorTexts { FlavorText = "some description" } } };
            var expectedPokemon = new Pokemon { Name = "pikachu", Species = new NamedApiResource<PokemonSpecies> { Name = "pikachu" } };

            pokeApiClientMock.Setup(client => client.GetResourceAsync<Pokemon>(pokemonName)).ReturnsAsync(expectedPokemon);
            pokeApiClientMock.Setup(client => client.GetResourceAsync<PokemonSpecies>(expectedPokemon.Species.Name)).ReturnsAsync(expectedSpecies);

            var handler = new GetPokemonByNameRequestHandler(loggerMock.Object, pokeApiClientMock.Object);
            var request = new GetPokemonByNameRequest(pokemonName);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedPokemon.Name, result.Name);
            Assert.Equal(expectedSpecies.Habitat.Name, result.Habitat);
            Assert.Equal(expectedSpecies.IsLegendary, result.IsLegendary);
            Assert.Equal(expectedSpecies.FlavorTextEntries.First().FlavorText, result.Description);
        }

        [Fact]
        public async Task Handle_ExceptionThrown_LogsError()
        {
            // Arrange
            var pokemonName = "Any Pokemon";

            pokeApiClientMock.Setup(client => client.GetResourceAsync<Pokemon>(It.IsAny<string>())).ThrowsAsync(new Exception("Test exception"));

            var handler = new GetPokemonByNameRequestHandler(loggerMock.Object, pokeApiClientMock.Object);
            var request = new GetPokemonByNameRequest(pokemonName);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(request, CancellationToken.None));

            loggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);
        }

    }
}
