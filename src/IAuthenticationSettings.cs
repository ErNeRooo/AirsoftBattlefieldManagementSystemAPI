namespace AirsoftBattlefieldManagementSystemAPI
{
    public interface IAuthenticationSettings
    {
        string JwtKey { get; }
        int JwtExpireDays { get; }
        string JwtIssuer { get; }
    }
}
