using Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Validators
{
    public class EvaluationValidator : AbstractValidator<Evaluation>
    {
        public EvaluationValidator()
        {
            RuleFor(x => x.Name).Length(1, 100);
            RuleFor(x => x.Date).Must(list => list.DayOfYear >= DateTime.Now.DayOfYear && list.Year >= DateTime.Now.Year)
           .WithMessage("Evaluation cannot be created on the past.");
            RuleFor(x => x.Value).LessThanOrEqualTo(10).GreaterThanOrEqualTo(0);
        }
    }
}
