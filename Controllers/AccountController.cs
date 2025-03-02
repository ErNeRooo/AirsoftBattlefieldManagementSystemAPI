using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftBattlefieldManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController(ILogger<AccountController> logger, IAccountService accountService) : Controller
    {
        [HttpGet]
        [Route("id/{id}")]
        public ActionResult<AccountDto> GetById(int id)
        {
            AccountDto? accountDto = accountService.GetById(id);

            if (accountDto is null) return NotFound();

            return Ok(accountDto);
        }

        [HttpPost]
        public ActionResult Create([FromBody] CreateAccountDto accountDto)
        {
            if(!ModelState.IsValid) return BadRequest();

            int id = accountService.Create(accountDto);

            return Created($"/Account/id/{id}", null);
        }

        [HttpPut]
        [Route("id/{id}")]
        public ActionResult Update(int id, [FromBody] UpdateAccountDto accountDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            bool isSuccessful = accountService.Update(id, accountDto);

            if (isSuccessful) return Ok();

            return NotFound();
        }

        [HttpDelete]
        [Route("id/{id}")]
        public ActionResult Delete(int id)
        {
            bool isSuccessful = accountService.DeleteById(id);

            if (isSuccessful) return NoContent();

            return NotFound();
        }
    }
}
