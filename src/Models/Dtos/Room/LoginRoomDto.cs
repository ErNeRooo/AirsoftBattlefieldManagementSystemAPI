namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Room
{
    public class LoginRoomDto
    {
        public string JoinCode { get; set; }
        public string Password { get; set; } = string.Empty;
    }
}
