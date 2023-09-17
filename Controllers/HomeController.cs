using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalRSample.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalRSample.Data;
using SignalRSample.Entities;
using SignalRSample.Hubs;

namespace SignalRSample.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IHubContext<DeathlyHallowHub> _context;
		private readonly ApplicationDbContext _dbContext;
		private readonly IHubContext<OrderHub> _orderHubContext;

		public HomeController(ILogger<HomeController> logger, 
			IHubContext<DeathlyHallowHub> context, 
			ApplicationDbContext dbContext,
			IHubContext<OrderHub> orderHubContext)
		{
			_logger = logger;
			_context = context;
			_dbContext = dbContext;
			_orderHubContext = orderHubContext;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		public IActionResult Notification()
		{
			return View();
		}

		public IActionResult DeathlyHallowRace()
		{
			return View();
		}

		public IActionResult HarryPotterHouse()
		{
			return View();
		}

		public IActionResult BasicChat()
		{
			return View();
		}

		[Authorize]
		public IActionResult AdvancedChat()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			return View(new ChatViewModel()
			{
				Rooms = _dbContext.ChatRooms.ToList(),
				MaxRoomAllowed = 4,
				UserId = userId
			});
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		public async Task<IActionResult> DeathlyHallows(string type)
		{
			SD.DeahlyHallowRace[type] += 1;

			//send notification that model has updated
			await _context.Clients.All.SendAsync("updateHallowCounter",
				SD.DeahlyHallowRace[SD.Cloak],
				SD.DeahlyHallowRace[SD.Stone],
				SD.DeahlyHallowRace[SD.Wand]);

			return Ok();
		}

		[ActionName("Order")]
		public async Task<IActionResult> Order()
		{
			string[] name = { "Bhrugen", "Ben", "Jess", "Laura", "Ron" };
			string[] itemName = { "Food1", "Food2", "Food3", "Food4", "Food5" };

			Random rand = new Random();
			// Generate a random index less than the size of the array.  
			int index = rand.Next(name.Length);

			Order order = new Order()
			{
				Name = name[index],
				ItemName = itemName[index],
				Count = index
			};
			
			return View(order);
		}

		[ActionName("Order")]
		[HttpPost]
		public async Task<IActionResult> OrderPost(Order order)
		{

			_dbContext.Orders.Add(order);
			_dbContext.SaveChanges();
			await _orderHubContext.Clients.All.SendAsync("OrderIsCreated");

			return RedirectToAction(nameof(Order));
		}

		[ActionName("OrderList")]
		public async Task<IActionResult> OrderList()
		{
			return View();
		}

		[HttpGet]
		public IActionResult GetAllOrder()
		{
			var productList = _dbContext.Orders.ToList();
			return Json(new { data = productList });
		}
	}
}
