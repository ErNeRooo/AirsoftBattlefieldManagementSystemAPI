namespace AirsoftBattlefieldManagementSystemAPI.Models.Entities
{
    public class Location
    {
        public int LocationId { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public short Accuracy { get; set; }
        public short Bearing { get; set; }
        public DateTime Time { get; set; }
    }
}
