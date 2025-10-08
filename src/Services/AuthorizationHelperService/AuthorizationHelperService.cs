using AirsoftBattlefieldManagementSystemAPI.Authorization;
using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Authorization.IsAdminOrOfficerOfTargetPlayer;
using AirsoftBattlefieldManagementSystemAPI.Authorization.IsNotSelf;
using AirsoftBattlefieldManagementSystemAPI.Authorization.JwtPlayerIdHasExistingPlayerEntity;
using AirsoftBattlefieldManagementSystemAPI.Authorization.PlayerIsInTheSameRoomAsResource;
using AirsoftBattlefieldManagementSystemAPI.Authorization.PlayerIsInTheSameTeamAsResource;
using AirsoftBattlefieldManagementSystemAPI.Authorization.PlayerIsRoomAdminOrTargetTeamOfficer;
using AirsoftBattlefieldManagementSystemAPI.Authorization.PlayerOwnsResource;
using AirsoftBattlefieldManagementSystemAPI.Authorization.TargetPlayerIsInTheSameTeam;

namespace AirsoftBattlefieldManagementSystemAPI.Services.AuthorizationHelperService
{
    public class AuthorizationHelperService(IAuthorizationService authorizationService) : IAuthorizationHelperService
    {
        public bool CheckPlayerIsInTheSameRoomAsResource(ClaimsPrincipal user, int? roomId, string? message = null)
        {
            var playerIsInTheSameRoomAsResourceResult =
            authorizationService.AuthorizeAsync(user, roomId ?? 0,
                new PlayerIsInTheSameRoomAsResourceRequirement()).Result;

            if (!playerIsInTheSameRoomAsResourceResult.Succeeded) throw new ForbidException(message ?? "You're must be in the same room as target resource");
            return true;
        }
        
        public bool CheckPlayerIsInTheSameTeamAsResource(ClaimsPrincipal user, int? teamId, string? message = null)
        {
            var playerIsInTheSameRoomAsResourceResult =
                authorizationService.AuthorizeAsync(user, teamId ?? 0,
                    new PlayerIsInTheSameTeamAsResourceRequirement()).Result;

            if (!playerIsInTheSameRoomAsResourceResult.Succeeded) throw new ForbidException(message ?? "You're must be in the same team as target resource");
            return true;
        }

        public bool CheckIfPlayerIsAdminOrOfficerOfTargetPlayer(ClaimsPrincipal user, int officerPlayerId, int adminPlayerId,
            string? message = null)
        {
            var playerIsInTheSameRoomAsResourceResult =
                authorizationService.AuthorizeAsync(user, null,
                    new IsAdminOrOfficerOfTargetPlayerRequirement(officerPlayerId, adminPlayerId)).Result;

            if (!playerIsInTheSameRoomAsResourceResult.Succeeded) throw new ForbidException(message ?? "You're must be admin or officer of target player");
            return true;
        }


        public bool CheckTargetPlayerIsInTheSameTeam(ClaimsPrincipal user, int playerId, int teamId, string? message = null)
        {
            var playerIsInTheSameRoomAsResourceResult =
                authorizationService.AuthorizeAsync(user, null,
                    new TargetPlayerIsInTheSameTeamRequirement(playerId, teamId)).Result;

            if (!playerIsInTheSameRoomAsResourceResult.Succeeded) throw new ForbidException(message ?? "Target player must be in the same team");
            return true;
        }

        public bool CheckPlayerOwnsResource(ClaimsPrincipal user, int? playerId, string? message = null)
        {
            var authorizationResult =
                authorizationService.AuthorizeAsync(user, playerId,
                    new PlayerOwnsResourceRequirement()).Result;

            if (!authorizationResult.Succeeded) throw new ForbidException(message ?? "You're unauthorize to manipulate this resource");
            return true;
        }
        
        public bool CheckPlayerIsRoomAdminOrTargetTeamOfficer(ClaimsPrincipal user, int teamId, string? message = null)
        {
            var authorizationResult =
                authorizationService.AuthorizeAsync(user, teamId, new PlayerIsRoomAdminOrTargetTeamOfficerRequirement(teamId)).Result;

            if (!authorizationResult.Succeeded) throw new ForbidException(message ?? "You're must be room admin or team officer to manipulate this resource");
            return true;
        }
        
        public bool CheckPlayerIdHasExistingEntity(ClaimsPrincipal user, string? message = null)
        {
            var authorizationResult =
                authorizationService.AuthorizeAsync(user, null, new JwtPlayerIdHasExistingPlayerEntityRequirement()).Result;

            if (!authorizationResult.Succeeded) throw new ForbidException(message ?? "You do not exist player");
            return true;
        }

        public bool CheckIfPlayerIsNotSelf(ClaimsPrincipal user, int targetPlayerId, string? message = null)
        {
            var authorizationResult =
                authorizationService.AuthorizeAsync(user, targetPlayerId, new IsNotSelfRequirement()).Result;

            if (!authorizationResult.Succeeded) throw new ForbidException(message ?? "You can't do it yourself");
            return true;
        }
    }
}
