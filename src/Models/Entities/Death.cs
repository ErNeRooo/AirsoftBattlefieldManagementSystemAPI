namespace AirsoftBattlefieldManagementSystemAPI.Models.Entities
{
    public class Death
    {
        public int DeathId { get; set; }

        public int? PlayerId { get; set; }
        public virtual Player Player { get; set; }
        public int? LocationId { get; set; }
        public virtual Location Location { get; set; }
        public int? RoomId { get; set; }
        public virtual Room Room { get; set; }
    }
}
