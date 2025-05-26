using System.IdentityModel.Tokens.Jwt;
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

namespace AirsoftBattlefieldManagementSystemAPI.Services.PlayerService
{
    public class PlayerService(IBattleManagementSystemDbContext dbContext, IMapper mapper, IAuthenticationSettings authenticationSettings, IPasswordHasher<Room> passwordHasher, IAuthorizationService authorizationService) : IPlayerService
    {
        public PlayerDto GetById(int id, ClaimsPrincipal user)
        {
            Player? player = dbContext.Player.FirstOrDefault(p => p.PlayerId == id);

            if (player is null) throw new NotFoundException($"Player with id {id} not found");

            var playerIsInTheSameRoomAsResourceResult =
                authorizationService.AuthorizeAsync(user, player.RoomId,
                    new PlayerIsInTheSameRoomAsResourceRequirement()).Result;

            if (!playerIsInTheSameRoomAsResourceResult.Succeeded) throw new ForbidException($"You're unauthorize to manipulate this resource");
            
            PlayerDto playerDto = mapper.Map<PlayerDto>(player);

            return playerDto;
        }

        public int Create(PostPlayerDto playerDto)
        {
            var player = mapper.Map<Player>(playerDto);

            dbContext.Player.Add(player);
            dbContext.SaveChanges();

            return player.PlayerId;
        }

        public void Update(int id, PutPlayerDto playerDto, ClaimsPrincipal user)
        {
            var authorizationResult =
                authorizationService.AuthorizeAsync(user, id,
                    new PlayerOwnsResourceRequirement()).Result;

            if (!authorizationResult.Succeeded) throw new ForbidException($"You're unauthorize to manipulate player with id {id}");

            Player? previousPlayer = dbContext.Player.FirstOrDefault(p => p.PlayerId == id);

            if (previousPlayer is null) throw new NotFoundException($"Player with id {id} not found");

            mapper.Map(playerDto, previousPlayer);
            dbContext.Player.Update(previousPlayer);
            dbContext.SaveChanges();
        }

        public void DeleteById(int id, ClaimsPrincipal user)
        {
            var authorizationResult =
                authorizationService.AuthorizeAsync(user, id,
                    new PlayerOwnsResourceRequirement()).Result;

            if (!authorizationResult.Succeeded) throw new ForbidException($"You're unauthorize to manipulate player with id {id}");

            var player = dbContext.Player.FirstOrDefault(p => p.PlayerId == id);

            if (player is null) throw new NotFoundException($"Player with id {id} not found");

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
