using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ApiWithRoles.HelperClasses
{
	public class UploadHandler
	{
		public string Upload(IFormFile? file)
		{
			if (file != null)
			{
				//Extension
				List<string> validExtension = new List<string>() { ".jpg", ".png", "gif", "docx", "pdf" };
				string extension = Path.GetExtension(file.FileName);
				if (!validExtension.Contains(extension))
				{
					return $"Extension is not Valid ({string.Join(',', validExtension)})";
				}

				//fileSize
				long size = file.Length;
				if (size > (5 * 1024 * 1024))
				{
					return "Maximum File Size is 5 Mb";
				}

				//name Changing
				string fileName = Guid.NewGuid().ToString() + extension;
				//Path
				string path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
				using FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
				file.CopyTo(stream);

				return Path.Combine(path, fileName);
			}
			return "";
		}

		public void Delete(string filePath)
		{
			if (filePath == null) {
				return;
			}
			if (System.IO.File.Exists(filePath))
			{
				System.IO.File.Delete(filePath);
			}
		}

	}
}
