using System.Security.Claims;

namespace AirsoftBattlefieldManagementSystemAPI.Services.ClaimsHelperService;

public interface IClaimsHelperService
{
    public int GetIntegerClaimValue(string claimName, ClaimsPrincipal user);
}