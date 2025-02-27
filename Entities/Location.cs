namespace AirsoftBattlefieldManagementSystemAPI.Entities
{
    public class Location
    {
        public int LocationID { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public Int16 Accuracy { get; set; }
        public Int16 Bearing { get; set; }
}
}
