using System.Text.RegularExpressions;
using CalendarBackend.Db;
using CalendarBackend.Identity.Requirements;
using CalendarBackend.Repository.Interfaces;
using CalendarBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;

namespace CalendarBackend.Identity.Policies;

public class RoomCreatorHandler : AuthorizationHandler<RoomCreatorRequirement>
{
    private readonly UserManager<CalendarUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserService _userRoleService;
    private readonly IRoomRepository _roomRepository;

    public RoomCreatorHandler(
        UserManager<CalendarUser> userManager,
        IHttpContextAccessor httpContextAccessor,
        UserService userRoleService,
		IRoomRepository roomRepository)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _userManager = userManager;
        _userRoleService = userRoleService;
		_roomRepository = roomRepository;
    }

    protected override async System.Threading.Tasks.Task<bool> HandleRequirementAsync(
        AuthorizationHandlerContext context,
        RoomCreatorRequirement permissionRequirement)
    {
        if (context.User == null)
        {
            context.Fail();
            return false;
        }

        var request = _httpContextAccessor.HttpContext.Request;

        var roomIdValue = 0;

        var roomIdRegexMatch = Regex.Match(request.Path.Value, @"(Room\/)[0-9]+(\/|$)");


        if (roomIdRegexMatch.Success)
        {
            roomIdValue = int.Parse(new String(roomIdRegexMatch.Value.Where(Char.IsDigit).ToArray()));
        }

        var userId = 0;


        if (!roomIdRegexMatch.Success)
        {
			request.EnableBuffering();
			var stream = new StreamReader(request.Body);
			var body = await stream.ReadToEndAsync();

			request.Body.Position = 0;

			if (body == null)
			{
				context.Fail();
				return false;
			}

            if (request.HasJsonContentType())
            {
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
            }
            else
            {
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
        }

        var user = await _userManager.FindByIdAsync(context.User.Claims.First(x => x.Type == "userId").Value);

        if (user == null)
        {
            context.Fail();
            return false;
        }

		

        var room = _roomRepository.GetById(roomIdValue);

        if (room == null)
        {
            context.Fail();
            return false;
        }

        if (room.AuthorId == user.Id)
        {
            context.Succeed(permissionRequirement);
            return true;
        }

        context.Fail();
        return false;
    }
}
