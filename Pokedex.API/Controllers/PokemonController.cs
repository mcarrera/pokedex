using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pokedex.API.Handlers.Queries;
using System.Net;

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
