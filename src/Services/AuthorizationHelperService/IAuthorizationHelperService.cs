using System.Security.Claims;

namespace AirsoftBattlefieldManagementSystemAPI.Services.AuthorizationHelperService
{
    public interface IAuthorizationHelperService
    {
        public bool CheckPlayerIsInTheSameRoomAsResource(ClaimsPrincipal user, int? roomId, string? message = null);

        public bool CheckPlayerIsInTheSameTeamAsResource(ClaimsPrincipal user, int? teamId, string? message = null);
        
        public bool CheckIfPlayerIsAdminOrOfficerOfTargetPlayer(ClaimsPrincipal user, int officerPlayerId, int adminPlayerId, string? message = null);
        
        public bool CheckTargetPlayerIsInTheSameTeam(ClaimsPrincipal user, int playerId, int teamId,
            string? message = null);
        
        public bool CheckPlayerOwnsResource(ClaimsPrincipal user, int? playerId, string? message = null);

        public bool CheckPlayerIsRoomAdminOrTargetTeamOfficer(ClaimsPrincipal user, int teamId, string? message = null);

        public bool CheckPlayerIdHasExistingEntity(ClaimsPrincipal user, string? message = null);
        
        public bool CheckIfPlayerIsNotSelf(ClaimsPrincipal user, int targetPlayerId, string? message = null);
    }
}
