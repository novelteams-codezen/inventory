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
    /// Controller responsible for managing productuom related operations.
    /// </summary>
    /// <remarks>
    /// This Controller provides endpoints for adding, retrieving, updating, and deleting productuom information.
    /// </remarks>
    [Route("api/productuom")]
    [Authorize]
    public class ProductUomController : ControllerBase
    {
        private readonly IProductUomService _productUomService;

        /// <summary>
        /// Initializes a new instance of the ProductUomController class with the specified context.
        /// </summary>
        /// <param name="iproductuomservice">The iproductuomservice to be used by the controller.</param>
        public ProductUomController(IProductUomService iproductuomservice)
        {
            _productUomService = iproductuomservice;
        }

        /// <summary>Adds a new productuom</summary>
        /// <param name="model">The productuom data to be added</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [UserAuthorize("ProductUom",Entitlements.Create)]
        public IActionResult Post([FromBody] ProductUom model)
        {
            var id = _productUomService.Create(model);
            return Ok(new { id });
        }

        /// <summary>Retrieves a list of productuoms based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of productuoms</returns>
        [HttpGet]
        [UserAuthorize("ProductUom",Entitlements.Read)]
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

            var result = _productUomService.Get(filterCriteria, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return Ok(result);
        }

        /// <summary>Retrieves a specific productuom by its primary key</summary>
        /// <param name="id">The primary key of the productuom</param>
        /// <returns>The productuom data</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [UserAuthorize("ProductUom",Entitlements.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var result = _productUomService.GetById(id);
            return Ok(result);
        }

        /// <summary>Deletes a specific productuom by its primary key</summary>
        /// <param name="id">The primary key of the productuom</param>
        /// <returns>The result of the operation</returns>
        [HttpDelete]
        [UserAuthorize("ProductUom",Entitlements.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [Route("{id:Guid}")]
        public IActionResult DeleteById([FromRoute] Guid id)
        {
            var status = _productUomService.Delete(id);
            return Ok(new { status });
        }

        /// <summary>Updates a specific productuom by its primary key</summary>
        /// <param name="id">The primary key of the productuom</param>
        /// <param name="updatedEntity">The productuom data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPut]
        [UserAuthorize("ProductUom",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] ProductUom updatedEntity)
        {
            if (id != updatedEntity.Id)
            {
                return BadRequest("Mismatched Id");
            }

            var status = _productUomService.Update(id, updatedEntity);
            return Ok(new { status });
        }

        /// <summary>Updates a specific productuom by its primary key</summary>
        /// <param name="id">The primary key of the productuom</param>
        /// <param name="updatedEntity">The productuom data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPatch]
        [UserAuthorize("ProductUom",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] JsonPatchDocument<ProductUom> updatedEntity)
        {
            if (updatedEntity == null)
                return BadRequest("Patch document is missing.");
            var status = _productUomService.Patch(id, updatedEntity);
            return Ok(new { status });
        }
    }
}