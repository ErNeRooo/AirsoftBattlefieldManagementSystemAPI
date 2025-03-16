namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get
{
    public class RoomDto
    {
        public int RoomId { get; set; }
        public int MaxPlayers { get; set; }
        public string JoinCode { get; set; }
        public int AdminPlayerId { get; set; }
    }
}
