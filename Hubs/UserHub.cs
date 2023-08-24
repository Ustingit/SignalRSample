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

		public async Task NewWindowLoaded()
		{
			_totalViews++;
			await Clients.All.SendAsync("updateTotalViews", _totalViews);
		}


	}
}
