namespace AirsoftBattlefieldManagementSystemAPI.Entities
{
    public class PlayerLocation
    {
        public int PlayerLocationId { get; set; }
        
        public int PlayerId { get; set; }
        public virtual Player Player { get; set; }
        public int LocationId { get; set; }
        public virtual Location Location { get; set; }
    }
}
