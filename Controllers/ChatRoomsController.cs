using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalRSample.Data;
using SignalRSample.Entities;

namespace SignalRSample.Controllers
{
	public class ChatRoomsController : Controller
	{
		private readonly ApplicationDbContext _context;

		public ChatRoomsController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		[Route("/[controller]/GetChatUser")]
		public async Task<ActionResult<IEnumerable<Object>>> GetChatUser()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var users = await _context.Users.ToArrayAsync();

			if (users == null)
			{
				return NotFound();
			}

			return users.Where(_ => _.Id != userId).Select(_ => new 
			{
				Id = _.Id,
				Name = _.UserName
			}).ToList();
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<ChatRoom>>> GetChatRoom()
		{
			return await _context.ChatRooms.ToArrayAsync();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<ChatRoom>> GetChatRoom(int id)
		{
			return await _context.ChatRooms.FirstOrDefaultAsync(_ => _.Id == id);
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<ChatRoom>> UpdateChatRoom(int id, ChatRoom room)
		{
			if (id != room.Id)
				return BadRequest();

			_context.Entry(room).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				return NotFound();
			}

			return NoContent();
		}

		[HttpPost("/[controller]/PostChatRoom")]
		public async Task<ActionResult<ChatRoom>> CreateChatRoom([FromBody] ChatRoom room)
		{
			_context.ChatRooms.Add(room);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetChatRoom), new {id = room.Id});
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<ChatRoom>> DeleteChatRoom(int id)
		{
			var room = await _context.ChatRooms.FirstOrDefaultAsync(_ => _.Id == id);

			if (room == null)
			{
				return NotFound();
			}

			_context.ChatRooms.Remove(room);
			await _context.SaveChangesAsync();

			return NoContent();
		}
	}
}
