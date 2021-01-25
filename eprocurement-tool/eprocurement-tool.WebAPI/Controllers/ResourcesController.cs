using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EGPS.WebAPI.Controllers
{
    [Route("api/v1/resources")]
    [ApiController]
    [Authorize]
    public class ResourcesController : ControllerBase
    {
        private readonly IResourceRepository _resourceRepository;
        public ResourcesController(IResourceRepository resourceRepository)
        {
            _resourceRepository = resourceRepository ?? throw new ArgumentNullException(nameof(resourceRepository));
        }

        [HttpGet]
        public async Task<IActionResult> GetResources(string name, string searchQuery)
        {
            try
            {
                var resources = await _resourceRepository.GetResources(name, searchQuery);

                return Ok(new
                {
                    success = true,
                    message = "Successfully retrieved resources",
                    resources
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }
    }
}
