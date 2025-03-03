namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get
{
    public class PlayerDto
    {
        public int PlayerId { get; set; }
        public int MaxPlayers { get; set; }
        public bool IsDead { get; set; }
    }
}
