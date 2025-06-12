using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Account;
using AirsoftBattlefieldManagementSystemAPI.Services.AccountService;
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
        public ActionResult<AccountDto> Create([FromBody] PostAccountDto accountDto)
        {
            AccountDto resultAccount = accountService.Create(accountDto, User);

            return Created($"/Account/id/{resultAccount.AccountId}", resultAccount);
        }

        [HttpPost]
        [Route("login")]
        public ActionResult<AccountDto> LogIn([FromBody] LoginAccountDto accountDto)
        {
            AccountDto resultAccount = accountService.LogIn(accountDto, User);

            return Created($"/Account/id/{resultAccount.AccountId}", resultAccount);
        }

        [HttpPut]
        [Route("")]
        public ActionResult<AccountDto> Update([FromBody] PutAccountDto accountDto)
        {
            AccountDto resultAccount = accountService.Update(accountDto, User);

            return Ok(resultAccount);
        }

        [HttpDelete]
        [Route("")]
        public ActionResult Delete()
        {
            accountService.Delete(User);

            return NoContent();
        }
    }
}
