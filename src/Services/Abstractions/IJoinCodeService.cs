using AirsoftBattlefieldManagementSystemAPI.Enums;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Abstractions
{
    public interface IJoinCodeService
    {
        string Generate(JoinCodeFormat joinCodeFormat, int codeLength);
    }
}
