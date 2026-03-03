using FluentValidation;
using TodoBack.Dtos.Users;

namespace TodoBack.Validations.Users {

    public class CreateUserValidation : AbstractValidator<CreateUserDto> {

        public CreateUserValidation() {
            RuleFor(u => u.UserName).Length(1, 25);
            RuleFor(u => u.Password).Length(6, 25);
            RuleFor(u => u.Email).EmailAddress();
        }
    }
}
