namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get
{
    public class BattleDto
    {
        public int BattleId { get; set; }
        public bool IsActive { get; set; }
        public int RoomId { get; set; }
    }
}
