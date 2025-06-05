namespace AirsoftBattlefieldManagementSystemAPI.Models.Entities
{
    public class Room
    {
        public int RoomId { get; set; }
        public int MaxPlayers { get; set; }
        public string JoinCode { get; set; }
        public string PasswordHash { get; set; }

        public int? AdminPlayerId { get; set; }
        public virtual Player AdminPlayer { get; set; }
        public virtual List<Battle> Battles { get; set; }
        public virtual List<Team> Teams { get; set; }
    }
}
