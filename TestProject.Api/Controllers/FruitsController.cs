using System.Threading.Tasks;
using JsonApiDotNetCore.Controllers;
using JsonApiDotNetCore.Services;
using Microsoft.AspNetCore.Mvc;
using TestProject.Api.Models;

namespace TestProject.Api.Controllers
{
    public class FruitsController : JsonApiController<Fruit>
    {
        public FruitsController(
            IJsonApiContext jsonApiContext,
            IResourceService<Fruit, int> resourceService) :
            base(jsonApiContext, resourceService)
        { }

        /*
        [HttpPatch("{id}")]
        public override async Task<IActionResult> PatchAsync(int id, [FromBody] Fruit entity)
        {
            return await base.PatchAsync(id, entity);
        }
        */
    }
}
