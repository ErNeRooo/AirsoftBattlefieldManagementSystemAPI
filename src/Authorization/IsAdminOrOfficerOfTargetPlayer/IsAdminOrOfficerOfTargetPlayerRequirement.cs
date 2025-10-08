using Microsoft.AspNetCore.Authorization;

namespace AirsoftBattlefieldManagementSystemAPI.Authorization.IsAdminOrOfficerOfTargetPlayer
{
    public class IsAdminOrOfficerOfTargetPlayerRequirement : IAuthorizationRequirement
    {
        public int OfficerPlayerId { get; }
        public int AdminPlayerId { get; }

        public IsAdminOrOfficerOfTargetPlayerRequirement(int officerPlayerId, int adminPlayerId)
        {
            OfficerPlayerId = officerPlayerId;
            AdminPlayerId = adminPlayerId;
        }
    }
}
