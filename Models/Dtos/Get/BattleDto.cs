namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get
{
    public class BattleDto
    {
        public string Name { get; set; }
        public int BattleId { get; set; }
        public bool IsActive { get; set; }
        public int RoomId { get; set; }
    }
}
