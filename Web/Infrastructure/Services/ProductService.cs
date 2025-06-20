using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using ShoppingMall.Web.DTOs;
using ShoppingMall.Web.Infrastructure.Repositories;
using ShoppingMall.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingMall.Web.Infrastructure.Services
{
    public class ProductService : IProductService, IService<Product>
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private const string AllProductsCacheKey = "AllProducts";
        private const string FeaturedProductsCacheKey = "FeaturedProducts";
        private const string NewArrivalsCacheKey = "NewArrivals";
        private readonly IGenericService<Product> _genericService;


        public ProductService(IRepository<Product> productRepository, IMapper mapper, IMemoryCache cache, IGenericService<Product> genericService)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _cache = cache;
            _genericService = genericService;
        }
        public IGenericService<Product> Generic => _genericService;

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            if (!_cache.TryGetValue(AllProductsCacheKey, out IEnumerable<ProductDTO> products))
            {
                var productEntities = await _productRepository.GetAllAsync();
                products = _mapper.Map<IEnumerable<ProductDTO>>(productEntities);
                _cache.Set(AllProductsCacheKey, products);
            }
            return products;
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _productRepository.FindAsync(p => p.CategoryID == categoryId);
            return _mapper.Map<IEnumerable<ProductDTO>>(products);;
        }

        public async Task<IEnumerable<ProductDTO>> SearchProductsAsync(string searchTerm)
        {
            var products = await _productRepository.FindAsync(p => 
                p.ProductName.Contains(searchTerm) || 
                (p.Description != null && p.Description.Contains(searchTerm)));
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<ProductDTO> CreateProductAsync(ProductDTO productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            product.CreatedDate = DateTime.Now;
            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();
            ClearCache();
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<ProductDTO?> UpdateProductAsync(int id, Product productDto)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
                return null;

            _mapper.Map(productDto, existingProduct);
            existingProduct.ModifiedDate = DateTime.Now;
            await _productRepository.UpdateAsync(existingProduct);
            await _productRepository.SaveChangesAsync();
            ClearCache();
            return _mapper.Map<ProductDTO>(existingProduct);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return false;

            await _productRepository.DeleteAsync(id);
            await _productRepository.SaveChangesAsync();
            ClearCache();
            return true;
        }

        public async Task<IEnumerable<ProductDTO>> GetFeaturedProductsAsync(int count = 8)
        {
            var cacheKey = $"{FeaturedProductsCacheKey}_{count}";
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<ProductDTO> products))
            {
                var productEntities = await _productRepository.FindAsync(p => !p.Discontinued);
                products = _mapper.Map<IEnumerable<ProductDTO>>(productEntities)
                    .OrderByDescending(p => p.UnitsInStock)
                    .Take(count);
                _cache.Set(cacheKey, products);
            }
            return products;
        }

        public async Task<IEnumerable<ProductDTO>> GetNewArrivalsAsync(int count = 8)
        {
            var cacheKey = $"{NewArrivalsCacheKey}_{count}";
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<ProductDTO> products))
            {
                var productEntities = await _productRepository.FindAsync(p => !p.Discontinued);
                products = _mapper.Map<IEnumerable<ProductDTO>>(productEntities)
                    .OrderByDescending(p => p.CreatedDate)
                    .Take(count);
                _cache.Set(cacheKey, products);
            }
            return products;
        }

        private void ClearCache()
        {
            _cache.Remove(AllProductsCacheKey);
            _cache.Remove(FeaturedProductsCacheKey);
            _cache.Remove(NewArrivalsCacheKey);
        }
    }
} 