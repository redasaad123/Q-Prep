using Core.Interfaces;
using Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Linq;

namespace ProjectAPI.Hubs
{

    public class CommunityHub : Hub
    {
        private readonly IUnitOfWork<UserGroup> userGroupUnitOfWork;

        public CommunityHub(IUnitOfWork<UserGroup> UserGroupUnitOfWork)
        {
            userGroupUnitOfWork = UserGroupUnitOfWork;
        }
        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            var groups = await userGroupUnitOfWork.Entity
                .FindAll(x => x.UserID == userId);

            foreach (var group in groups)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, group.GroupName);
            }

            await base.OnConnectedAsync();

        }
        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        //public async Task JoinGroup(string groupName)
        //{
        //    var username = Context?.User?.Identity?.Name;

        //    await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        //}

        //public async Task LeaveGroup(string groupName)
        //{
        //    var userId = Context.UserIdentifier;
        //    await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        //}







    }

}
