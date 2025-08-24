using App.Repositories.StockCards;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.StockCardServices
{
   
    public class CreateStockCardRequestValidatior : AbstractValidator<CreateStockCardRequestValidatior>
    {
        private readonly IStockCardRepository stockCardRepository;

        public CreateStockCardRequestValidatior(IStockCardRepository stockCardRepository)
        {
            stockCardRepository = stockCardRepository;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ürün İsmi Gereklidir")
                .Length(3, 10).WithMessage("Ürün İsmi 3-10 arasında olmalıdır.");
        }
        

    }
}
