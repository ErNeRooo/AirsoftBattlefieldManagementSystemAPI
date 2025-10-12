using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.ZoneVertex
{
    public class ZoneVertexDto
    {
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }

        public ZoneVertexDto(decimal longitude, decimal latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }

        public ZoneVertexDto()
        {
            
        }
    }
}
