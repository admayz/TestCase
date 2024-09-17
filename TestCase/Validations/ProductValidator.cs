using FluentValidation;
using TestCase.Models;

namespace TestCase.Validations
{
    public class ProductValidator : AbstractValidator<Product>
    {
        /// <summary>
        /// `Product` nesnesi için doğrulama kurallarını tanımlar.
        /// Bu sınıf, ürünün adını, fiyatını ve açıklamasını doğrular.
        /// </summary>
        public ProductValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("Name alanı boş bırakılamaz.");

            RuleFor(x => x.Price)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("Price değeri 0'dan büyük olmalı");

            RuleFor(x => x.Description)
                .NotNull()
                .NotEmpty()
                .WithMessage("Description alanı boş bırakılamaz.");
        }
    }
}
