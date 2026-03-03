using FluentValidation;
using TodoBack.Dtos.Users;

namespace TodoBack.Validations.Users {
    public class LoginUserValidation: AbstractValidator<LoginUserDto> {

        public LoginUserValidation() {
            RuleFor(u => u.Password).Length(6, 25);
            RuleFor(u => u.Email).EmailAddress();
        }
    }
}
