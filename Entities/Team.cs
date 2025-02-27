namespace AirsoftBattlefieldManagementSystemAPI.Entities
{
    public class Team
    {
        public int TeamID { get; set; }
        public string Name { get; set; }
        public int BattleID { get; set; }

        public virtual Battle Battle { get; set; }
    }
}
