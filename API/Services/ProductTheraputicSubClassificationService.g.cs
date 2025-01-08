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
    /// The producttheraputicsubclassificationService responsible for managing producttheraputicsubclassification related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting producttheraputicsubclassification information.
    /// </remarks>
    public interface IProductTheraputicSubClassificationService
    {
        /// <summary>Retrieves a specific producttheraputicsubclassification by its primary key</summary>
        /// <param name="id">The primary key of the producttheraputicsubclassification</param>
        /// <returns>The producttheraputicsubclassification data</returns>
        ProductTheraputicSubClassification GetById(Guid id);

        /// <summary>Retrieves a list of producttheraputicsubclassifications based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of producttheraputicsubclassifications</returns>
        List<ProductTheraputicSubClassification> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new producttheraputicsubclassification</summary>
        /// <param name="model">The producttheraputicsubclassification data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(ProductTheraputicSubClassification model);

        /// <summary>Updates a specific producttheraputicsubclassification by its primary key</summary>
        /// <param name="id">The primary key of the producttheraputicsubclassification</param>
        /// <param name="updatedEntity">The producttheraputicsubclassification data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, ProductTheraputicSubClassification updatedEntity);

        /// <summary>Updates a specific producttheraputicsubclassification by its primary key</summary>
        /// <param name="id">The primary key of the producttheraputicsubclassification</param>
        /// <param name="updatedEntity">The producttheraputicsubclassification data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<ProductTheraputicSubClassification> updatedEntity);

        /// <summary>Deletes a specific producttheraputicsubclassification by its primary key</summary>
        /// <param name="id">The primary key of the producttheraputicsubclassification</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The producttheraputicsubclassificationService responsible for managing producttheraputicsubclassification related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting producttheraputicsubclassification information.
    /// </remarks>
    public class ProductTheraputicSubClassificationService : IProductTheraputicSubClassificationService
    {
        private inventoryContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the ProductTheraputicSubClassification class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public ProductTheraputicSubClassificationService(inventoryContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific producttheraputicsubclassification by its primary key</summary>
        /// <param name="id">The primary key of the producttheraputicsubclassification</param>
        /// <returns>The producttheraputicsubclassification data</returns>
        public ProductTheraputicSubClassification GetById(Guid id)
        {
            var entityData = _dbContext.ProductTheraputicSubClassification.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of producttheraputicsubclassifications based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of producttheraputicsubclassifications</returns>/// <exception cref="Exception"></exception>
        public List<ProductTheraputicSubClassification> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetProductTheraputicSubClassification(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new producttheraputicsubclassification</summary>
        /// <param name="model">The producttheraputicsubclassification data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(ProductTheraputicSubClassification model)
        {
            model.Id = CreateProductTheraputicSubClassification(model);
            return model.Id;
        }

        /// <summary>Updates a specific producttheraputicsubclassification by its primary key</summary>
        /// <param name="id">The primary key of the producttheraputicsubclassification</param>
        /// <param name="updatedEntity">The producttheraputicsubclassification data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, ProductTheraputicSubClassification updatedEntity)
        {
            UpdateProductTheraputicSubClassification(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific producttheraputicsubclassification by its primary key</summary>
        /// <param name="id">The primary key of the producttheraputicsubclassification</param>
        /// <param name="updatedEntity">The producttheraputicsubclassification data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<ProductTheraputicSubClassification> updatedEntity)
        {
            PatchProductTheraputicSubClassification(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific producttheraputicsubclassification by its primary key</summary>
        /// <param name="id">The primary key of the producttheraputicsubclassification</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteProductTheraputicSubClassification(id);
            return true;
        }
        #region
        private List<ProductTheraputicSubClassification> GetProductTheraputicSubClassification(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.ProductTheraputicSubClassification.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<ProductTheraputicSubClassification>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(ProductTheraputicSubClassification), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<ProductTheraputicSubClassification, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateProductTheraputicSubClassification(ProductTheraputicSubClassification model)
        {
            _dbContext.ProductTheraputicSubClassification.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateProductTheraputicSubClassification(Guid id, ProductTheraputicSubClassification updatedEntity)
        {
            _dbContext.ProductTheraputicSubClassification.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteProductTheraputicSubClassification(Guid id)
        {
            var entityData = _dbContext.ProductTheraputicSubClassification.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.ProductTheraputicSubClassification.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchProductTheraputicSubClassification(Guid id, JsonPatchDocument<ProductTheraputicSubClassification> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.ProductTheraputicSubClassification.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.ProductTheraputicSubClassification.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}