using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Player;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Helpers;
using AirsoftBattlefieldManagementSystemAPI.Realtime;
using AirsoftBattlefieldManagementSystemAPI.Services.AuthorizationHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.ClaimsHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService;
using Microsoft.AspNetCore.SignalR;

namespace AirsoftBattlefieldManagementSystemAPI.Services.PlayerService
{
    public class PlayerService(
        IBattleManagementSystemDbContext dbContext, 
        IMapper mapper, 
        IAuthenticationSettings authenticationSettings, 
        IAuthorizationHelperService authorizationHelper, 
        IDbContextHelperService dbHelper, 
        IClaimsHelperService claimsHelper,
        IHubContext<RoomNotificationHub, IRoomNotificationClient> hubContext
        ) : IPlayerService
    {
        public PlayerDto GetMe(ClaimsPrincipal user)
        {
            Player player = dbHelper.Player.FindSelf(user);
            
            PlayerDto playerDto = mapper.Map<PlayerDto>(player);

            return playerDto;
        }
        
        public PlayerDto GetById(int playerId, ClaimsPrincipal user)
        {
            Player player = dbHelper.Player.FindById(playerId);
            int claimPlayerId = claimsHelper.GetIntegerClaimValue("playerId", user);

            if (claimPlayerId != playerId)
            {
                authorizationHelper.CheckPlayerIsInTheSameRoomAsResource(user, player.RoomId);
            }
            
            PlayerDto playerDto = mapper.Map<PlayerDto>(player);

            return playerDto;
        }

        public PlayerDto Create(PostPlayerDto playerDto)
        {
            var player = mapper.Map<Player>(playerDto);

            dbContext.Player.Add(player);
            dbContext.SaveChanges();

            return mapper.Map<PlayerDto>(player);
        }
        
        public PlayerDto KickFromRoom(int playerId, ClaimsPrincipal user)
        {
            Player targetPlayer = dbHelper.Player.FindByIdIncludingAllRelatedEntities(playerId);
            Room room = dbHelper.Room.FindByIdIncludingPlayers(targetPlayer.RoomId);
            
            authorizationHelper.CheckIfPlayerIsNotSelf(user, targetPlayer.PlayerId);
            authorizationHelper.CheckPlayerOwnsResource(user, targetPlayer.Room.AdminPlayerId);

            if(targetPlayer.PlayerId == targetPlayer.Team?.OfficerPlayerId)
            {
                UnassignTeamOfficer(targetPlayer.Team);
            }
            
            UnassignPlayerTeam(targetPlayer);
            UnassignPlayerRoom(targetPlayer);
            
            ClearPlayerMapPings(playerId);
            ClearPlayerOrders(playerId);
            
            dbContext.Player.Update(targetPlayer);
            dbContext.SaveChanges();
            
            PlayerDto responsePlayerDto = mapper.Map<PlayerDto>(targetPlayer);            
            
            IEnumerable<string> playerIds = room.GetAllPlayerIdsWithoutSelf(targetPlayer.PlayerId);

            hubContext.Clients.Users(playerIds).PlayerLeftRoom(playerId);
            
            return responsePlayerDto;
        }
        
        public PlayerDto KickFromTeam(int playerId, ClaimsPrincipal user)
        {
            Player targetPlayer = dbHelper.Player.FindByIdIncludingAllRelatedEntities(playerId);
            Room room = dbHelper.Room.FindByIdIncludingPlayers(targetPlayer.RoomId);
            
            authorizationHelper.CheckIfPlayerIsNotSelf(user, targetPlayer.PlayerId);
            authorizationHelper.CheckPlayerIsInTheSameRoomAsResource(user, targetPlayer.RoomId); 
            authorizationHelper.CheckIfPlayerIsAdminOrOfficerOfTargetPlayer(user, targetPlayer.Team.OfficerPlayerId ?? 0, room.AdminPlayerId ?? 0);
            
            if(targetPlayer.PlayerId == targetPlayer.Team.OfficerPlayerId)
            {
                UnassignTeamOfficer(targetPlayer.Team);
            }
            
            UnassignPlayerTeam(targetPlayer);
            ClearPlayerMapPings(playerId);
            ClearPlayerOrders(playerId);
            
            dbContext.SaveChanges();
            
            PlayerDto responsePlayerDto = mapper.Map<PlayerDto>(targetPlayer);            
            
            IEnumerable<string> playerIds = room.GetAllPlayerIdsWithoutSelf(targetPlayer.PlayerId);

            hubContext.Clients.Users(playerIds).PlayerLeftTeam(playerId);
            
            return responsePlayerDto;
        }

        private void ClearPlayerMapPings(int playerId)
        {
            IQueryable<MapPing> mapPingsToRemove = dbContext.MapPing.Where(ping => ping.PlayerId == playerId);
            dbContext.MapPing.RemoveRange(mapPingsToRemove);
        }
        
        private void ClearPlayerOrders(int playerId)
        {
            IQueryable<Order> ordersToRemove = dbContext.Order.Where(order => order.PlayerId == playerId);
            dbContext.Order.RemoveRange(ordersToRemove);
        }
        
        private void UnassignPlayerTeam(Player player)
        {
            player.TeamId = null;
            dbContext.Player.Update(player);
        }
        
        private void UnassignPlayerRoom(Player player)
        {
            player.RoomId = null;
            dbContext.Player.Update(player);
        }
        
        private void UnassignTeamOfficer(Team team)
        {
            team.OfficerPlayerId = null;
            dbContext.Team.Update(team);
        }

        public PlayerDto Update(PutPlayerDto playerDto, int playerId, ClaimsPrincipal user)
        {
            int selfPlayerId = claimsHelper.GetIntegerClaimValue("playerId", user);
            
            Player targetPlayer = dbHelper.Player.FindByIdIncludingRoom(playerId);
            Room room = dbHelper.Room.FindByIdIncludingPlayers(targetPlayer.RoomId);
            if(selfPlayerId != playerId) authorizationHelper.CheckPlayerOwnsResource(user, room.AdminPlayerId);

            if (playerDto.TeamId == 0)
            {
                
            }
            else if(playerDto.TeamId is not null)
            {
                Team targetTeam = dbHelper.Team.FindById(playerDto.TeamId);
                
                authorizationHelper.CheckPlayerIsInTheSameRoomAsResource(user, targetTeam.RoomId,
                    $"Target team {targetTeam.TeamId} is not in the same room as player");

                Team? previousTeam = GetPreviousTeam(targetPlayer);

                if (previousTeam?.OfficerPlayerId == playerId)
                {
                    UnassignTeamOfficer(previousTeam);
                    dbContext.SaveChanges();
                }
                
                ClearPlayerOrders(targetPlayer.PlayerId);
                ClearPlayerMapPings(targetPlayer.PlayerId);
            }

            if(selfPlayerId == targetPlayer.PlayerId) mapper.Map(playerDto, targetPlayer);
            else targetPlayer.TeamId = playerDto.TeamId;
            
            dbContext.Player.Update(targetPlayer);
            dbContext.SaveChanges();
            
            PlayerDto responsePlayerDto = mapper.Map<PlayerDto>(targetPlayer);            
            
            IEnumerable<string> playerIds = room.GetAllPlayerIdsWithoutSelf(targetPlayer.PlayerId);

            hubContext.Clients.Users(playerIds).PlayerUpdated(responsePlayerDto);
            
            return responsePlayerDto;
        }

        private Team? GetPreviousTeam(Player player) => dbContext.Team.FirstOrDefault(team => team.TeamId == player.TeamId);

        public void Delete(ClaimsPrincipal user)
        {
            int playerId = claimsHelper.GetIntegerClaimValue("playerId", user);
            Player player = dbHelper.Player.FindById(playerId);
            Room room = dbHelper.Room.FindByIdIncludingPlayers(player.RoomId);

            authorizationHelper.CheckPlayerOwnsResource(user, playerId);
            
            dbContext.Player.Remove(player);
            dbContext.SaveChanges();
            
            IEnumerable<string> playerIds = room.GetAllPlayerIdsWithoutSelf(player.PlayerId);

            hubContext.Clients.Users(playerIds).PlayerDeleted(player.PlayerId);
        }

        public string GenerateJwt(int playerId)
        {
            List<Claim> claims = new List<Claim>
            {
                new("playerId", $"{playerId}")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddDays(authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(
                authenticationSettings.JwtIssuer,
                audience: authenticationSettings.JwtIssuer,
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }
    }
}
