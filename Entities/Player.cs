namespace AirsoftBattlefieldManagementSystemAPI.Entities
{
    public class Player
    {
        public int PlayerID { get; set; }
        public string Name { get; set; }
        public bool IsDead { get; set; }
        public int TeamID { get; set; }
        public int AccountID { get; set; }

        public virtual Team Team { get; set; }
        public virtual Account Account { get; set; }
    }
}
