using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalRSample.Hubs
{
	public class NotificationHub : Hub
	{
		public static int _counter = 0;
		public static List<string> _messages = new List<string>();

		public async Task SendNotification(string message)
		{
			if (!string.IsNullOrEmpty(message))
			{
				_counter++;
				_messages.Add(message);
				await LoadNotifications();
			}
		}

		public async Task LoadNotifications()
		{
			await Clients.All.SendAsync("LoadNotifications", _counter, _messages);
		}
	}
}
