namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get
{
    public class PlayerDto
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public bool IsDead { get; set; }
        public int TeamId { get; set; }
        public int RoomId { get; set; }
        public int AccountId { get; set; }
    }
}
