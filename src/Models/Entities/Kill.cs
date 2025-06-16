namespace AirsoftBattlefieldManagementSystemAPI.Models.Entities
{
    public class Kill
    {
        public int KillId { get; set; }

        public int PlayerId { get; set; }
        public virtual Player Player { get; set; }
        public int LocationId { get; set; }
        public virtual Location Location { get; set; }
        public int BattleId { get; set; }
        public virtual Battle Battle { get; set; }
    }
}
