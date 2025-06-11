using Microsoft.AspNetCore.Authorization;

namespace AirsoftBattlefieldManagementSystemAPI.Authorization.TargetPlayerIsInTheSameTeam;

public class TargetPlayerIsInTheSameTeamRequirement : IAuthorizationRequirement
{
    public int PlayerId { get; init; }
    public int TeamId { get; init; }
    
    public TargetPlayerIsInTheSameTeamRequirement(int playerId, int teamId)
    {
        PlayerId = playerId;
        TeamId = teamId;
    }
}