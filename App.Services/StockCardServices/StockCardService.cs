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
        public async Task<ServiceResult<List<StockCardDto>>> GetTopPriceASync(int count)
        {
            var stockcards = await stockcardRepository.GetTopPriceProductsAsync(count);

            var productsAsDto = stockcards.Select(p => new StockCardDto(p.Id, p.Name)).ToList();

            return new ServiceResult<List<StockCardDto>>()
            {
                Data = productsAsDto
            };
        }
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

        public async Task<ServiceResult<CreateStockCardResponse>> CreateAsync(CreateStockCardRequest request)
        {
            var stockcard = new StockCard()
            {
                Name = request.Name,
                Code = request.Code,
                Tax = request.Tax,
                Type = request.Type,
                Unit = request.Unit,
                SubGroupId = request.SubGroupId,
                CompanyId = request.CompanyId,
                MainGroupId = request.MainGroupId,  
                BranchId = request.BranchId,
                CategoryId = request.CategoryId,

                // default değerler:
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            await stockcardRepository.AddAsync(stockcard);

            await unitOfWork.SaveChangesAsync();

            return ServiceResult<CreateStockCardResponse>.Success(new CreateStockCardResponse(stockcard.Id));
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateStockCardRequest request)
        {
            var stockcard = await stockcardRepository.GetByIdAsync(id);

            if (stockcard == null)
            {
                return ServiceResult.Fail("Product not found", HttpStatusCode.NotFound);
            }
            stockcard.Name = request.Name;
            stockcard.Code = request.Code;
            stockcard.Tax = request.Tax;
            stockcard.Type = request.Type;
            stockcard.Unit = request.Unit;
            stockcard.SubGroupId = request.SubGroupId;
            stockcard.CompanyId = request.CompanyId;
            stockcard.MainGroupId = request.MainGroupId;
            stockcard.BranchId = request.BranchId;
            stockcard.CategoryId = request.CategoryId;
            stockcard.IsActive = request.IsActive;
            stockcard.CreatedDate = DateTime.UtcNow;

            stockcardRepository.Update(stockcard);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);


        }
        public async Task<ServiceResult<List<StockCardDto>>> GetAllList()
        {
            var stockcards = await stockcardRepository.GetAllWithDetailsAsync();

            var stockcardAsDto = stockcards.Select(p => new StockCardDto(
               p.Id,
               p.Name
               //p.Code,
               //p.Type,
               //p.Unit,
               //p.Tax,
               //p.CreatedDate,
               //p.Company.Name,
               //p.Branch.Name,
               //p.MainGroup.Name,
               //p.SubGroup?.Name,
               //p.Category?.Name
            )).ToList();

            return ServiceResult<List<StockCardDto>>.Success(stockcardAsDto);
        }

    }
}
