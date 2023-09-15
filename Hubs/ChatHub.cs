using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalRSample.Data;

namespace SignalRSample.Hubs
{
	public class ChatHub : Hub
	{
		private readonly ApplicationDbContext _dbContext;

		public ChatHub(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task SendMessageToAll(string user, string message)
		{
			await Clients.All.SendAsync("MessageReceived", user, message);
		}

		[Authorize] // means only authorized user can send private message
		public async Task SendMessageToReceiver(string sender, string receiver, string message)
		{
			var cancellationToken = CancellationToken.None;
			var receiverUser = await _dbContext.Users.FirstOrDefaultAsync(_ => _.Email.ToLower() == receiver.ToLower(), cancellationToken);

			if (receiverUser != null)
			{
				await Clients.User(receiverUser.Id).SendAsync("MessageReceived", sender, message, cancellationToken);
			}
			else
			{
				throw new ArgumentException("User not found");
			}
		}
	}
}
