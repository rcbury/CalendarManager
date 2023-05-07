using CalendarBackend.Db;
using CalendarBackend.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CalendarBackend.Services;

public class UserService
{
    private readonly UserManager<CalendarUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly CalendarDevContext _calendarDevContext;
    private readonly ImageStorageService _imageStorageService;

    public UserService(
        UserManager<CalendarUser> userManager,
        IConfiguration configuration,
        CalendarDevContext calendarDevContext,
        ImageStorageService imageStorageService)
    {
        _userManager = userManager;
        _configuration = configuration;
        _calendarDevContext = calendarDevContext;
        _imageStorageService = imageStorageService;
    }

    // TODO: move to repository
    async public Task<UserRole>? GetUserRoleByRoom(int userId, int roomId)
    {
        var userRoom = _calendarDevContext.RoomUsers
            .Where(x => x.User.Id == userId)
            .Where(x => x.RoomId == roomId)
            .FirstOrDefault();

        if (userRoom == null)
            return null;

        var userRole = GetUserRole(userRoom.UserRoleId);

        return userRole;
    }

    // TODO: move to repository
    public UserRole? GetUserRole(int userRoleId)
    {
        var userRole = _calendarDevContext.UserRoles
            .Where(x => x.Id == userRoleId)
            .FirstOrDefault();

        return userRole;
    }

    public async Task<bool> UpdateAvatar(int userId, IFormFile avatar)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        var avatarPath = await _imageStorageService.StoreProfileImageForUserOnDisk(user.Id, avatar);

        user.AvatarPath = avatarPath;

        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded;
    }

    public async Task<UserUpdateResponse> UpdateUser(UserDto userDto, int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        //if (user == null)
        //throw new Exception("User not found");

        user.FirstName = userDto.FirstName;
        user.LastName = userDto.LastName;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return new UserUpdateResponse
            {
                Result = false,
                Errors = result.Errors
            };
        }

        var updatedUserDto = new UserDto();
        updatedUserDto.LastName = user.LastName;
        updatedUserDto.FirstName = user.FirstName;
        updatedUserDto.UserName = user.UserName;
        updatedUserDto.Email = user.Email;

        return new UserUpdateResponse
        {
            UserDto = updatedUserDto,
            Result = true,
            Errors = result.Errors
        };
    }

    public async Task<CalendarUser?> GetUserByClaim(ClaimsPrincipal? authorizedUserClaim) 
    {

        var userIdClaim = authorizedUserClaim.Claims.Where(x => x.Type == "userId").FirstOrDefault();

        if (userIdClaim == null)
            return null;

        var user = await _userManager.FindByIdAsync(userIdClaim.Value);

        return user;
    }
}
