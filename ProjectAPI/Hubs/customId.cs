using Core.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ProjectAPI.Hubs
{
    public class customId : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;


        }
    }
}
