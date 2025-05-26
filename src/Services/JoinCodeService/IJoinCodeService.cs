using AirsoftBattlefieldManagementSystemAPI.Enums;

namespace AirsoftBattlefieldManagementSystemAPI.Services.JoinCodeService
{
    public interface IJoinCodeService
    {
        string Generate(JoinCodeFormat joinCodeFormat, int codeLength);
    }
}
