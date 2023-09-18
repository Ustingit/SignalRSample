using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SignalRSample.Entities
{
	public class ChatRoom
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("name")]
		[Required]
		public string Name { get; set; }
	}
}
