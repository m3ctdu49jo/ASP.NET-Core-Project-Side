using ShoppingMall.Web.DTOs;
using ShoppingMall.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingMall.Web.Infrastructure.Services
{
    public interface IProductService
    {
        Task<Product?> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
        Task<IEnumerable<ProductDTO>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<ProductDTO>> SearchProductsAsync(string searchTerm);
        Task<ProductDTO> CreateProductAsync(ProductDTO productDto);
        Task<ProductDTO?> UpdateProductAsync(int id, Product productDto);
        Task<bool> DeleteProductAsync(int id);
        Task<IEnumerable<ProductDTO>> GetFeaturedProductsAsync(int count = 8);
        Task<IEnumerable<ProductDTO>> GetNewArrivalsAsync(int count = 8);
    }
} 