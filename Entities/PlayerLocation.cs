namespace AirsoftBattlefieldManagementSystemAPI.Entities
{
    public class PlayerLocation
    {
        public int PlayerLocationID { get; set; }
        public int PlayerID { get; set; }
        public int LocationID { get; set; }
        public DateTime DateTime { get; set; }

        public virtual Player Player { get; set; }
        public virtual Location Location { get; set; }
    }
}
