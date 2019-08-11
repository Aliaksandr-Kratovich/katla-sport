using FluentValidation;

namespace KatlaSport.Services.PhoneManagement
{
    public class UpdatePhoneRequestValidator : AbstractValidator<UpdatePhoneRequest>
    {
        public UpdatePhoneRequestValidator()
        {
            RuleFor(r => r.Country).Length(1, 20);
            RuleFor(r => r.Mark).Length(1,20);
        }
    }
}
