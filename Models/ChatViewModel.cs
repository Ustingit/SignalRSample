using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRSample.Entities;

namespace SignalRSample.Models
{
	public class ChatViewModel
	{
		public ChatViewModel()
		{
			Rooms = new List<ChatRoom>();
		}

		public int MaxRoomAllowed { get; set; }

		public IList<ChatRoom> Rooms { get; set; }

		public string? UserId { get; set; }

		public bool AllowAddRoom => Rooms == null || Rooms.Count < MaxRoomAllowed;
	}
}
