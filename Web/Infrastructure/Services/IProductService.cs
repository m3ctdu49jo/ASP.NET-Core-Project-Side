using ShoppingMall.Web.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingMall.Web.Infrastructure.Services
{
    public interface IProductService
    {
        Task<ProductDTO?> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
        Task<IEnumerable<ProductDTO>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<ProductDTO>> SearchProductsAsync(string searchTerm);
        Task<ProductDTO> CreateProductAsync(ProductDTO productDto);
        Task<ProductDTO?> UpdateProductAsync(int id, ProductDTO productDto);
        Task<bool> DeleteProductAsync(int id);
        Task<IEnumerable<ProductDTO>> GetFeaturedProductsAsync(int count = 8);
        Task<IEnumerable<ProductDTO>> GetNewArrivalsAsync(int count = 8);
    }
} 