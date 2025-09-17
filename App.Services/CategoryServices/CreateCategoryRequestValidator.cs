using App.Repositories.Categories;
using FluentValidation;

namespace App.Services.CategoryServices
{
    public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
    {
        private readonly ICategoryRepository _categoryRepository;

        public CreateCategoryRequestValidator(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Kategori ismi gereklidir.")
                .MinimumLength(3).WithMessage("Kategori ismi en az 3 karakter olmalıdır.")
                .MaximumLength(100).WithMessage("Kategori ismi en fazla 100 karakter olabilir.")
                .Must((request, name) =>
                    !_categoryRepository
                        .Where(sc => sc.CompanyId == request.CompanyId && sc.Name == name)
                        .Any())
                .WithMessage("Aynı şirket içerisinde bu kategori ismi zaten mevcut.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Kategori kodu gereklidir.")
                .MinimumLength(2).WithMessage("Kategori kodu en az 2 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("Kategori kodu en fazla 50 karakter olabilir.")
                .Matches("^[A-Za-z0-9_-]+$").WithMessage("Kategori yalnızca harf, rakam, alt çizgi ve tire içerebilir.")
                .Must((request, code) =>
                    !_categoryRepository
                        .Where(sc => sc.CompanyId == request.CompanyId && sc.Code == code)
                        .Any())
                .WithMessage("Aynı şirket içerisinde bu kategori kodu zaten mevcut.");
        }
    }
}
