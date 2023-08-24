using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalRSample.Hubs
{
	public class UserHub : Hub
	{
		private static int _totalViews = 0;
		private static int _totalUsers = 0;

		public async Task NewWindowLoaded()
		{
			_totalViews++;
			await Clients.All.SendAsync("updateTotalViews", _totalViews);
		}

		public override Task OnConnectedAsync()
		{
			_totalUsers++;
			Clients.All.SendAsync("updateTotalUsers", _totalUsers).GetAwaiter().GetResult();
			return base.OnConnectedAsync();
		}

		public override Task OnDisconnectedAsync(Exception? exception)
		{
			_totalUsers--;
			Clients.All.SendAsync("updateTotalUsers", _totalUsers).GetAwaiter().GetResult();
			return base.OnDisconnectedAsync(exception);
		}
	}
}
