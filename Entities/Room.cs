namespace AirsoftBattlefieldManagementSystemAPI.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual List<Battle> Battles { get; set; }
        public virtual List<Team> Teams { get; set; }
    }
}
