namespace Pokedex.API.Tests.Services
{
    using Microsoft.Extensions.Logging;
    using Moq;
    using Moq.Protected;
    using Newtonsoft.Json;
    using Pokedex.API.Services;
    using System.Net;
    using Xunit;

    public class FunTranslationServiceTests
    {

        [Fact]
        public async Task TranslateToShakespeareAsync_ShouldReturnTranslatedText()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"contents\": {\"translated\": \"Test translation\"}}"),
            };

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(message =>
                        message.Method == HttpMethod.Get
                        && message.RequestUri.ToString().Contains("/translate/shakespeare.json")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);

            var httpClient = new HttpClient(handlerMock.Object);
            httpClient.BaseAddress = new Uri("https://api.funtranslations.com");
            var loggerMock = new Mock<ILogger<FunTranslationService>>();
            var service = new FunTranslationService(loggerMock.Object, httpClient);

            // Act
            var result = await service.TranslateToShakespeareAsync("test-text");

            // Assert
            Assert.Equal("Test translation", result);
        }

        [Fact]
        public async Task TestTranslateToShakespeareAsyncReturnsEmptyStringOnException()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(message =>
                        message.Method == HttpMethod.Get
                        && message.RequestUri.ToString().Contains("/translate/shakespeare.json")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .Throws<HttpRequestException>();

            var httpClient = new HttpClient(handlerMock.Object);
            httpClient.BaseAddress = new Uri("https://api.funtranslations.com");
            var loggerMock = new Mock<ILogger<FunTranslationService>>();
            var service = new FunTranslationService(loggerMock.Object, httpClient);

            // Act
            var result = await service.TranslateToShakespeareAsync("test-text");

            // Assert
            Assert.Equal(string.Empty, result);
        }


        [Fact]
        public async Task TranslateToYodaAsync_ShouldReturnTranslatedText()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"contents\": {\"translated\": \"Test translation\"}}"),
            };

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(message =>
                        message.Method == HttpMethod.Get
                        && message.RequestUri.ToString().Contains("/translate/yoda.json")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);

            var httpClient = new HttpClient(handlerMock.Object);
            httpClient.BaseAddress = new Uri("https://api.funtranslations.com");
            var loggerMock = new Mock<ILogger<FunTranslationService>>();
            var service = new FunTranslationService(loggerMock.Object, httpClient);

            // Act
            var result = await service.TranslateToYodaAsync("test-text");

            // Assert
            Assert.Equal("Test translation", result);
        }


        [Fact]
        public async Task TestTranslateToYodaAsyncReturnsEmptyStringOnException()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(message =>
                        message.Method == HttpMethod.Get
                        && message.RequestUri.ToString().Contains("/translate/yoda.json")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .Throws<HttpRequestException>();

            var httpClient = new HttpClient(handlerMock.Object);
            httpClient.BaseAddress = new Uri("https://api.funtranslations.com");
            var loggerMock = new Mock<ILogger<FunTranslationService>>();
            var service = new FunTranslationService(loggerMock.Object, httpClient);

            // Act
            var result = await service.TranslateToYodaAsync("test-text");

            // Assert
            Assert.Equal(string.Empty, result);
        }

    }
}