using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRSample
{
	public static class SD
	{

		static SD()
		{
			DeahlyHallowRace = new Dictionary<string, int>();
			DeahlyHallowRace.Add(Cloak, 0);
			DeahlyHallowRace.Add(Wand, 0);
			DeahlyHallowRace.Add(Stone, 0);
		}

		public const string Wand = "wand";
		public const string Stone = "stone";
		public const string Cloak = "cloak";

		public static Dictionary<string, int> DeahlyHallowRace;
	}
}
