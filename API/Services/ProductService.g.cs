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
    /// The productService responsible for managing product related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting product information.
    /// </remarks>
    public interface IProductService
    {
        /// <summary>Retrieves a specific product by its primary key</summary>
        /// <param name="id">The primary key of the product</param>
        /// <returns>The product data</returns>
        Product GetById(Guid id);

        /// <summary>Retrieves a list of products based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of products</returns>
        List<Product> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new product</summary>
        /// <param name="model">The product data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(Product model);

        /// <summary>Updates a specific product by its primary key</summary>
        /// <param name="id">The primary key of the product</param>
        /// <param name="updatedEntity">The product data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, Product updatedEntity);

        /// <summary>Updates a specific product by its primary key</summary>
        /// <param name="id">The primary key of the product</param>
        /// <param name="updatedEntity">The product data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<Product> updatedEntity);

        /// <summary>Deletes a specific product by its primary key</summary>
        /// <param name="id">The primary key of the product</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The productService responsible for managing product related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting product information.
    /// </remarks>
    public class ProductService : IProductService
    {
        private inventoryContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the Product class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public ProductService(inventoryContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific product by its primary key</summary>
        /// <param name="id">The primary key of the product</param>
        /// <returns>The product data</returns>
        public Product GetById(Guid id)
        {
            var entityData = _dbContext.Product.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of products based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of products</returns>/// <exception cref="Exception"></exception>
        public List<Product> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetProduct(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new product</summary>
        /// <param name="model">The product data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(Product model)
        {
            model.Id = CreateProduct(model);
            return model.Id;
        }

        /// <summary>Updates a specific product by its primary key</summary>
        /// <param name="id">The primary key of the product</param>
        /// <param name="updatedEntity">The product data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, Product updatedEntity)
        {
            UpdateProduct(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific product by its primary key</summary>
        /// <param name="id">The primary key of the product</param>
        /// <param name="updatedEntity">The product data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<Product> updatedEntity)
        {
            PatchProduct(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific product by its primary key</summary>
        /// <param name="id">The primary key of the product</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeleteProduct(id);
            return true;
        }
        #region
        private List<Product> GetProduct(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.Product.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<Product>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(Product), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<Product, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreateProduct(Product model)
        {
            _dbContext.Product.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdateProduct(Guid id, Product updatedEntity)
        {
            _dbContext.Product.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeleteProduct(Guid id)
        {
            var entityData = _dbContext.Product.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.Product.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchProduct(Guid id, JsonPatchDocument<Product> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.Product.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.Product.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}