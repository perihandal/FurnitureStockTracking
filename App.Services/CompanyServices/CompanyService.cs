using App.Repositories.StockCards;
using App.Repositories;
using App.Services.StockCardServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Repositories.Companies;
using System.Net;

namespace App.Services.CompanyServices
{
    public class CompanyService(ICompanyRepository companyRepository, IUnitOfWork unitOfWork) : ICompanyService
    {
        public async Task<ServiceResult<CreateCompanyResponse>> CreateAsync(CreateCompanyRequest request)
        {
            var company = new Company()
            {
                Code = request.Code,
                Name = request.Name,
                TaxNumber = request.TaxNumber,
                Address = request.Address,
                Phone = request.Phone,
                UserId = request.UserId,
                IsActive = true,
            };

            await companyRepository.AddAsync(company);

            await unitOfWork.SaveChangesAsync();

            return ServiceResult<CreateCompanyResponse>.Success(new CreateCompanyResponse(company.Id));
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateCompanyRequest request)
        {
            var company = await companyRepository.GetByIdAsync(id);

            if (company == null)
            {
                return ServiceResult.Fail("Product not found", HttpStatusCode.NotFound);
            }
            company.Code = request.Code;
            company.Name = request.Name;
            company.Phone = request.Phone;
            company.IsActive = request.IsActive;
            company.TaxNumber = request.TaxNumber;
            company.Address = request.Address;
            company.UserId= request.UserId;

            companyRepository.Update(company);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);


        }

        public async Task<ServiceResult<List<CompanyDto>>> GetAllList()
        {
            var companies = await companyRepository.GetAllWithDetailsAsync();

            var companyAsDto = companies.Select(c => new CompanyDto(
                c.Code,
                c.Name,
                c.TaxNumber,
                c.Address,
                c.Phone,
                c.IsActive,
                c.User.FullName,
                c.Branches.Select(b => b.Name).ToList(),
                c.Warehouses.Select(w => w.Name).ToList()

            )).ToList();

            return ServiceResult<List<CompanyDto>>.Success(companyAsDto);
        }



    }
}
