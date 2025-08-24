using App.Repositories.StockCards;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace App.Services.StockCardServices
{
    public class CreateStockCardRequestValidator : AbstractValidator<CreateStockCardRequest>
    {
        private readonly IStockCardRepository stockCardRepository;

        public CreateStockCardRequestValidator(IStockCardRepository stockCardRepository)
        {
            this.stockCardRepository = stockCardRepository;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ürün ismi gereklidir.")
                .MinimumLength(3).WithMessage("Ürün ismi en az 3 karakter olmalıdır.")
                .MaximumLength(100).WithMessage("Ürün ismi en fazla 100 karakter olabilir.");
                //.MustAsync(async (request, name, ct) =>
                //    !await stockCardRepository
                //        .Where(sc => sc.CompanyId == request.CompanyId && sc.Name == name)
                //        .AnyAsync(ct))
                //.WithMessage("Aynı şirket içerisinde bu ürün ismi zaten mevcut.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Ürün kodu gereklidir.")
                .MinimumLength(2).WithMessage("Ürün kodu en az 2 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("Ürün kodu en fazla 50 karakter olabilir.")
                .Matches("^[A-Za-z0-9_-]+$").WithMessage("Ürün kodu yalnızca harf, rakam, alt çizgi ve tire içerebilir.");
                //.MustAsync(async (request, code, ct) =>
                //    !await stockCardRepository
                //        .Where(sc => sc.CompanyId == request.CompanyId && sc.Code == code)
                //        .AnyAsync(ct))
                //.WithMessage("Aynı şirket içerisinde bu ürün kodu zaten mevcut.");

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Geçersiz ürün tipi.");

            RuleFor(x => x.Unit)
                .NotEmpty().WithMessage("Birim gereklidir.")
                .MaximumLength(20).WithMessage("Birim en fazla 20 karakter olabilir.");

            RuleFor(x => x.Tax)
                .InclusiveBetween(0, 100).WithMessage("Vergi yüzdesi 0 ile 100 arasında olmalıdır.");

            When(x => x.CreateDefaultBarcode, () =>
            {
                RuleFor(x => x.DefaultBarcodeType)
                    .IsInEnum().WithMessage("Geçersiz varsayılan barkod tipi.");
            });
        }
    }
}