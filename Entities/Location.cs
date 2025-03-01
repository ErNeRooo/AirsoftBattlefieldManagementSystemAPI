namespace AirsoftBattlefieldManagementSystemAPI.Entities
{
    public class Location
    {
        public int LocationId { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public Int16 Accuracy { get; set; }
        public Int16 Bearing { get; set; }
        public DateTime Time { get; set; }
}
}
