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
			RuleFor(x => x.Cpf).Length(14).Must(x => IsCpf(x));
            RuleFor(x => x.Password).Length(8, 100);
            RuleFor(x => x.Rg).Length(8, 16);
            RuleFor(x => x.Street).Length(1, 120);
            RuleFor(x => x.Number).Length(0, 6);
            RuleFor(x => x.State).Length(2, 3);
            RuleFor(x => x.City).Length(1, 120);
		}
			public static bool IsCpf(string cpf)
			{
				if (cpf == null)
				{
					return false;
				}
				int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
				int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
				string tempCpf;
				string digito;
				int soma;
				int resto;
				cpf = cpf.Trim();
				cpf = cpf.Replace(".", "").Replace("-", "");
				if (cpf.Length != 11)
					return false;
				tempCpf = cpf.Substring(0, 9);
				soma = 0;

				for (int i = 0; i < 9; i++)
					soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
				resto = soma % 11;
				if (resto < 2)
					resto = 0;
				else
					resto = 11 - resto;
				digito = resto.ToString();
				tempCpf = tempCpf + digito;
				soma = 0;
				for (int i = 0; i < 10; i++)
					soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
				resto = soma % 11;
				if (resto < 2)
					resto = 0;
				else
					resto = 11 - resto;
				digito = digito + resto.ToString();
				return cpf.EndsWith(digito);
			}
		}
}
