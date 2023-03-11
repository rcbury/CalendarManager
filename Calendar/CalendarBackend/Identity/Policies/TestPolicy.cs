using CalendarBackend.Db;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

public class PermissionRequirement : IAuthorizationRequirement
{
}

public class RoomAdminHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly CalendarDevContext _calendarDevContext;
    private readonly UserManager<CalendarUser> _userManager;

    public RoomAdminHandler(CalendarDevContext calendarDevContext, UserManager<CalendarUser> userManager)
    {
        _userManager = userManager;
        _calendarDevContext = calendarDevContext;
    }

    protected override async System.Threading.Tasks.Task<bool> HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement permissionRequirement)
    {
        if (context.User == null)
        {
            // no user authorized. Alternatively call context.Fail() to ensure a failure 
            // as another handler for this requirement may succeed
            return false;
        }

        var user = await _userManager.FindByIdAsync(context.User.Claims.First(x => x.Type == "userId").Value);

		if (user == null)
		{
			return false;
		}

        Console.WriteLine(user.UserName);

        var permissionCheck = true;

        if (permissionCheck)
        {
            context.Succeed(permissionRequirement);
        }

        return true;
    }
}
