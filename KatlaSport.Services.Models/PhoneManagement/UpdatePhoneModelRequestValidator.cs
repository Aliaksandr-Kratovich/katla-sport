
using FluentValidation;

namespace KatlaSport.Services.PhoneManagement
{
    public class UpdatePhoneModelRequestValidator : AbstractValidator<UpdatePhoneModelRequest>
    {
        public UpdatePhoneModelRequestValidator()
        {
            RuleFor(r => r.Code).Length(1, 20);
            RuleFor(r => r.Model).Length(1,20);
            RuleFor(r => r.PhoneId).GreaterThan(0);
        }
    }
}
