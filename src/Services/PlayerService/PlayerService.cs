﻿using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using System.Security.Claims;
using System.Text;
using AirsoftBattlefieldManagementSystemAPI.Authorization;
using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Player;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Services.AuthorizationHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService;

namespace AirsoftBattlefieldManagementSystemAPI.Services.PlayerService
{
    public class PlayerService(IBattleManagementSystemDbContext dbContext, IMapper mapper, IAuthenticationSettings authenticationSettings, IAuthorizationHelperService authorizationHelper, IDbContextHelperService dbHelper) : IPlayerService
    {
        public PlayerDto GetById(int id, ClaimsPrincipal user)
        {
            Player player = dbHelper.FindPlayerById(id);

            authorizationHelper.CheckPlayerIsInTheSameRoomAsResource(user, player.RoomId);
            
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

        public PlayerDto Update(int id, PutPlayerDto playerDto, ClaimsPrincipal user)
        {
            authorizationHelper.CheckPlayerOwnsResource(user, id);

            Player previousPlayer = dbHelper.FindPlayerById(id);

            mapper.Map(playerDto, previousPlayer);
            dbContext.Player.Update(previousPlayer);
            dbContext.SaveChanges();
            
            return mapper.Map<PlayerDto>(previousPlayer);
        }

        public void DeleteById(int id, ClaimsPrincipal user)
        {
            authorizationHelper.CheckPlayerOwnsResource(user, id);

            Player player = dbHelper.FindPlayerById(id);

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
