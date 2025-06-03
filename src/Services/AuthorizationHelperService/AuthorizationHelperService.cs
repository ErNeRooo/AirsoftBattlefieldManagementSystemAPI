using AirsoftBattlefieldManagementSystemAPI.Authorization;
using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AirsoftBattlefieldManagementSystemAPI.Services.AuthorizationHelperService
{
    public class AuthorizationHelperService(IAuthorizationService authorizationService) : IAuthorizationHelperService
    {
        public void CheckPlayerIsInTheSameRoomAsResource(ClaimsPrincipal user, int? roomId)
        {
            var playerIsInTheSameRoomAsResourceResult =
            authorizationService.AuthorizeAsync(user, roomId,
                new PlayerIsInTheSameRoomAsResourceRequirement()).Result;

            if (!playerIsInTheSameRoomAsResourceResult.Succeeded) throw new ForbidException($"You're must be in the same room as target resource");
        }

        public void CheckPlayerOwnsResource(ClaimsPrincipal user, int playerId)
        {
            var authorizationResult =
                authorizationService.AuthorizeAsync(user, playerId,
                    new PlayerOwnsResourceRequirement()).Result;

            if (!authorizationResult.Succeeded) throw new ForbidException($"You're unauthorize to manipulate this resource");
        }
    }
}
