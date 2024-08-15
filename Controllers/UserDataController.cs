using ApiWithRoles.Data;
using ApiWithRoles.HelperClasses;
using ApiWithRoles.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiWithRoles.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserDataController : ControllerBase
	{
		readonly AppDbContext _context;

		public UserDataController(AppDbContext context)
		{
			_context = context;
		}
		[HttpPost]
		public IActionResult Index(UserData? model)
		{
			if( model is not null)
			{
				UserData Data = new UserData()
				{
					Name = model.Name,
					CreatedDate = DateTime.Now.Date,
					Picture = new UploadHandler().Upload(model.File)
				};
				
				_context.UserDatas.Add(Data);
				_context.SaveChanges();

			}

			return Ok();
		}
		[HttpGet("getuserdata")]
		public IActionResult GetUserData()
		{
			var data = _context.UserDatas;
			return Ok(data);
		}
		[HttpGet]
		public IActionResult GetUserDataById(int Id)
		{
			var data = _context.UserDatas.Find(Id);
			return Ok(data);
		}
		[HttpPut]
		public IActionResult EditUserData(int ID, UserData? model)
		{
			if (model is not null)
			{
				var editInfo = _context.UserDatas.Find(ID);
				if (editInfo != null)
				{
					editInfo.Name = model.Name;
					editInfo.CreatedDate = model.CreatedDate;
					if (model.File is not null)
					{
						new UploadHandler().Delete(editInfo.Picture);
						editInfo.Picture = new UploadHandler().Upload(model.File);
						//_context.Entry(model).State = EntityState.Modified;
					}

					editInfo.Picture = editInfo.Picture;
					
					//_context.Entry(editInfo).State = EntityState.Modified;
					_context.SaveChanges();
				}
				
			}
			return Ok("Edited Sucessfully");
		}
		[HttpDelete]
		public IActionResult DeleteUserData(int ID)
		{
			UserData userData = _context.UserDatas.Find(ID);
			if (userData == null)
			{
				return BadRequest();
			}

			try
			{
				var fileModel = userData.Picture;
				if (System.IO.File.Exists(fileModel))
				{
					System.IO.File.Delete(fileModel);
				}
			} catch (Exception ex)
			{
				var error = ex.Message;
				return Ok(error);
			}

			//Finally Delete from Database
			_context.UserDatas.Remove(userData);
			_context.SaveChanges();
			return Ok("Deleted Sucessfully");
		}
	}
}
