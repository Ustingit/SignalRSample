using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalRSample.Data;

namespace SignalRSample.Hubs
{
	public class AdvancedChatHub : Hub
	{
		private readonly ApplicationDbContext _context;

		public AdvancedChatHub(ApplicationDbContext context)
		{
			_context = context;
		}

		public override Task OnConnectedAsync()
		{
			var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (!string.IsNullOrEmpty(userId))
			{
				var userName = (_context.Users.FirstOrDefault(_ => _.Id == userId))?.UserName;

				Clients.Users(HubConnections.OnlineUsers()).SendAsync("ReceiveUserConnected", userId, userName, HubConnections.HasUser(userId));
				HubConnections.AddUserConnection(userId, Context.ConnectionId);
			}

			return base.OnConnectedAsync();
		}

		public override Task OnDisconnectedAsync(Exception? exception)
		{
			var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (HubConnections.HasUserConnection(userId, Context.ConnectionId))
			{
				var userConnections = HubConnections.Users[userId];
				userConnections.Remove(Context.ConnectionId);

				HubConnections.Users.Remove(userId);
				if (userConnections.Any())
				{
					HubConnections.Users.Add(userId, userConnections);
				}
			}

			if (!string.IsNullOrEmpty(userId))
			{
				var userName = (_context.Users.FirstOrDefault(_ => _.Id == userId))?.UserName;
				
				Clients.Users(HubConnections.OnlineUsers()).SendAsync("ReceiveUserDisconnected", userId, userName, HubConnections.HasUser(userId));
				HubConnections.AddUserConnection(userId, Context.ConnectionId);
			}

			return base.OnDisconnectedAsync(exception);
		}

		public async Task SendAddRoomMessage(int maxRoom, int roomId, string roomName)
		{
			var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (!string.IsNullOrEmpty(userId))
			{
				var userName = (_context.Users.FirstOrDefault(_ => _.Id == userId))?.UserName;

				await Clients.All.SendAsync("ReceiveAddRoomMessage", maxRoom, roomId, roomName, userId, userName);
			}
		}

		public async Task SendDeleteRoomMessage(int deletedRoom, int selectedRoom, string roomName)
		{
			var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (!string.IsNullOrEmpty(userId))
			{
				var userName = (_context.Users.FirstOrDefault(_ => _.Id == userId))?.UserName;

				await Clients.All.SendAsync("ReceiveDeleteRoomMessage", deletedRoom, selectedRoom, roomName, userId, userName);
			}
		}
	}

	public static class HubConnections
	{
		// userid == connectionid
		// user can have multiple connections
		public static Dictionary<string, List<string>> Users = new Dictionary<string, List<string>>();

		public static bool HasUserConnection(string UserId, string ConnectionId)
		{
			try
			{
				if (Users.ContainsKey(UserId))
				{
					return Users[UserId].Any(p => p.Contains(ConnectionId));
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return false;
		}

		public static bool HasUser(string userId)
		{
			try
			{
				if (Users.TryGetValue(userId, out var connections))
				{
					return connections.Any();
				}
			}
			catch (Exception ex)
			{
				return false;
			}

			return false;
		}

		public static void AddUserConnection(string UserId, string ConnectionId)
		{

			if (!string.IsNullOrEmpty(UserId) && !HasUserConnection(UserId, ConnectionId))
			{
				if (Users.ContainsKey(UserId))
					Users[UserId].Add(ConnectionId);
				else
					Users.Add(UserId, new List<string> { ConnectionId });
			}
		}

		public static List<string> OnlineUsers()
		{
			return Users.Keys.ToList();
		}
    }
}
