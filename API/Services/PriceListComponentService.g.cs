using inventory.Models;
using inventory.Data;
using inventory.Filter;
using inventory.Entities;
using inventory.Logger;
using Microsoft.AspNetCore.JsonPatch;
using System.Linq.Expressions;

namespace inventory.Services
{
    /// <summary>
    /// The pricelistcomponentService responsible for managing pricelistcomponent related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting pricelistcomponent information.
    /// </remarks>
    public interface IPriceListComponentService
    {
        /// <summary>Retrieves a specific pricelistcomponent by its primary key</summary>
        /// <param name="id">The primary key of the pricelistcomponent</param>
        /// <returns>The pricelistcomponent data</returns>
        PriceListComponent GetById(Guid id);

        /// <summary>Retrieves a list of pricelistcomponents based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of pricelistcomponents</returns>
        List<PriceListComponent> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new pricelistcomponent</summary>
        /// <param name="model">The pricelistcomponent data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(PriceListComponent model);

        /// <summary>Updates a specific pricelistcomponent by its primary key</summary>
        /// <param name="id">The primary key of the pricelistcomponent</param>
        /// <param name="updatedEntity">The pricelistcomponent data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, PriceListComponent updatedEntity);

        /// <summary>Updates a specific pricelistcomponent by its primary key</summary>
        /// <param name="id">The primary key of the pricelistcomponent</param>
        /// <param name="updatedEntity">The pricelistcomponent data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<PriceListComponent> updatedEntity);

        /// <summary>Deletes a specific pricelistcomponent by its primary key</summary>
        /// <param name="id">The primary key of the pricelistcomponent</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The pricelistcomponentService responsible for managing pricelistcomponent related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting pricelistcomponent information.
    /// </remarks>
    public class PriceListComponentService : IPriceListComponentService
    {
        private inventoryContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the PriceListComponent class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public PriceListComponentService(inventoryContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific pricelistcomponent by its primary key</summary>
        /// <param name="id">The primary key of the pricelistcomponent</param>
        /// <returns>The pricelistcomponent data</returns>
        public PriceListComponent GetById(Guid id)
        {
            var entityData = _dbContext.PriceListComponent.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of pricelistcomponents based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of pricelistcomponents</returns>/// <exception cref="Exception"></exception>
        public List<PriceListComponent> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetPriceListComponent(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new pricelistcomponent</summary>
        /// <param name="model">The pricelistcomponent data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(PriceListComponent model)
        {
            model.Id = CreatePriceListComponent(model);
            return model.Id;
        }

        /// <summary>Updates a specific pricelistcomponent by its primary key</summary>
        /// <param name="id">The primary key of the pricelistcomponent</param>
        /// <param name="updatedEntity">The pricelistcomponent data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, PriceListComponent updatedEntity)
        {
            UpdatePriceListComponent(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific pricelistcomponent by its primary key</summary>
        /// <param name="id">The primary key of the pricelistcomponent</param>
        /// <param name="updatedEntity">The pricelistcomponent data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<PriceListComponent> updatedEntity)
        {
            PatchPriceListComponent(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific pricelistcomponent by its primary key</summary>
        /// <param name="id">The primary key of the pricelistcomponent</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeletePriceListComponent(id);
            return true;
        }
        #region
        private List<PriceListComponent> GetPriceListComponent(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.PriceListComponent.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<PriceListComponent>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(PriceListComponent), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<PriceListComponent, object>>(Expression.Convert(property, typeof(object)), parameter);
                if (sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase))
                {
                    result = result.OrderBy(lambda);
                }
                else if (sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
                {
                    result = result.OrderByDescending(lambda);
                }
                else
                {
                    throw new ApplicationException("Invalid sort order. Use 'asc' or 'desc'");
                }
            }

            var paginatedResult = result.Skip(skip).Take(pageSize).ToList();
            return paginatedResult;
        }

        private Guid CreatePriceListComponent(PriceListComponent model)
        {
            _dbContext.PriceListComponent.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdatePriceListComponent(Guid id, PriceListComponent updatedEntity)
        {
            _dbContext.PriceListComponent.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeletePriceListComponent(Guid id)
        {
            var entityData = _dbContext.PriceListComponent.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.PriceListComponent.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchPriceListComponent(Guid id, JsonPatchDocument<PriceListComponent> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.PriceListComponent.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.PriceListComponent.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}