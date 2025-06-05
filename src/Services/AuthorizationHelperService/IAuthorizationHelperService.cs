using System.Security.Claims;

namespace AirsoftBattlefieldManagementSystemAPI.Services.AuthorizationHelperService
{
    public interface IAuthorizationHelperService
    {
        public void CheckPlayerIsInTheSameRoomAsResource(ClaimsPrincipal user, int? roomId);

        public void CheckPlayerOwnsResource(ClaimsPrincipal user, int? playerId);
    }
}
