namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Player
{
    public class PutPlayerDto
    {
        public string? Name { get; set; }
        public bool? IsDead { get; set; }
        public int? TeamId { get; set; }
    }
}
