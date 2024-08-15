using ApiWithRoles.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiWithRoles.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountsController : ControllerBase
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IConfiguration _configuration;

        public AccountsController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_configuration = configuration;
		}
		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] Register model)
		{
			var user= new IdentityUser { UserName = model.Username, Email = model.Email, PhoneNumber=model.PhoneNumber};
			var result = await _userManager.CreateAsync(user,model.Password);
			if (result.Succeeded)
			{
				return Ok(new { message = "User Registered Sucessfully" });
			}
			return BadRequest(result.Errors);
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] Login model)
		{
			var user = await _userManager.FindByNameAsync(model.Username);
			if (user!=null && await _userManager.CheckPasswordAsync(user, model.Password))
			{
				var userRoles = await _userManager.GetRolesAsync(user);
				var authClaims = new List<Claim>
				{
					new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
				};

				authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
				var token = new JwtSecurityToken(
					issuer: _configuration["Jwt:Issuer"],
					expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpiryMinutes"]!)),
					claims: authClaims,
					signingCredentials: new SigningCredentials(
						new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
						SecurityAlgorithms.HmacSha256)
					); //End Token

				return Ok(new {Token = new JwtSecurityTokenHandler().WriteToken(token)});
			}
			return Unauthorized();
		}

		[HttpPost("add-role")]
		public async Task<IActionResult> AddRole([FromBody] string role)
		{
			if(!await _roleManager.RoleExistsAsync(role))
			{
				var result = await _roleManager.CreateAsync(new IdentityRole(role));
				if (result.Succeeded)
				{
					return Ok(new { message = "Role Added Sucessfully" });
				}
				return BadRequest(result.Errors);
			}
			return BadRequest("Role Already Exist");
		}

		[HttpPost("assign-role")]
		public async Task<IActionResult> AssignRole([FromBody] UserRole model)
		{
			var user = await _userManager.FindByNameAsync(model.Username);
			if (user == null)
			{
				return BadRequest("User Not Found");
			}
			var result = await _userManager.AddToRoleAsync(user, model.Role);
			if (result.Succeeded)
			{
				return Ok(new { message = "Role Assigned Sucessfully" });
			}
			return BadRequest(result.Errors);
		}


	}
}
