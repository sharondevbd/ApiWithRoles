using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiWithRoles.Controllers
{
	[Authorize(Roles = "Manager")]
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		[HttpGet]
		public IActionResult Get()
		{
			var today = DateTime.Now.ToString();

			return Ok($"You Have Accessed User Controller, time: {today}");
		}
	}
}
