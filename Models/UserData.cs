using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiWithRoles.Models
{
	public class UserData
	{
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
		[DataType(DataType.Date)]
		public DateTime CreatedDate { get; set; } = DateTime.Now.Date;
        [NotMapped]
        public IFormFile? File { get; set; }
        public string? Picture { get; set; } = null;
    }
}
