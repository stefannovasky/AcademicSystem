using Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Validators
{
    public class AttendanceValidator : AbstractValidator<Attendance>
    {
        public AttendanceValidator()
        {
            RuleFor(x => x.Value).NotNull();
            RuleFor(x => x.Date).NotNull();
            RuleFor(x => x.Date).Must(x => x.DayOfYear == DateTime.Now.DayOfYear && x.Year == DateTime.Now.Year);
        }
    }
}
