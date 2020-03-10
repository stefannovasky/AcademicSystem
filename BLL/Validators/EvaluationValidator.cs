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
        }
    }
}
