namespace AirsoftBattlefieldManagementSystemAPI.Models.Entities
{
    public class Player
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public bool IsDead { get; set; }

        public int? TeamId { get; set; }
        public virtual Team Team { get; set; }
        public int? AccountId { get; set; }
        public virtual Account Account { get; set; }
        public int? RoomId { get; set; }
        public virtual Room Room { get; set; }

        public virtual List<PlayerLocation> PlayerLocations { get; set; }
        public virtual List<Kill> Kills { get; set; }
        public virtual List<Death> Deaths { get; set; }
    }
}
