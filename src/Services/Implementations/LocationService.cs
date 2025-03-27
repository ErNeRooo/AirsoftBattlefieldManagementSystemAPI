using AirsoftBattlefieldManagementSystemAPI.Authorization;
using AirsoftBattlefieldManagementSystemAPI.Enums;
using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.Abstractions;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Implementations
{
    public class LocationService(IMapper mapper, IBattleManagementSystemDbContext dbContext, IAuthorizationService authorizationService) : ILocationService
    {
        public LocationDto? GetById(int id)
        {
            Location? location = dbContext.Location.FirstOrDefault(t => t.LocationId == id);
            PlayerLocation? playerLocation = dbContext.PlayerLocation.FirstOrDefault(t => t.LocationId == id);

            if (location is null || playerLocation is null) throw new NotFoundException($"Location with id {id} not found");

            LocationDto locationDto = new LocationDto
            {
                PlayerId = playerLocation.PlayerId,
                LocationId = location.LocationId,
                Longitude = location.Longitude,
                Latitude = location.Latitude,
                Accuracy = location.Accuracy,
                Bearing = location.Bearing,
                Time = location.Time
            };

            return locationDto;
        }

        public List<LocationDto>? GetAllOfPlayerWithId(int playerId)
        {
            var locationIDs = 
                dbContext.PlayerLocation
                .Where(pl => pl.PlayerId == playerId)
                .Select(pl => pl.LocationId);

            if (locationIDs is null) throw new NotFoundException($"Player with id {playerId} not found");

            var locations = dbContext.Location
                .Where(l => locationIDs.Contains(l.LocationId)).ToList();

            List<LocationDto> locationDtos = locations.Select(l =>
            {
                return new LocationDto
                {
                    PlayerId = playerId,
                    LocationId = l.LocationId,
                    Longitude = l.Longitude,
                    Latitude = l.Latitude,
                    Accuracy = l.Accuracy,
                    Bearing = l.Bearing,
                    Time = l.Time
                };
            }).ToList();

            return locationDtos;
        }

        public int Create(int playerId, PostLocationDto locationDto, ClaimsPrincipal user)
        {
            var authorizationResult =
                authorizationService.AuthorizeAsync(user, playerId,
                    new PlayerOwnsResourceRequirement()).Result;

            if (!authorizationResult.Succeeded) throw new ForbidException($"You're unauthorize to manipulate this resource");

            Player? player = dbContext.Player.FirstOrDefault(p => p.PlayerId == playerId);

            if(player is null) throw new NotFoundException($"Player with id {playerId} not found");

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

        public void Update(int id, PutLocationDto locationDto, ClaimsPrincipal user)
        {
            Location? oldLocation = dbContext.Location.FirstOrDefault(t => t.LocationId == id);
            PlayerLocation? playerLocation = dbContext.PlayerLocation.FirstOrDefault(playerLocation => playerLocation.LocationId == id);

            if (oldLocation is null) throw new NotFoundException($"Location with id {id} not found");
            if (playerLocation is null) throw new NotFoundException($"Location to player reference not found");

            var authorizationResult =
                authorizationService.AuthorizeAsync(user, playerLocation.PlayerId,
                    new PlayerOwnsResourceRequirement()).Result;

            if (!authorizationResult.Succeeded) throw new ForbidException($"You're unauthorize to manipulate this resource");

            mapper.Map(locationDto, oldLocation);
            dbContext.Location.Update(oldLocation);
            dbContext.SaveChanges();
        }

        public void DeleteById(int id, ClaimsPrincipal user)
        {
            Location? location = dbContext.Location.FirstOrDefault(t => t.LocationId == id);
            PlayerLocation? playerLocation = dbContext.PlayerLocation.FirstOrDefault(playerLocation => playerLocation.LocationId == id);

            if (location is null) throw new NotFoundException($"Location with id {id} not found");
            if (playerLocation is null) throw new NotFoundException($"Location to player reference not found");

            var authorizationResult =
                authorizationService.AuthorizeAsync(user, playerLocation.PlayerId,
                    new PlayerOwnsResourceRequirement()).Result;

            if (!authorizationResult.Succeeded) throw new ForbidException($"You're unauthorize to manipulate this resource");

            dbContext.Location.Remove(location);
            dbContext.SaveChanges();
        }
    }
}
