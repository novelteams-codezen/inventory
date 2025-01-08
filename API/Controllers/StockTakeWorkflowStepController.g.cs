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
    /// Controller responsible for managing stocktakeworkflowstep related operations.
    /// </summary>
    /// <remarks>
    /// This Controller provides endpoints for adding, retrieving, updating, and deleting stocktakeworkflowstep information.
    /// </remarks>
    [Route("api/stocktakeworkflowstep")]
    [Authorize]
    public class StockTakeWorkflowStepController : ControllerBase
    {
        private readonly IStockTakeWorkflowStepService _stockTakeWorkflowStepService;

        /// <summary>
        /// Initializes a new instance of the StockTakeWorkflowStepController class with the specified context.
        /// </summary>
        /// <param name="istocktakeworkflowstepservice">The istocktakeworkflowstepservice to be used by the controller.</param>
        public StockTakeWorkflowStepController(IStockTakeWorkflowStepService istocktakeworkflowstepservice)
        {
            _stockTakeWorkflowStepService = istocktakeworkflowstepservice;
        }

        /// <summary>Adds a new stocktakeworkflowstep</summary>
        /// <param name="model">The stocktakeworkflowstep data to be added</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [UserAuthorize("StockTakeWorkflowStep",Entitlements.Create)]
        public IActionResult Post([FromBody] StockTakeWorkflowStep model)
        {
            var id = _stockTakeWorkflowStepService.Create(model);
            return Ok(new { id });
        }

        /// <summary>Retrieves a list of stocktakeworkflowsteps based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of stocktakeworkflowsteps</returns>
        [HttpGet]
        [UserAuthorize("StockTakeWorkflowStep",Entitlements.Read)]
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

            var result = _stockTakeWorkflowStepService.Get(filterCriteria, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return Ok(result);
        }

        /// <summary>Retrieves a specific stocktakeworkflowstep by its primary key</summary>
        /// <param name="id">The primary key of the stocktakeworkflowstep</param>
        /// <returns>The stocktakeworkflowstep data</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [UserAuthorize("StockTakeWorkflowStep",Entitlements.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var result = _stockTakeWorkflowStepService.GetById(id);
            return Ok(result);
        }

        /// <summary>Deletes a specific stocktakeworkflowstep by its primary key</summary>
        /// <param name="id">The primary key of the stocktakeworkflowstep</param>
        /// <returns>The result of the operation</returns>
        [HttpDelete]
        [UserAuthorize("StockTakeWorkflowStep",Entitlements.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [Route("{id:Guid}")]
        public IActionResult DeleteById([FromRoute] Guid id)
        {
            var status = _stockTakeWorkflowStepService.Delete(id);
            return Ok(new { status });
        }

        /// <summary>Updates a specific stocktakeworkflowstep by its primary key</summary>
        /// <param name="id">The primary key of the stocktakeworkflowstep</param>
        /// <param name="updatedEntity">The stocktakeworkflowstep data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPut]
        [UserAuthorize("StockTakeWorkflowStep",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] StockTakeWorkflowStep updatedEntity)
        {
            if (id != updatedEntity.Id)
            {
                return BadRequest("Mismatched Id");
            }

            var status = _stockTakeWorkflowStepService.Update(id, updatedEntity);
            return Ok(new { status });
        }

        /// <summary>Updates a specific stocktakeworkflowstep by its primary key</summary>
        /// <param name="id">The primary key of the stocktakeworkflowstep</param>
        /// <param name="updatedEntity">The stocktakeworkflowstep data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPatch]
        [UserAuthorize("StockTakeWorkflowStep",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] JsonPatchDocument<StockTakeWorkflowStep> updatedEntity)
        {
            if (updatedEntity == null)
                return BadRequest("Patch document is missing.");
            var status = _stockTakeWorkflowStepService.Patch(id, updatedEntity);
            return Ok(new { status });
        }
    }
}