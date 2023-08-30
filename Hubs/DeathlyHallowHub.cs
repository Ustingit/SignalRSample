using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalRSample.Hubs
{
	public class DeathlyHallowHub : Hub
	{
		public Dictionary<string, int> GetRaceStatus()
		{
			return SD.DeahlyHallowRace;
		}
	}
}
