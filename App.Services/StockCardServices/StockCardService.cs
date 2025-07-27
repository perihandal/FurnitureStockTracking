using App.Repositories;
using App.Repositories.Categories;
using App.Repositories.StockCards;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace App.Services.StockCardServices
{
    public class StockCardService(IStockCardRepository stockcardRepository, IUnitOfWork unitOfWork) : IStockCardService
    {
        //public async Task<ServiceResult<List<StockCardDto>>> GetTopPriceASync(int count)
        //{
        //    var stockcards = await stockcardRepository.GetTopPriceProductsAsync(count);

        //    var productsAsDto = stockcards.Select(p => new StockCardDto(p.Id, p.Name, p.Price)).ToList();

        //    return new ServiceResult<List<StockCardDto>>()
        //    {
        //        Data = productsAsDto
        //    };
        //}
        //public async Task<ServiceResult<StockCardDto?>> GetByIdAsync(int id)
        //{
        //    var stockcard = await stockcardRepository.GetByIdAsync(id);

        //    if (stockcard == null)
        //    {
        //        ServiceResult<StockCardDto>.Fail(errorMessage: "Product not found", HttpStatusCode.NotFound);
        //    }

        //    var productsAsDto = new StockCardDto(stockcard!.Id, stockcard.Name, stockcard.Price);

        //    return ServiceResult<StockCardDto>.Success((StockCardDto)productsAsDto)!;

        //}

        //public async Task<ServiceResult<CreateStockCardResponse>> CreateAsync(CreateStockCardRequest request)
        //{
        //    var stockcard = new StockCard()
        //    {
        //        Name = request.Name,
        //        Price = request.Price,
        //        StockQuantity = request.StockQuantity,
        //        Unit = request.Unit,
        //        Type = request.Type,
        //        CategoryId = request.CategoryId,
        //        SupplierId = request.SupplierId,

        //        // default değerler:
        //        IsActive = true,
        //        CreatedDate = DateTime.UtcNow
        //    };

        //    await stockcardRepository.AddAsync(stockcard);

        //    await unitOfWork.SaveChangesAsync();

        //    return ServiceResult<CreateStockCardResponse>.Success(new CreateStockCardResponse(stockcard.Id));
        //}

        //public async Task<ServiceResult> UpdateAsync(int id, UpdateStockCardRequest request)
        //{
        //    var product = await stockcardRepository.GetByIdAsync(id);

        //    if (product == null)
        //    {
        //        return ServiceResult.Fail("Product not found", HttpStatusCode.NotFound);
        //    }

        //    product.Name = request.Name;
        //    product.Type = request.Type;
        //    product.Unit = request.Unit;
        //    //product.Price = request.Price;
        //    //product.StockQuantity = request.StockQuantity;
        //    //product.IsActive = request.IsActive;
        //    product.CreatedDate = DateTime.UtcNow;

        //    stockcardRepository.Update(product);
        //    await unitOfWork.SaveChangesAsync();
        //    return ServiceResult.Success(HttpStatusCode.NoContent);


        //}

        //public async Task<ServiceResult<List<StockCardDto>>> GetAllList()
        //{
        //    var products = await stockcardRepository.GetAll().ToListAsync();

        //    var productAsDto = products.Select(p => new StockCardDto(p.Id, p.Name, p.Price)).ToList();

        //    return ServiceResult<List<StockCardDto>>.Success(productAsDto);

        //}
    }
}
