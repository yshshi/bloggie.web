using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Web.Models.Domain
{
	public class Comment
	{
        public int Id { get; set; }
        public string  Content { get; set; }
        public string User { get; set; }
		public DateTime DateCreated { get; set; }


		[ForeignKey("Post")]
		public Guid PostId { get; set; }
		public virtual BlogPost Post { get; set; }
	}
}
