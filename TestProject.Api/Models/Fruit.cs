using System.ComponentModel.DataAnnotations;

using JsonApiDotNetCore.Models;

namespace TestProject.Api.Models
{
    public class Fruit : Identifiable
    {
        [Required]
        [Attr("Name")]
        public string Name { get; set; }

        public Fruit()
        {
            Name = "Freddy Mercury";
        }
    }
}
