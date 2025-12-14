#nullable disable
using FluentValidation;
using FluentWebApplication.Models;

namespace FluentWebApplication.Validators;


/// <summary>
/// Provides validation rules for the <see cref="Models.Person"/> model.
/// </summary>
/// <remarks>
/// This validator ensures that the <c>FirstName</c>, <c>LastName</c>, and <c>EmailAddress</c> properties
/// of the <see cref="Models.Person"/> model are not empty. Additionally, it validates
/// that the <c>EmailAddress</c> property contains a valid email address format.
/// </remarks>
public class PersonValidator : AbstractValidator<Person>
{
    public PersonValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.EmailAddress).NotEmpty().EmailAddress();
    }
}