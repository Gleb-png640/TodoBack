using FluentValidation;
using TodoBack.Dtos.Tasks;

namespace TodoBack.Validations.Tasks
{
    public class UpdateUserTaskValidation : AbstractValidator<UpdateUserTaskDto>
    {

        public UpdateUserTaskValidation()
        {
            RuleFor(n => n.Name).Length(1, 25);
            RuleFor(n => n.Description).Length(0, 100);
        }
    }
}
