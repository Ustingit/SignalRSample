using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalRSample.Hubs
{
	public class ChatHub : Hub
	{
		public async Task SendMessageToAll(string user, string message)
		{
			await Clients.All.SendAsync("MessageReceived", user, message);
		}
	}
}
