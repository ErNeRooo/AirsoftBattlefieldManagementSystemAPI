﻿using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Player;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Services.AuthorizationHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.ClaimsHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService;

namespace AirsoftBattlefieldManagementSystemAPI.Services.PlayerService
{
    public class PlayerService(IBattleManagementSystemDbContext dbContext, IMapper mapper, IAuthenticationSettings authenticationSettings, IAuthorizationHelperService authorizationHelper, IDbContextHelperService dbHelper, IClaimsHelperService claimsHelper) : IPlayerService
    {
        public PlayerDto GetMe(ClaimsPrincipal user)
        {
            Player player = dbHelper.Player.FindSelf(user);
            
            PlayerDto playerDto = mapper.Map<PlayerDto>(player);

            return playerDto;
        }
        
        public PlayerDto GetById(int id, ClaimsPrincipal user)
        {
            Player player = dbHelper.Player.FindById(id);
            int playerId = claimsHelper.GetIntegerClaimValue("playerId", user);

            if (playerId != id)
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
        
        public PlayerDto Kick(int id, ClaimsPrincipal user)
        {
            Player player = dbHelper.Player.FindById(id);
            Room room = dbHelper.Room.FindById(player.RoomId);
            
            authorizationHelper.CheckPlayerOwnsResource(user, room.AdminPlayerId);

            player.RoomId = 0;
            dbContext.Player.Update(player);
            dbContext.SaveChanges();
            
            return mapper.Map<PlayerDto>(player);
        }

        public PlayerDto Update(PutPlayerDto playerDto, ClaimsPrincipal user)
        {
            int playerId = claimsHelper.GetIntegerClaimValue("playerId", user);
            authorizationHelper.CheckPlayerOwnsResource(user, playerId);
            
            Player previousPlayer = dbHelper.Player.FindById(playerId);
            
            if(playerDto.TeamId is not null)
            {
                Team targetTeam = dbHelper.Team.FindById(playerDto.TeamId);
                
                authorizationHelper.CheckPlayerIsInTheSameRoomAsResource(user, targetTeam.RoomId,
                    $"Target team {targetTeam.TeamId} is not in the same room as player");
                
                Team? oldTeam = previousPlayer.TeamId is not null ? dbHelper.Team.FindById(previousPlayer.TeamId) : null;

                if (oldTeam?.OfficerPlayerId == playerId)
                {
                    oldTeam.OfficerPlayerId = null;
                    dbContext.Team.Update(oldTeam);
                }
            }

            mapper.Map(playerDto, previousPlayer);
            dbContext.Player.Update(previousPlayer);
            dbContext.SaveChanges();
            
            return mapper.Map<PlayerDto>(previousPlayer);
        }

        public void Delete(ClaimsPrincipal user)
        {
            int playerId = claimsHelper.GetIntegerClaimValue("playerId", user);
            Player player = dbHelper.Player.FindById(playerId);

            authorizationHelper.CheckPlayerOwnsResource(user, playerId);
            
            dbContext.Player.Remove(player);
            dbContext.SaveChanges();
        }

        public string GenerateJwt(int playerId)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("playerId", $"{playerId}")
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
