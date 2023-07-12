using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace eShopSolution.ViewModels.System.User
{
	public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
	{
		public RegisterRequestValidator()
		{
			RuleFor(a => a.FirstName).NotEmpty().WithMessage("First name is required")
				.MaximumLength(200).WithMessage("First name is cannot over 200 characters");

			RuleFor(a => a.LastName).NotEmpty().WithMessage("Last name is required").MaximumLength(200)
				.WithMessage("Last name is cannot over 200 characters");

			RuleFor(a => a.DateOfBirth).GreaterThan(DateTime.Now.AddYears(-100))
				.WithMessage("Birthday cannot  greater than 100 years");

			RuleFor(a => a.Email).NotEmpty().WithMessage("Email is required")
				.Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").WithMessage("Email format not match");

			RuleFor(a => a.PhoneNumber).NotEmpty().WithMessage("Phone number is required");

			RuleFor(a => a.UserName).NotEmpty().WithMessage("User name is required");
			RuleFor(a => a.Password).NotEmpty().WithMessage("User name is required").MinimumLength(6)
				.WithMessage("Password is at least 6 characters");

			RuleFor(a => a).Custom((request, context) =>
			{
				if (request.Password != request.ComfirmPassword)
				{
					context.AddFailure("Confirm password is not match");
				}
			});
		}
	}
}
