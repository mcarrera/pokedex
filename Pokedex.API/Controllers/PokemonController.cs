using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pokedex.API.Handlers.Queries;

namespace Pokedex.API.Controllers
{

    /// <summary>
    /// Pokemon Controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PokemonController : ControllerBase
    {

        private readonly IMediator _mediator;
        private readonly ILogger<PokemonController> _logger;

        public PokemonController(IMediator mediator, ILogger<PokemonController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Returns a Pokemon with description and additional info.
        /// </summary>
        /// <param name="pokemonName">The name of the Pokemon to retrieve information for.</param>
        /// <returns>
        /// Returns a <see cref="IActionResult"/> containing the Pokemon information.
        /// If the Pokemon is found, returns a <see cref="OkObjectResult"/> with a <see cref="PokemonInfoDto"/> object.
        /// If the Pokemon is not found, returns a <see cref="NotFoundResult"/>.
        /// If an error occurs during processing, returns a <see cref="StatusCodeResult"/> with status code 500 (Internal Server Error).
        /// </returns>
        [HttpGet("{pokemonName}")]
        public async Task<IActionResult> GetPokemonByName(string pokemonName)
        {
            try
            {
                var response = await _mediator.Send(new GetPokemonByNameRequest(pokemonName));
                if (response == null)
                {
                    return NotFound();
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Error in GetPokemonInfo: {0} ");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        /// <summary>
        /// Returns a Pokemon with a translated description and additional info.
        /// </summary>
        /// <param name="pokemonName">The name of the Pokemon to retrieve information for.</param>
        /// <returns>
        /// Returns a <see cref="IActionResult"/> containing the Pokemon information.
        /// If the Pokemon is found, returns a <see cref="OkObjectResult"/> with a <see cref="PokemonInfoDto"/> object.
        /// If the Pokemon is not found, returns a <see cref="NotFoundResult"/>.
        /// If an error occurs during processing, returns a <see cref="StatusCodeResult"/> with status code 500 (Internal Server Error).
        /// </returns>
        [HttpGet("translated/{pokemonName}")]
        public async Task<IActionResult> GetPokemonByNameWithTranslatedDescription(string pokemonName)
        {
            try
            {
                var pokemonInfo = await _mediator.Send(new GetPokemonByNameTranslatedRequest(pokemonName));
                if (pokemonInfo == null)
                {
                    return NotFound();
                }
                return Ok(pokemonInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Error in GetPokemonByNameWithTranslatedDescription: {0} ");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
