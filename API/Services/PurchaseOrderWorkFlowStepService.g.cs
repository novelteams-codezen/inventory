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
    /// The purchaseorderworkflowstepService responsible for managing purchaseorderworkflowstep related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting purchaseorderworkflowstep information.
    /// </remarks>
    public interface IPurchaseOrderWorkFlowStepService
    {
        /// <summary>Retrieves a specific purchaseorderworkflowstep by its primary key</summary>
        /// <param name="id">The primary key of the purchaseorderworkflowstep</param>
        /// <returns>The purchaseorderworkflowstep data</returns>
        PurchaseOrderWorkFlowStep GetById(Guid id);

        /// <summary>Retrieves a list of purchaseorderworkflowsteps based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of purchaseorderworkflowsteps</returns>
        List<PurchaseOrderWorkFlowStep> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new purchaseorderworkflowstep</summary>
        /// <param name="model">The purchaseorderworkflowstep data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(PurchaseOrderWorkFlowStep model);

        /// <summary>Updates a specific purchaseorderworkflowstep by its primary key</summary>
        /// <param name="id">The primary key of the purchaseorderworkflowstep</param>
        /// <param name="updatedEntity">The purchaseorderworkflowstep data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, PurchaseOrderWorkFlowStep updatedEntity);

        /// <summary>Updates a specific purchaseorderworkflowstep by its primary key</summary>
        /// <param name="id">The primary key of the purchaseorderworkflowstep</param>
        /// <param name="updatedEntity">The purchaseorderworkflowstep data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<PurchaseOrderWorkFlowStep> updatedEntity);

        /// <summary>Deletes a specific purchaseorderworkflowstep by its primary key</summary>
        /// <param name="id">The primary key of the purchaseorderworkflowstep</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The purchaseorderworkflowstepService responsible for managing purchaseorderworkflowstep related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting purchaseorderworkflowstep information.
    /// </remarks>
    public class PurchaseOrderWorkFlowStepService : IPurchaseOrderWorkFlowStepService
    {
        private inventoryContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the PurchaseOrderWorkFlowStep class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public PurchaseOrderWorkFlowStepService(inventoryContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific purchaseorderworkflowstep by its primary key</summary>
        /// <param name="id">The primary key of the purchaseorderworkflowstep</param>
        /// <returns>The purchaseorderworkflowstep data</returns>
        public PurchaseOrderWorkFlowStep GetById(Guid id)
        {
            var entityData = _dbContext.PurchaseOrderWorkFlowStep.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of purchaseorderworkflowsteps based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of purchaseorderworkflowsteps</returns>/// <exception cref="Exception"></exception>
        public List<PurchaseOrderWorkFlowStep> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetPurchaseOrderWorkFlowStep(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new purchaseorderworkflowstep</summary>
        /// <param name="model">The purchaseorderworkflowstep data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(PurchaseOrderWorkFlowStep model)
        {
            model.Id = CreatePurchaseOrderWorkFlowStep(model);
            return model.Id;
        }

        /// <summary>Updates a specific purchaseorderworkflowstep by its primary key</summary>
        /// <param name="id">The primary key of the purchaseorderworkflowstep</param>
        /// <param name="updatedEntity">The purchaseorderworkflowstep data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, PurchaseOrderWorkFlowStep updatedEntity)
        {
            UpdatePurchaseOrderWorkFlowStep(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific purchaseorderworkflowstep by its primary key</summary>
        /// <param name="id">The primary key of the purchaseorderworkflowstep</param>
        /// <param name="updatedEntity">The purchaseorderworkflowstep data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<PurchaseOrderWorkFlowStep> updatedEntity)
        {
            PatchPurchaseOrderWorkFlowStep(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific purchaseorderworkflowstep by its primary key</summary>
        /// <param name="id">The primary key of the purchaseorderworkflowstep</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeletePurchaseOrderWorkFlowStep(id);
            return true;
        }
        #region
        private List<PurchaseOrderWorkFlowStep> GetPurchaseOrderWorkFlowStep(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.PurchaseOrderWorkFlowStep.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<PurchaseOrderWorkFlowStep>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(PurchaseOrderWorkFlowStep), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<PurchaseOrderWorkFlowStep, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreatePurchaseOrderWorkFlowStep(PurchaseOrderWorkFlowStep model)
        {
            _dbContext.PurchaseOrderWorkFlowStep.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdatePurchaseOrderWorkFlowStep(Guid id, PurchaseOrderWorkFlowStep updatedEntity)
        {
            _dbContext.PurchaseOrderWorkFlowStep.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeletePurchaseOrderWorkFlowStep(Guid id)
        {
            var entityData = _dbContext.PurchaseOrderWorkFlowStep.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.PurchaseOrderWorkFlowStep.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchPurchaseOrderWorkFlowStep(Guid id, JsonPatchDocument<PurchaseOrderWorkFlowStep> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.PurchaseOrderWorkFlowStep.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.PurchaseOrderWorkFlowStep.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}