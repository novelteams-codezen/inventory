using Microsoft.AspNetCore.Mvc;
using inventory.Models;
using inventory.Services;
using inventory.Entities;
using inventory.Filter;
using inventory.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;

namespace inventory.Controllers
{
    /// <summary>
    /// Controller responsible for managing openingbalanceitem related operations.
    /// </summary>
    /// <remarks>
    /// This Controller provides endpoints for adding, retrieving, updating, and deleting openingbalanceitem information.
    /// </remarks>
    [Route("api/openingbalanceitem")]
    [Authorize]
    public class OpeningBalanceItemController : ControllerBase
    {
        private readonly IOpeningBalanceItemService _openingBalanceItemService;

        /// <summary>
        /// Initializes a new instance of the OpeningBalanceItemController class with the specified context.
        /// </summary>
        /// <param name="iopeningbalanceitemservice">The iopeningbalanceitemservice to be used by the controller.</param>
        public OpeningBalanceItemController(IOpeningBalanceItemService iopeningbalanceitemservice)
        {
            _openingBalanceItemService = iopeningbalanceitemservice;
        }

        /// <summary>Adds a new openingbalanceitem</summary>
        /// <param name="model">The openingbalanceitem data to be added</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [UserAuthorize("OpeningBalanceItem",Entitlements.Create)]
        public IActionResult Post([FromBody] OpeningBalanceItem model)
        {
            var id = _openingBalanceItemService.Create(model);
            return Ok(new { id });
        }

        /// <summary>Retrieves a list of openingbalanceitems based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of openingbalanceitems</returns>
        [HttpGet]
        [UserAuthorize("OpeningBalanceItem",Entitlements.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult Get([FromQuery] string filters, string searchTerm, int pageNumber = 1, int pageSize = 10, string sortField = null, string sortOrder = "asc")
        {
            List<FilterCriteria> filterCriteria = null;
            if (pageSize < 1)
            {
                return BadRequest("Page size invalid.");
            }

            if (pageNumber < 1)
            {
                return BadRequest("Page mumber invalid.");
            }

            if (!string.IsNullOrEmpty(filters))
            {
                filterCriteria = JsonHelper.Deserialize<List<FilterCriteria>>(filters);
            }

            var result = _openingBalanceItemService.Get(filterCriteria, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return Ok(result);
        }

        /// <summary>Retrieves a specific openingbalanceitem by its primary key</summary>
        /// <param name="id">The primary key of the openingbalanceitem</param>
        /// <returns>The openingbalanceitem data</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [UserAuthorize("OpeningBalanceItem",Entitlements.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var result = _openingBalanceItemService.GetById(id);
            return Ok(result);
        }

        /// <summary>Deletes a specific openingbalanceitem by its primary key</summary>
        /// <param name="id">The primary key of the openingbalanceitem</param>
        /// <returns>The result of the operation</returns>
        [HttpDelete]
        [UserAuthorize("OpeningBalanceItem",Entitlements.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [Route("{id:Guid}")]
        public IActionResult DeleteById([FromRoute] Guid id)
        {
            var status = _openingBalanceItemService.Delete(id);
            return Ok(new { status });
        }

        /// <summary>Updates a specific openingbalanceitem by its primary key</summary>
        /// <param name="id">The primary key of the openingbalanceitem</param>
        /// <param name="updatedEntity">The openingbalanceitem data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPut]
        [UserAuthorize("OpeningBalanceItem",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] OpeningBalanceItem updatedEntity)
        {
            if (id != updatedEntity.Id)
            {
                return BadRequest("Mismatched Id");
            }

            var status = _openingBalanceItemService.Update(id, updatedEntity);
            return Ok(new { status });
        }

        /// <summary>Updates a specific openingbalanceitem by its primary key</summary>
        /// <param name="id">The primary key of the openingbalanceitem</param>
        /// <param name="updatedEntity">The openingbalanceitem data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPatch]
        [UserAuthorize("OpeningBalanceItem",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] JsonPatchDocument<OpeningBalanceItem> updatedEntity)
        {
            if (updatedEntity == null)
                return BadRequest("Patch document is missing.");
            var status = _openingBalanceItemService.Patch(id, updatedEntity);
            return Ok(new { status });
        }
    }
}