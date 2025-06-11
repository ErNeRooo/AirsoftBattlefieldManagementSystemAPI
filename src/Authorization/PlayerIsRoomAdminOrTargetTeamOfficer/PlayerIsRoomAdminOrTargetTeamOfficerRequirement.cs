using Microsoft.AspNetCore.Authorization;

namespace AirsoftBattlefieldManagementSystemAPI.Authorization.PlayerIsRoomAdminOrTargetTeamOfficer;

public class PlayerIsRoomAdminOrTargetTeamOfficerRequirement : IAuthorizationRequirement
{
    public int TeamId { get; set; }
    
    public PlayerIsRoomAdminOrTargetTeamOfficerRequirement(int teamId)
    {
        TeamId = teamId;
    }
}