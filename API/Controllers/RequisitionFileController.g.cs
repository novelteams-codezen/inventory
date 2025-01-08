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
    /// Controller responsible for managing requisitionfile related operations.
    /// </summary>
    /// <remarks>
    /// This Controller provides endpoints for adding, retrieving, updating, and deleting requisitionfile information.
    /// </remarks>
    [Route("api/requisitionfile")]
    [Authorize]
    public class RequisitionFileController : ControllerBase
    {
        private readonly IRequisitionFileService _requisitionFileService;

        /// <summary>
        /// Initializes a new instance of the RequisitionFileController class with the specified context.
        /// </summary>
        /// <param name="irequisitionfileservice">The irequisitionfileservice to be used by the controller.</param>
        public RequisitionFileController(IRequisitionFileService irequisitionfileservice)
        {
            _requisitionFileService = irequisitionfileservice;
        }

        /// <summary>Adds a new requisitionfile</summary>
        /// <param name="model">The requisitionfile data to be added</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [UserAuthorize("RequisitionFile",Entitlements.Create)]
        public IActionResult Post([FromBody] RequisitionFile model)
        {
            var id = _requisitionFileService.Create(model);
            return Ok(new { id });
        }

        /// <summary>Retrieves a list of requisitionfiles based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of requisitionfiles</returns>
        [HttpGet]
        [UserAuthorize("RequisitionFile",Entitlements.Read)]
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

            var result = _requisitionFileService.Get(filterCriteria, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return Ok(result);
        }

        /// <summary>Retrieves a specific requisitionfile by its primary key</summary>
        /// <param name="id">The primary key of the requisitionfile</param>
        /// <returns>The requisitionfile data</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [UserAuthorize("RequisitionFile",Entitlements.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var result = _requisitionFileService.GetById(id);
            return Ok(result);
        }

        /// <summary>Deletes a specific requisitionfile by its primary key</summary>
        /// <param name="id">The primary key of the requisitionfile</param>
        /// <returns>The result of the operation</returns>
        [HttpDelete]
        [UserAuthorize("RequisitionFile",Entitlements.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [Route("{id:Guid}")]
        public IActionResult DeleteById([FromRoute] Guid id)
        {
            var status = _requisitionFileService.Delete(id);
            return Ok(new { status });
        }

        /// <summary>Updates a specific requisitionfile by its primary key</summary>
        /// <param name="id">The primary key of the requisitionfile</param>
        /// <param name="updatedEntity">The requisitionfile data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPut]
        [UserAuthorize("RequisitionFile",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] RequisitionFile updatedEntity)
        {
            if (id != updatedEntity.Id)
            {
                return BadRequest("Mismatched Id");
            }

            var status = _requisitionFileService.Update(id, updatedEntity);
            return Ok(new { status });
        }

        /// <summary>Updates a specific requisitionfile by its primary key</summary>
        /// <param name="id">The primary key of the requisitionfile</param>
        /// <param name="updatedEntity">The requisitionfile data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPatch]
        [UserAuthorize("RequisitionFile",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] JsonPatchDocument<RequisitionFile> updatedEntity)
        {
            if (updatedEntity == null)
                return BadRequest("Patch document is missing.");
            var status = _requisitionFileService.Patch(id, updatedEntity);
            return Ok(new { status });
        }
    }
}