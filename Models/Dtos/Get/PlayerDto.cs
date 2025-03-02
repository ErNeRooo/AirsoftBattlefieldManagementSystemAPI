namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get
{
    public class PlayerDto
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public bool IsDead { get; set; }
    }
}
