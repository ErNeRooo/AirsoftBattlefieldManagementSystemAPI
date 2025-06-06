using AirsoftBattlefieldManagementSystemAPI.Authorization;
using AirsoftBattlefieldManagementSystemAPI.Enums;
using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Location;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Services.AuthorizationHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService;

namespace AirsoftBattlefieldManagementSystemAPI.Services.LocationService
{
    public class LocationService(IMapper mapper, IBattleManagementSystemDbContext dbContext, IAuthorizationHelperService authorizationHelper, IDbContextHelperService dbHelper) : ILocationService
    {
        public LocationDto GetById(int id, ClaimsPrincipal user)
        {
            Location location = dbHelper.FindLocationById(id);
            PlayerLocation playerLocation = dbHelper.FindPlayerLocationById(id);

            authorizationHelper.CheckPlayerIsInTheSameRoomAsResource(user, playerLocation.RoomId);

            LocationDto locationDto = new LocationDto
            {
                PlayerId = playerLocation.PlayerId,
                LocationId = location.LocationId,
                RoomId = playerLocation.RoomId,
                Longitude = location.Longitude,
                Latitude = location.Latitude,
                Accuracy = location.Accuracy,
                Bearing = location.Bearing,
                Time = location.Time
            };

            return locationDto;
        }

        public List<LocationDto> GetAllLocationsOfPlayerWithId(int playerId, ClaimsPrincipal user)
        {
            Player player = dbHelper.FindPlayerById(playerId);

            authorizationHelper.CheckPlayerIsInTheSameRoomAsResource(user, playerId);

            List<Location> locations = dbHelper.FindAllLocationsOfPlayer(player);

            List<LocationDto> locationDtos = locations.Select(l =>
            {
                return new LocationDto
                {
                    PlayerId = playerId,
                    LocationId = l.LocationId,
                    RoomId = player.RoomId,
                    Longitude = l.Longitude,
                    Latitude = l.Latitude,
                    Accuracy = l.Accuracy,
                    Bearing = l.Bearing,
                    Time = l.Time
                };
            }).ToList();

            return locationDtos;
        }

        public LocationDto Create(int playerId, PostLocationDto locationDto, ClaimsPrincipal user)
        {
            authorizationHelper.CheckPlayerOwnsResource(user, playerId);

            Player player = dbHelper.FindPlayerById(playerId);

            Location location = mapper.Map<Location>(locationDto);
            dbContext.Location.Add(location);

            dbContext.SaveChanges();

            PlayerLocation playerLocation = new PlayerLocation();
            playerLocation.LocationId = location.LocationId;
            playerLocation.PlayerId = playerId;
            playerLocation.RoomId = player.RoomId;
            dbContext.PlayerLocation.Add(playerLocation);

            dbContext.SaveChanges();
            
            LocationDto locationToReturn = new LocationDto
            {
                LocationId = location.LocationId,
                PlayerId = playerId,
                RoomId = player.RoomId,
                Longitude = location.Longitude,
                Latitude = location.Latitude,
                Accuracy = location.Accuracy,
                Bearing = location.Bearing,
                Time = location.Time
            };

            return locationToReturn;
        }

        public LocationDto Update(int id, PutLocationDto locationDto, ClaimsPrincipal user)
        {
            Location oldLocation = dbHelper.FindLocationById(id);
            PlayerLocation playerLocation = dbHelper.FindPlayerLocationById(id);

            authorizationHelper.CheckPlayerOwnsResource(user, playerLocation.PlayerId);

            mapper.Map(locationDto, oldLocation);
            dbContext.Location.Update(oldLocation);
            dbContext.SaveChanges();
            
            LocationDto locationToReturn = new LocationDto
            {
                LocationId = oldLocation.LocationId,
                PlayerId = playerLocation.PlayerId,
                RoomId = playerLocation.RoomId,
                Longitude = oldLocation.Longitude,
                Latitude = oldLocation.Latitude,
                Accuracy = oldLocation.Accuracy,
                Bearing = oldLocation.Bearing,
                Time = oldLocation.Time
            };

            return locationToReturn;
        }

        public void DeleteById(int id, ClaimsPrincipal user)
        {
            Location location = dbHelper.FindLocationById(id);
            PlayerLocation playerLocation = dbHelper.FindPlayerLocationById(id);

            authorizationHelper.CheckPlayerOwnsResource(user, playerLocation.PlayerId);

            dbContext.Location.Remove(location);
            dbContext.SaveChanges();
        }
    }
}
