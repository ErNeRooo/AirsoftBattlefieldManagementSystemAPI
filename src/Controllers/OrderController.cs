using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Order;
using AirsoftBattlefieldManagementSystemAPI.Services.OrderService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftBattlefieldManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class OrderController(IOrderService orderService) : Controller
    {
        [HttpGet]
        [Route("id/{id}")]
        public ActionResult<OrderDto> GetById(int id)
        {
            OrderDto orderDto = orderService.GetById(id, User);

            return Ok(orderDto);
        }

        [HttpPost]
        [Route("")]
        public ActionResult<OrderDto> Create([FromBody] PostOrderDto orderDto)
        {
            OrderDto resultOrder = orderService.Create(orderDto, User);

            return Created($"/Order/id/{resultOrder.OrderId}", resultOrder);
        }

        [HttpDelete]
        [Route("id/{id}")]
        public ActionResult Delete(int id)
        {
            orderService.DeleteById(id, User);

            return NoContent();
        }
    }
}
