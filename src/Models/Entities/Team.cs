namespace AirsoftBattlefieldManagementSystemAPI.Models.Entities
{
    public class Team
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
        public int RoomId { get; set; }

        public int OfficerPlayerId { get; set; }
        public virtual Player OfficerPlayer { get; set; }
        public virtual Room Room { get; set; }
        public virtual List<Player> Players { get; set; }
    }
}
