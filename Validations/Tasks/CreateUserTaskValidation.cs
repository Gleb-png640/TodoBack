using FluentValidation;
using TodoBack.Dtos.Tasks;

namespace TodoBack.Validations.Tasks
{
    public class CreateUserTaskValidation : AbstractValidator<CreateUserTaskDto>
    {

        public CreateUserTaskValidation()
        {
            RuleFor(n => n.Name).Length(1, 25);
            RuleFor(n => n.Description).Length(0, 100);
        }
    }
}
