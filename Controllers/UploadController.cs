using ApiWithRoles.HelperClasses;
using ApiWithRoles.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiWithRoles.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UploadController : ControllerBase
	{
		[HttpPost ("upload")]
		public IActionResult UploadFile(IFormFile file)
		{
			return Ok(new UploadHandler().Upload(file));
		}
	}
}
