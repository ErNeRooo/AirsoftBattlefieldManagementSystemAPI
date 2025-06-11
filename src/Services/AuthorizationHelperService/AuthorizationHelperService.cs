using AirsoftBattlefieldManagementSystemAPI.Authorization;
using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Authorization.JwtPlayerIdHasExistingPlayerEntity;
using AirsoftBattlefieldManagementSystemAPI.Authorization.PlayerIsInTheSameRoomAsResource;
using AirsoftBattlefieldManagementSystemAPI.Authorization.PlayerIsRoomAdminOrTargetTeamOfficer;
using AirsoftBattlefieldManagementSystemAPI.Authorization.PlayerOwnsResource;
using AirsoftBattlefieldManagementSystemAPI.Authorization.TargetPlayerIsInTheSameTeam;

namespace AirsoftBattlefieldManagementSystemAPI.Services.AuthorizationHelperService
{
    public class AuthorizationHelperService(IAuthorizationService authorizationService) : IAuthorizationHelperService
    {
        public void CheckPlayerIsInTheSameRoomAsResource(ClaimsPrincipal user, int? roomId, string? message = null)
        {
            var playerIsInTheSameRoomAsResourceResult =
            authorizationService.AuthorizeAsync(user, roomId ?? 0,
                new PlayerIsInTheSameRoomAsResourceRequirement()).Result;

            if (!playerIsInTheSameRoomAsResourceResult.Succeeded) throw new ForbidException(message ?? "You're must be in the same room as target resource");
        }
        
        public void CheckTargetPlayerIsInTheSameTeam(ClaimsPrincipal user, int playerId, int teamId, string? message = null)
        {
            var playerIsInTheSameRoomAsResourceResult =
                authorizationService.AuthorizeAsync(user, null,
                    new TargetPlayerIsInTheSameTeamRequirement(playerId, teamId)).Result;

            if (!playerIsInTheSameRoomAsResourceResult.Succeeded) throw new ForbidException(message ?? "Target player must be in the same team");
        }

        public void CheckPlayerOwnsResource(ClaimsPrincipal user, int? playerId, string? message = null)
        {
            var authorizationResult =
                authorizationService.AuthorizeAsync(user, playerId,
                    new PlayerOwnsResourceRequirement()).Result;

            if (!authorizationResult.Succeeded) throw new ForbidException(message ?? "You're unauthorize to manipulate this resource");
        }
        
        public void CheckPlayerIsRoomAdminOrTargetTeamOfficer(ClaimsPrincipal user, int teamId, string? message = null)
        {
            var authorizationResult =
                authorizationService.AuthorizeAsync(user, teamId, new PlayerIsRoomAdminOrTargetTeamOfficerRequirement(teamId)).Result;

            if (!authorizationResult.Succeeded) throw new ForbidException(message ?? "You're must be room admin or team officer to manipulate this resource");
        }
        
        public void CheckPlayerIdHasExistingEntity(ClaimsPrincipal user, string? message = null)
        {
            var authorizationResult =
                authorizationService.AuthorizeAsync(user, null, new JwtPlayerIdHasExistingPlayerEntityRequirement()).Result;

            if (!authorizationResult.Succeeded) throw new ForbidException(message ?? "You do not exist player");
        }
    }
}
