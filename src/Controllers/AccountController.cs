using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Login;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftBattlefieldManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AccountController(IAccountService accountService) : Controller
    {
        [HttpGet]
        [Route("id/{id}")]
        public ActionResult<AccountDto> GetById(int id)
        {
            AccountDto accountDto = accountService.GetById(id);

            return Ok(accountDto);
        }

        [HttpPost]
        [Route("signup")]
        public ActionResult Create([FromBody] PostAccountDto accountDto)
        {
            int id = accountService.Create(accountDto, User);

            return Created($"/Account/id/{id}", null);
        }

        [HttpPost]
        [Route("login")]
        public ActionResult LogIn([FromBody] LoginAccountDto accountDto)
        {
            int id = accountService.LogIn(accountDto, User);

            return Created($"/Account/id/{id}", null);
        }

        [HttpPut]
        [Route("id/{id}")]
        public ActionResult Update(int id, [FromBody] PutAccountDto accountDto)
        {
            accountService.Update(id, accountDto);

            return Ok();

        }

        [HttpDelete]
        [Route("id/{id}")]
        public ActionResult Delete(int id)
        {
            accountService.DeleteById(id);

            return NoContent();
        }
    }
}
