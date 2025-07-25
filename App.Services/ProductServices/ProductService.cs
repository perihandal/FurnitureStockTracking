using App.Repositories;
using App.Repositories.Categories;
using App.Repositories.Products;
using App.Repositories.Suppliers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace App.Services.ProductServices
{
    public class ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork):IProductService
    {
        public async Task<ServiceResult<List<ProductDto>>> GetTopPriceASync(int count)
        {
            var products = await productRepository.GetTopPriceProductsAsync(count);

            var productsAsDto = products.Select (p => new ProductDto(p.Id, p.Name, p.Price)).ToList();

            return new ServiceResult<List<ProductDto>>()
            {
                Data = productsAsDto
            };
        }
        public async Task<ServiceResult<ProductDto?>> GetByIdAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);

            if (product == null)
            {
                ServiceResult<ProductDto>.Fail(errorMessage: "Product not found", HttpStatusCode.NotFound);
            }

            var productsAsDto = new ProductDto (product!.Id, product.Name, product.Price);

            return ServiceResult<ProductDto>.Success(productsAsDto)!;

        }

        public async Task<ServiceResult<CreateProductResponse>> CreateAsync(CreateProductRequest request)
        {
            var product = new Product()
            {
                Name = request.Name,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                Unit = request.Unit,
                Type = request.Type,
                CategoryId = request.CategoryId,
                SupplierId = request.SupplierId,

                // default değerler:
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            }; 

            await productRepository.AddAsync(product);

            await unitOfWork.SaveChangesAsync();

            return ServiceResult<CreateProductResponse>.Success(new CreateProductResponse(product.Id));
        }

        public async Task<ServiceResult>UpdateAsync(int id, UpdateProductRequest request)
        {
            var product= await productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return ServiceResult.Fail("Product not found", HttpStatusCode.NotFound);
            }

            product.Name = request.Name;
            product.Type = request.Type;
            product.Unit = request.Unit;
            product.Price = request.Price;
            product.StockQuantity = request.StockQuantity;
            product.IsActive = request.IsActive;
            product.CreatedDate = DateTime.UtcNow;

            productRepository.Update(product);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);


        }

        public async Task<ServiceResult<List<ProductDto>>> GetAllList()
        {
           var products = await productRepository.GetAll().ToListAsync();

            var productAsDto=products.Select(p=>new ProductDto(p.Id,p.Name,p.Price)).ToList();
            
            return ServiceResult<List<ProductDto>>.Success(productAsDto);

        }
    }
}
