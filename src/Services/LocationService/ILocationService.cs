﻿using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Location;
using System.Security.Claims;

namespace AirsoftBattlefieldManagementSystemAPI.Services.LocationService
{
    public interface ILocationService
    {
        public LocationDto GetById(int id, ClaimsPrincipal user);
        public List<LocationDto> GetAllLocationsOfPlayerWithId(int targetPlayerId, ClaimsPrincipal user);
        public LocationDto Create(PostLocationDto postLocationDto, ClaimsPrincipal user);
        public LocationDto Update(int id, PutLocationDto locationDto, ClaimsPrincipal user);
        public void DeleteById(int id, ClaimsPrincipal user);
    }
}
