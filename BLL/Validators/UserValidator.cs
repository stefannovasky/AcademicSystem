using Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
		{
			RuleFor(x => x.Name).Length(0, 100);
			RuleFor(x => x.Email).EmailAddress();
			RuleFor(x => x.Cpf).Length(14);
            RuleFor(x => x.Password).Length(8, 100);
            RuleFor(x => x.Rg).Length(8, 16);
            RuleFor(x => x.Street).Length(1, 120);
            RuleFor(x => x.Number).Length(0, 6);
            RuleFor(x => x.State).Length(3);
            RuleFor(x => x.City).Length(1, 120);
		}
	}
}
