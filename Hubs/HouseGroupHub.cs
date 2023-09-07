using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalRSample.Hubs
{
	public class HouseGroupHub : Hub
	{
		private static List<string> _joinedGroups = new List<string>();

		public async Task JoinHouse(string houseName)
		{
			var key = GetKey(houseName);
			if (!_joinedGroups.Contains(key))
			{
				_joinedGroups.Add(key);
				await Clients.Caller.SendAsync("subscriptionStatus", GetJoinedGroups(), houseName, true);
				await Groups.AddToGroupAsync(Context.ConnectionId, houseName);
			}
		}

		public async Task LeaveHouse(string houseName)
		{
			var key = GetKey(houseName);
			if (_joinedGroups.Contains(key))
			{
				_joinedGroups.Remove(key);
				await Clients.Caller.SendAsync("subscriptionStatus", GetJoinedGroups(), houseName, false);
				await Groups.RemoveFromGroupAsync(Context.ConnectionId, houseName);
			}
		}

		private string GetJoinedGroups()
		{
			var result = "";

			foreach (var joinedGroup in _joinedGroups)
			{
				if (joinedGroup.Contains(Context.ConnectionId))
				{
					result += joinedGroup.Split(":")[1] + " ";
				}
			}

			return result;
		}

		private string GetKey(string groupName) => $"{Context.ConnectionId}:{groupName}";
	}
}
