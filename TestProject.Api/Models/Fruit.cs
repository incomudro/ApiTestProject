using System.ComponentModel.DataAnnotations;

using JsonApiDotNetCore.Models;

namespace TestProject.Api.Models
{
    public class Fruit : Identifiable
    {
        [Required]
        [Attr("name")]
        public string Name { get; set; }
    }
}
