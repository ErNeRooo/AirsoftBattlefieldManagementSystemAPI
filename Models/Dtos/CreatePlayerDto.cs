namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos
{
    public class CreatePlayerDto
    {
        public string Name { get; set; }
        public bool IsDead { get; set; }
        public int TeamId { get; set; }
    }
}
