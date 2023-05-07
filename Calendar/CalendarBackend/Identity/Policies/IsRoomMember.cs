using CalendarBackend.Db;
using CalendarBackend.Identity.Requirements;
using CalendarBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;

namespace CalendarBackend.Identity.Policies;

public class RoomMemberHandler : AuthorizationHandler<RoomMemberRequirement>
{
    private readonly CalendarDevContext _calendarDevContext;
    private readonly UserManager<CalendarUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserService _userRoleService;

    public RoomMemberHandler(
        CalendarDevContext calendarDevContext,
        UserManager<CalendarUser> userManager,
        IHttpContextAccessor httpContextAccessor,
        UserService userRoleService)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _userManager = userManager;
        _calendarDevContext = calendarDevContext;
        _userRoleService = userRoleService;
    }

    protected override async System.Threading.Tasks.Task<bool> HandleRequirementAsync(
        AuthorizationHandlerContext context,
        RoomMemberRequirement permissionRequirement)
    {
        if (context.User == null)
        {
			context.Fail();
            return false;
        }

        var request = _httpContextAccessor.HttpContext.Request;
		request.EnableBuffering();
        var stream = new StreamReader(request.Body);
        var body = await stream.ReadToEndAsync();

        request.Body.Position = 0;

        if (body == null)
        {
            context.Fail();
            return false;
        }

		var roomIdValue = 0;
		var userId = 0;

		if (request.HasJsonContentType()){
			JObject parsedJson = null;

			try
			{
				parsedJson = JObject.Parse(body);
			}
			catch (Exception e)
			{
				context.Fail();
				return false;
			}

			if (parsedJson == null)
			{
				context.Fail();
				return false;
			}

			var roomId = parsedJson.GetValue("RoomId");

			if (roomId == null)
			{
				context.Fail();
				return false;
			}

			roomIdValue = int.Parse(roomId.ToString());
		} else {
			try
			{
				roomIdValue = int.Parse(request.Form.Where(x => x.Key == "RoomId").FirstOrDefault().Value);
			}
			catch
			{
				context.Fail();
				return false;
			}
		}

		var user = await _userManager.FindByIdAsync(context.User.Claims.First(x => x.Type == "userId").Value);

		if (user == null)
		{
			context.Fail();
			return false;
		}

        var userRole = await _userRoleService.GetUserRoleByRoom(user.Id, roomIdValue);

        if (userRole == null)
        {
            context.Fail();
            return false;
        }

        if (userRole.Id == 2)
        {
            context.Succeed(permissionRequirement);
            return true;
        }

        context.Fail();
        return false;
    }
}
