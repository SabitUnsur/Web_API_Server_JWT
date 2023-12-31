﻿using AuthServer.Core.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using SharedLibrary.Dtos;

namespace AuthServer.Core.Services
{
    //Identity hazır verdiği için repository'e gerek yok direkt service yazdık.
    public interface IUserService
    {
        Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto);
        Task<Response<UserAppDto>> GetUserByNameAsync(string userName);
        Task<Response<NoContent>> CreateUserRoles(string userName);
    }
}
