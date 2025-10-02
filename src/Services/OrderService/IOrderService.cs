using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Order;

namespace AirsoftBattlefieldManagementSystemAPI.Services.OrderService
{
    public interface IOrderService
    {
        public OrderDto GetById(int id, ClaimsPrincipal user);
        public OrderDto Create(PostOrderDto postOrderDto, ClaimsPrincipal user);
        public void DeleteById(int id, ClaimsPrincipal user);
    }
}
