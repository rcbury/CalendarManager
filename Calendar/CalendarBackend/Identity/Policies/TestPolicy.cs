using CalendarBackend.Db;
using CalendarBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;

public class RoomAdminRequirement : IAuthorizationRequirement
{
}

public class RoomAdminHandler : AuthorizationHandler<RoomAdminRequirement>
{
    private readonly CalendarDevContext _calendarDevContext;
    private readonly UserManager<CalendarUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserRoleService _userRoleService;

    public RoomAdminHandler(
        CalendarDevContext calendarDevContext,
        UserManager<CalendarUser> userManager,
        IHttpContextAccessor httpContextAccessor,
        UserRoleService userRoleService)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _userManager = userManager;
        _calendarDevContext = calendarDevContext;
        _userRoleService = userRoleService;
    }

    protected override async System.Threading.Tasks.Task<bool> HandleRequirementAsync(
        AuthorizationHandlerContext context,
        RoomAdminRequirement permissionRequirement)
    {
        if (context.User == null)
        {
            // no user authorized. Alternatively call context.Fail() to ensure a failure 
            // as another handler for this requirement may succeed
            return false;
        }

        var request = _httpContextAccessor.HttpContext.Request;
        var stream = new StreamReader(request.Body);
        var body = stream.ReadToEnd();

        var parsedJson = JObject.Parse(body);

		var teamId = parsedJson.GetValue("TeamId");

		if (teamId == null)
		{
			context.Fail();
		}

        var teamIdValue = int.Parse(teamId.ToString());

        var user = await _userManager.FindByIdAsync(context.User.Claims.First(x => x.Type == "userId").Value);

        if (user == null)
        {
			context.Fail();
            return false;
        }

        var userRole = _userRoleService.GetUserRoomRole(user.Id, teamIdValue);

        var permissionCheck = true;

        if (permissionCheck)
        {
            context.Succeed(permissionRequirement);
        }

        return true;
    }
}
