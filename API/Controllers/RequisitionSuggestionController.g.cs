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
    /// Controller responsible for managing requisitionsuggestion related operations.
    /// </summary>
    /// <remarks>
    /// This Controller provides endpoints for adding, retrieving, updating, and deleting requisitionsuggestion information.
    /// </remarks>
    [Route("api/requisitionsuggestion")]
    [Authorize]
    public class RequisitionSuggestionController : ControllerBase
    {
        private readonly IRequisitionSuggestionService _requisitionSuggestionService;

        /// <summary>
        /// Initializes a new instance of the RequisitionSuggestionController class with the specified context.
        /// </summary>
        /// <param name="irequisitionsuggestionservice">The irequisitionsuggestionservice to be used by the controller.</param>
        public RequisitionSuggestionController(IRequisitionSuggestionService irequisitionsuggestionservice)
        {
            _requisitionSuggestionService = irequisitionsuggestionservice;
        }

        /// <summary>Adds a new requisitionsuggestion</summary>
        /// <param name="model">The requisitionsuggestion data to be added</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [UserAuthorize("RequisitionSuggestion",Entitlements.Create)]
        public IActionResult Post([FromBody] RequisitionSuggestion model)
        {
            var id = _requisitionSuggestionService.Create(model);
            return Ok(new { id });
        }

        /// <summary>Retrieves a list of requisitionsuggestions based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of requisitionsuggestions</returns>
        [HttpGet]
        [UserAuthorize("RequisitionSuggestion",Entitlements.Read)]
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

            var result = _requisitionSuggestionService.Get(filterCriteria, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return Ok(result);
        }

        /// <summary>Retrieves a specific requisitionsuggestion by its primary key</summary>
        /// <param name="id">The primary key of the requisitionsuggestion</param>
        /// <returns>The requisitionsuggestion data</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [UserAuthorize("RequisitionSuggestion",Entitlements.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var result = _requisitionSuggestionService.GetById(id);
            return Ok(result);
        }

        /// <summary>Deletes a specific requisitionsuggestion by its primary key</summary>
        /// <param name="id">The primary key of the requisitionsuggestion</param>
        /// <returns>The result of the operation</returns>
        [HttpDelete]
        [UserAuthorize("RequisitionSuggestion",Entitlements.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [Route("{id:Guid}")]
        public IActionResult DeleteById([FromRoute] Guid id)
        {
            var status = _requisitionSuggestionService.Delete(id);
            return Ok(new { status });
        }

        /// <summary>Updates a specific requisitionsuggestion by its primary key</summary>
        /// <param name="id">The primary key of the requisitionsuggestion</param>
        /// <param name="updatedEntity">The requisitionsuggestion data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPut]
        [UserAuthorize("RequisitionSuggestion",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] RequisitionSuggestion updatedEntity)
        {
            if (id != updatedEntity.Id)
            {
                return BadRequest("Mismatched Id");
            }

            var status = _requisitionSuggestionService.Update(id, updatedEntity);
            return Ok(new { status });
        }

        /// <summary>Updates a specific requisitionsuggestion by its primary key</summary>
        /// <param name="id">The primary key of the requisitionsuggestion</param>
        /// <param name="updatedEntity">The requisitionsuggestion data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPatch]
        [UserAuthorize("RequisitionSuggestion",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] JsonPatchDocument<RequisitionSuggestion> updatedEntity)
        {
            if (updatedEntity == null)
                return BadRequest("Patch document is missing.");
            var status = _requisitionSuggestionService.Patch(id, updatedEntity);
            return Ok(new { status });
        }
    }
}