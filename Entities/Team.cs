namespace AirsoftBattlefieldManagementSystemAPI.Entities
{
    public class Team
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
        public int RoomId { get; set; }

        public virtual Room Room { get; set; }
        public virtual List<Player> Players { get; set; }
    }
}
