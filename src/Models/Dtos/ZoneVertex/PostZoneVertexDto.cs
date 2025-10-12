using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.ZoneVertex
{
    public class PostZoneVertexDto
    {
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        
        
        public PostZoneVertexDto(decimal longitude, decimal latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }

        public PostZoneVertexDto()
        {
            
        }
    }
}
