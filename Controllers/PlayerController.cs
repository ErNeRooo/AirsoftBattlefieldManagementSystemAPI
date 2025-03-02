using AirsoftBattlefieldManagementSystemAPI.Models.Dtos;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftBattlefieldManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly BmsDbContext _dbContext;
        private readonly ILogger<PlayerController> _logger;
        private readonly IMapper _mapper;

        public PlayerController(ILogger<PlayerController> logger, BmsDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("id/{id}")]
        public ActionResult<PlayerDto> GetPlayer(int id)
        {
            Player player = _dbContext.Players.FirstOrDefault(p => p.PlayerId == id);

            if (player == null) return NotFound("Player not found");

            PlayerDto playerDto = _mapper.Map<PlayerDto>(player);

            return Ok(playerDto);
        }

        [HttpPost("")]
        public ActionResult<string> PostPlayer([FromBody] CreatePlayerDto playerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var player = _mapper.Map<Player>(playerDto);

            _dbContext.Players.Add(player);
            _dbContext.SaveChanges();

            return Created($"/player/{player.PlayerId}", null);
        }

        [HttpPut("id/{id}")]
        public ActionResult<string> PutPlayer(int id)
        {
            return Ok($"put {id}");
        }

        [HttpDelete("id/{id}")]
        public ActionResult<string> DeletePlayer(int id)
        {
            return NoContent();
        }

    }
}
