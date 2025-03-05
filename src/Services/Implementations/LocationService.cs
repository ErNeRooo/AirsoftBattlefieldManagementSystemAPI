using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.Abstractions;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Implementations
{
    public class LocationService(IMapper mapper, IBattleManagementSystemDbContext dbContext) : ILocationService
    {
        public LocationDto? GetById(int id)
        {
            Location? location = dbContext.Location.FirstOrDefault(t => t.LocationId == id);
            PlayerLocation? playerLocation = dbContext.PlayerLocation.FirstOrDefault(t => t.LocationId == id);

            if (location is null || playerLocation is null) return null;

            LocationDto locationDto = mapper.Map<LocationDto>(location, opt =>
            {
                opt.Items["playerId"] = playerLocation.PlayerId;
            });

            return locationDto;
        }

        public List<LocationDto>? GetAllOfPlayerWithId(int playerId)
        {
            var locationIDs = 
                dbContext.PlayerLocation
                .Where(pl => pl.PlayerId == playerId)
                .Select(pl => pl.LocationId);

            if (locationIDs.IsNullOrEmpty()) return null;

            var locations = dbContext.Location
                .Where(l => locationIDs.Contains(l.LocationId)).ToList();

            List<LocationDto> locationDtos = locations.Select(location =>
            {
                return mapper.Map<LocationDto>(location, opt =>
                {
                    opt.Items["playerId"] = playerId;
                });
            }).ToList();

            return locationDtos;
        }

        public int? Create(int playerId, CreateLocationDto locationDto)
        {
            Player? player = dbContext.Player.FirstOrDefault(p => p.PlayerId == playerId);

            if(player is null) return null;

            Location location = mapper.Map<Location>(locationDto);
            dbContext.Location.Add(location);

            dbContext.SaveChanges();

            PlayerLocation playerLocation = new PlayerLocation();
            playerLocation.LocationId = location.LocationId;
            playerLocation.PlayerId = playerId;
            dbContext.PlayerLocation.Add(playerLocation);

            dbContext.SaveChanges();

            return location.LocationId;
        }

        public bool Update(int id, UpdateLocationDto locationDto)
        {
            Location? previousLocation = dbContext.Location.FirstOrDefault(t => t.LocationId == id);

            if (previousLocation is null) return false;

            mapper.Map(locationDto, previousLocation);
            dbContext.Location.Update(previousLocation);
            dbContext.SaveChanges();

            return true;
        }

        public bool DeleteById(int id)
        {
            Location? location = dbContext.Location.FirstOrDefault(t => t.LocationId == id);

            if(location is null) return false;

            dbContext.Location.Remove(location);
            dbContext.SaveChanges();

            return true;
        }
    }
}
