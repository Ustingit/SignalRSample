using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRSample.Entities
{
	public class Order
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string ItemName { get; set; }
		public int Count { get; set; }
	}
}
