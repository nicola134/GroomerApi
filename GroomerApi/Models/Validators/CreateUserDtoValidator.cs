using FluentValidation;
using GroomerApi.Entities;

namespace GroomerApi.Models.Validators
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator(GroomerDbContext dbContext)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
            
            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    var emailInUse = dbContext.Users.Any(u => u.Email == value);
                    if(emailInUse)
                    {
                        context.AddFailure("Email", "That email is taken");
                    }
                });
           
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MinimumLength(6);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MinimumLength(6);

            RuleFor(x => x.Password)
                .MinimumLength(6);

            RuleFor(x => x.ConfirmPassword)
                .Equal(e => e.Password);

            RuleFor(x => x.City)
                .NotEmpty();

            RuleFor(x => x.Street)
                .NotEmpty();

            RuleFor(x => x.PostalCode)
                .NotEmpty()
                .Length(6);
        }
    }
}
