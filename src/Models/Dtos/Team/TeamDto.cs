namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Team
{
    public class TeamDto
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
        public int RoomId { get; set; }
        public int OfficerPlayerId { get; set; }
    }
}
