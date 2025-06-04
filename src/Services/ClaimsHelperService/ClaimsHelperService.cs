using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Exceptions;

namespace AirsoftBattlefieldManagementSystemAPI.Services.ClaimsHelperService;

public class ClaimsHelperService : IClaimsHelperService
{
    public int GetIntegerClaimValue(string claimName, ClaimsPrincipal user)
    {
        string? claimString = user.Claims.FirstOrDefault(c => c.Type == claimName)?.Value;
            
        bool isParsingSuccessfull = int.TryParse(claimString, out int playerId);
            
        if (!isParsingSuccessfull) throw new Exception("Invalid claim playerId");
            
        return playerId;
    }
}