using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Digipolis.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StarterKit.Api.Filters;
using StarterKit.Api.Models;
using StarterKit.Api.Models.Examples;
using StarterKit.Shared.Constants;
using StarterKit.Shared.Options;

namespace StarterKit.Api.Controllers
{
    // #VERSIONING: Controller wide versioning
    // you can change this to endpoint numbering
    // this does mean you need to version every endpoint
    // could be neccessary for backwards compatibility
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController, ApiVersion(Versions.V1)]
    public class ExamplesController : Controller
    {
        public ExamplesController(ILogger<ExamplesController> logger, IOptions<AppSettings> appSettings)
        {
            Logger = logger;

            // This is an example of how you inject configuration options that are read from config files and registered in Startup.cs.
            AppSettings = appSettings?.Value
                ?? throw new ArgumentNullException($"{GetType().Name}.Ctor - Parameter {nameof(appSettings)} cannot be null.");
        }

        public ILogger<ExamplesController> Logger { get; }
        public AppSettings AppSettings { get; }

        // Normally this data would come from an injected business or repository class
        private readonly List<Example> _examples = new List<Example>()
        {
            new Example() { Id = 1, Name = "Peter Parker" },
            new Example() { Id = 2, Name = "Clark Kent" },
            new Example() { Id = 3, Name = "Bruce Wayne" }
        };

        // GET /api/examples
        /// <summary>
        /// Get examples
        /// </summary>
        [HttpGet]
        [Consumes(MediaType.Json)]
        [Produces(MediaType.Json)]
        [ProducesResponseType(typeof(List<Example>), (int)HttpStatusCode.OK)]
        public IActionResult GetAll()
        {
            // this is how you log an information message
            Logger.LogInformation("Consumer requested all examples.");

            // this will return a HTTP Status Code 200 (OK) along with the data
            return Ok(_examples);
        }

        // GET /api/examples/2
        /// <summary>
        /// Get example
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        [Consumes(MediaType.Json)]
        [Produces(MediaType.Json)]
        [ProducesResponseType(typeof(Example), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.NotFound)]
        public IActionResult Get(int id)
        {
            var example = _examples.FirstOrDefault(e => e.Id == id);

            if (example == null)
            {
                // This is how you log a warning message
                Logger.LogWarning("Consumer requested example {0} that does not exist.", id);

                // this will return a HTTP Status Code 404 (Not Found) along with the message
                return NotFound($"No example found with id = {id}");
            }

            // this will return a HTTP Status Code 200 (OK) along with the data
            return Ok(example);
        }

        // POST /api/examples
        /// <summary>
        /// Create example
        /// </summary>
        /// <param name="example"></param>
        [HttpPost]
        [ValidateModelState]
        [Consumes(MediaType.Json)]
        [Produces(MediaType.Json)]
        [ProducesResponseType(typeof(Example), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.InternalServerError)]
        public IActionResult Create(Example example)
        {
            // try posting an example object with no name to get a non-valid model           

            Logger.LogInformation("Consumer added an example with Name = {0}", example.Name);

            // if the save succeeds, return a HTTP Status Code 201 (Created) along with the route where the consumer can request the new record
            return CreatedAtAction("Get", new { id = example.Id });
        }
    }
}
