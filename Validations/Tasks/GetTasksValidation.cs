using FluentValidation;
using TodoBack.QueryParameters;

namespace TodoBack.Validations.Tasks
{
    public class GetTasksValidation : AbstractValidator<GetPageQuery>
    {

        public GetTasksValidation()
        {
            RuleFor(p => p.page).GreaterThan(0);
            RuleFor(p => p.pageSize).GreaterThan(0);
        }
    }
}
