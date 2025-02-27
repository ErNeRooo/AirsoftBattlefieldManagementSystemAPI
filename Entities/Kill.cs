namespace AirsoftBattlefieldManagementSystemAPI.Entities
{
    public class Kill
    {
        public int KillID { get; set; }
        public int KillerPlayerID { get; set; }
        public int KilledPlayerID { get; set; }
        public int LocationID { get; set; }
        public DateTime Datetime { get; set; }

        public virtual Player KillerPlayer { get; set; }
        public virtual Player KilledPlayer { get; set; }
        public virtual Location Location { get; set; }
    }
}
