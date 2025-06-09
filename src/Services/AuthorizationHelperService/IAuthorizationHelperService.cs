using System.Security.Claims;

namespace AirsoftBattlefieldManagementSystemAPI.Services.AuthorizationHelperService
{
    public interface IAuthorizationHelperService
    {
        public void CheckPlayerIsInTheSameRoomAsResource(ClaimsPrincipal user, int? roomId, string? message = null);

        public void CheckPlayerOwnsResource(ClaimsPrincipal user, int? playerId, string? message = null);

        public void CheckPlayerIdHasExistingEntity(ClaimsPrincipal user, string? message = null);
    }
}
