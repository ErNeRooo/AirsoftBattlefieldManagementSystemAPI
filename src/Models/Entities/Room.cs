namespace AirsoftBattlefieldManagementSystemAPI.Models.Entities
{
    public class Room
    {
        public int RoomId { get; set; }
        public int MaxPlayers { get; set; }
        public int JoinRoomNumber { get; set; }

        public virtual List<Battle> Battles { get; set; }
        public virtual List<Team> Teams { get; set; }
    }
}
