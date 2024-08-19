using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiWithRoles.Controllers
{
	[Authorize(Roles = "SuperAdmin")]
	[Route("api/[controller]")]
	[ApiController]
	public class AdminController : ControllerBase
	{
		[HttpGet]
		public IActionResult Get()
		{
			var today = DateTime.Now.ToString();

			return Ok($"You Have Accessed Admin Controller, time: {today}");
		}
	}
}
