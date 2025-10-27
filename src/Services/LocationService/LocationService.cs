using AutoMapper;
using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Location;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Helpers;
using AirsoftBattlefieldManagementSystemAPI.Realtime;
using AirsoftBattlefieldManagementSystemAPI.Services.AuthorizationHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.ClaimsHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService;
using Microsoft.AspNetCore.SignalR;

namespace AirsoftBattlefieldManagementSystemAPI.Services.LocationService
{
    public class LocationService(
        IMapper mapper, 
        IBattleManagementSystemDbContext dbContext, 
        IClaimsHelperService claimsHelper, 
        IAuthorizationHelperService authorizationHelper, 
        IDbContextHelperService dbHelper,
        IHubContext<RoomNotificationHub, IRoomNotificationClient> hubContext
        ) : ILocationService
    {
        public LocationDto GetById(int id, ClaimsPrincipal user)
        {
            Location location = dbHelper.Location.FindById(id);
            PlayerLocation playerLocation = dbHelper.PlayerLocation.FindById(id);
            
            int playerId = claimsHelper.GetIntegerClaimValue("playerId", user);
            Player player = dbHelper.Player.FindById(playerId);

            authorizationHelper.CheckTargetPlayerIsInTheSameTeam(user, playerLocation.PlayerId, player.TeamId ?? 0);

            LocationDto locationDto = new LocationDto
            {
                PlayerId = playerLocation.PlayerId,
                LocationId = location.LocationId,
                BattleId = playerLocation.BattleId,
                Longitude = location.Longitude,
                Latitude = location.Latitude,
                Accuracy = location.Accuracy,
                Bearing = location.Bearing,
                Time = location.Time
            };

            return locationDto;
        }

        public List<LocationDto> GetAllLocationsOfPlayerWithId(int targetPlayerId, ClaimsPrincipal user)
        {
            int playerId = claimsHelper.GetIntegerClaimValue("playerId", user);
            Player player = dbHelper.Player.FindById(playerId);
            Player targetPlayer = dbHelper.Player.FindById(targetPlayerId);

            authorizationHelper.CheckTargetPlayerIsInTheSameTeam(user, targetPlayerId, player.TeamId ?? 0);
            
            Room room = dbHelper.Room.FindByIdIncludingBattle(targetPlayer.RoomId);
            
            List<Location> locations = dbHelper.Location.FindAllOfPlayer(targetPlayer);

            List<LocationDto> locationDtos = locations.Select(l =>
            {
                return new LocationDto
                {
                    PlayerId = targetPlayerId,
                    LocationId = l.LocationId,
                    BattleId = room.Battle?.BattleId ?? 0,
                    Longitude = l.Longitude,
                    Latitude = l.Latitude,
                    Accuracy = l.Accuracy,
                    Bearing = l.Bearing,
                    Time = l.Time
                };
            }).ToList();

            return locationDtos;
        }

        public LocationDto Create(PostLocationDto locationDto, ClaimsPrincipal user)
        {
            int playerId = claimsHelper.GetIntegerClaimValue("playerId", user);
            Player player = dbHelper.Player.FindById(playerId);
            Room room = dbHelper.Room.FindByIdIncludingRelated(player.RoomId);

            Location location = mapper.Map<Location>(locationDto);
            dbContext.Location.Add(location);

            dbContext.SaveChanges();

            PlayerLocation playerLocation = new PlayerLocation();
            playerLocation.LocationId = location.LocationId;
            playerLocation.PlayerId = playerId;
            playerLocation.BattleId = room.Battle?.BattleId ?? 0;
            dbContext.PlayerLocation.Add(playerLocation);

            dbContext.SaveChanges();
            
            LocationDto locationToReturn = new LocationDto
            {
                LocationId = location.LocationId,
                PlayerId = playerId,
                BattleId = room.Battle?.BattleId ?? 0,
                Longitude = location.Longitude,
                Latitude = location.Latitude,
                Accuracy = location.Accuracy,
                Bearing = location.Bearing,
                Time = location.Time
            };

            IEnumerable<string> playerIds = player.IsDead
                ? room.GetAllPlayerIdsWithoutSelf(player.PlayerId)
                : room.GetTeamPlayerIdsWithoutSelf(player.TeamId, player.PlayerId);

            hubContext.Clients.Users(playerIds).LocationCreated(locationToReturn);
            
            return locationToReturn;
        }

        public LocationDto Update(int id, PutLocationDto locationDto, ClaimsPrincipal user)
        {
            Location oldLocation = dbHelper.Location.FindById(id);
            PlayerLocation playerLocation = dbHelper.PlayerLocation.FindById(id);
            Player player = dbHelper.Player.FindById(playerLocation.PlayerId);
            Room room = dbHelper.Room.FindByIdIncludingPlayers(player.RoomId);

            authorizationHelper.CheckPlayerOwnsResource(user, playerLocation.PlayerId);

            mapper.Map(locationDto, oldLocation);
            dbContext.Location.Update(oldLocation);
            dbContext.SaveChanges();
            
            LocationDto locationToReturn = new()
            {
                LocationId = oldLocation.LocationId,
                PlayerId = playerLocation.PlayerId,
                BattleId = playerLocation.BattleId,
                Longitude = oldLocation.Longitude,
                Latitude = oldLocation.Latitude,
                Accuracy = oldLocation.Accuracy,
                Bearing = oldLocation.Bearing,
                Time = oldLocation.Time
            };

            IEnumerable<string> playerIds = player.IsDead
                ? room.GetAllPlayerIdsWithoutSelf(player.PlayerId)
                : room.GetTeamPlayerIdsWithoutSelf(player.TeamId, player.PlayerId);

            hubContext.Clients.Users(playerIds).LocationUpdated(locationToReturn);
            
            return locationToReturn;
        }

        public void DeleteById(int id, ClaimsPrincipal user)
        {
            Location location = dbHelper.Location.FindById(id);
            PlayerLocation playerLocation = dbHelper.PlayerLocation.FindById(id);
            Player player = dbHelper.Player.FindById(playerLocation.PlayerId);
            Room room = dbHelper.Room.FindByIdIncludingPlayers(player.RoomId);
            
            authorizationHelper.CheckPlayerOwnsResource(user, playerLocation.PlayerId);

            dbContext.Location.Remove(location);
            dbContext.SaveChanges();
            
            IEnumerable<string> playerIds = player.IsDead
                ? room.GetAllPlayerIdsWithoutSelf(player.PlayerId)
                : room.GetTeamPlayerIdsWithoutSelf(player.TeamId, player.PlayerId);
            
            hubContext.Clients.Users(playerIds).LocationDeleted(id);
        }
    }
}
