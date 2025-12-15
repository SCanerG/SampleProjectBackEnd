using FluentValidation;
using SampleProjectBackEnd.Application.DTOs.Requests;

namespace SampleProjectBackEnd.Application.Validators
{
    public class CategoryRequestDtoValidator : AbstractValidator<CategoryRequestDto>
    {
        public CategoryRequestDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Kategori adı boş olamaz.")
                .MinimumLength(2).WithMessage("Kategori adı en az 2 karakter olmalıdır.");
        }
    }
}
