using System.Security.Cryptography;
using System.Text;

namespace CalendarBackend.Services;

public class InviteLinkTokenGeneratorService
{
    private const string secret = "meritapasyasafalhui";
    private const string salt = "merittapasyasalthui";
    private const int LinkTimeoutSeconds = 60 * 60 * 24;

    public string GenerateRoomInviteTokenWithTimestamp(int roomId, long timestamp)
    {
        //var timestampb36 = Convert.ToString(timestamp, 36);

        var roomHash = GetRoomHmac(salt, roomId.ToString(), timestamp);

        Console.WriteLine(roomHash);

        var tokenValue = $"{timestamp}-{roomHash}";

        Console.WriteLine("generatedtoken: " + roomHash);

        return tokenValue;
    }

    public string GenerateRoomInviteToken(int roomId)
    {
        var timestampNow = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        return this.GenerateRoomInviteTokenWithTimestamp(roomId, timestampNow);
    }

    private string GetRoomHmac(string salt, string roomdId, long timestamp)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            var stringToHash = roomdId + salt + secret + timestamp;
            // ComputeHash - returns byte array  
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));

            // Convert byte array to a string   
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            var result = builder.ToString().Take(16);

            return String.Join("", result);
        }
    }

    public bool CheckToken(string token, int roomId)
    {
        var splittedToken = token.Split("-");
        //var timestampb36 = splittedToken[0];
        var tokenTimestamp = long.Parse(splittedToken[0]);
        var hash = splittedToken[1];

        var testToken = GenerateRoomInviteTokenWithTimestamp(roomId, tokenTimestamp);

        Console.WriteLine("token to check: " + token);
        Console.WriteLine("generated token: " + testToken);

        if (testToken != token)
        {
            return false;
        }

        var timestampNow = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        var tokenDatetime = DateTimeOffset.FromUnixTimeSeconds(tokenTimestamp).DateTime;
        var nowDatetime = DateTimeOffset.FromUnixTimeSeconds(timestampNow).DateTime;

        if ((nowDatetime - tokenDatetime).TotalSeconds < LinkTimeoutSeconds)
        {
            return true;
        }

        return false;
    }

    public string? GetInviteLink(string token, int roomId)
    {
		//probably move this to env variable
		var baseUrl = "https://localhost:7132";

		var inviteUrl = $"{baseUrl}/Room/{roomId}/acceptInvite?token={token}";

		return inviteUrl;
    }

}
