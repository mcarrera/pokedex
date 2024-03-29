using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PokeApiNet;
using Pokedex.API.Controllers;
using Pokedex.API.Handlers.Dtos;
using Pokedex.API.Handlers.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokedex.API.Tests
{
    public class PokemonControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<ILogger<PokemonController>> _loggerMock;
        private readonly PokemonController _controller;
        private readonly Fixture _fixture;

        public PokemonControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<PokemonController>>();
            _controller = new PokemonController(_mediatorMock.Object, _loggerMock.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetPokemonByName_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var expectedResult = _fixture.Create<PokemonInfoDto>();
            var request = _fixture.Create<GetPokemonByNameRequest>();

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetPokemonByNameRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.GetPokemonByName(It.IsAny<string>());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedResult, okResult.Value);
        }

        [Fact]
        public async Task GetPokemonByName_ReturnsNotFound_WhenPokemonDoesNotExist()
        {
            // Arrange
            var pokemonName = "invalid";
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetPokemonByNameRequest>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync((PokemonInfoDto)null!);

            // Act
            var result = await _controller.GetPokemonByName(pokemonName);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetPokemonByName_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange

            var pokemonName = "invalid";

            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<GetPokemonByNameRequest>(), default))
                        .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetPokemonByName(pokemonName);


            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

    }
}
