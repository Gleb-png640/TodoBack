using FluentValidation;
using TodoBack.QueryParameters;

namespace TodoBack.Validations.Tasks
{
    public class GetPageQueryValidation : AbstractValidator<GetPageQuery>
    {

        public GetPageQueryValidation()
        {
            RuleFor(p => p.page).GreaterThan(0);
            RuleFor(p => p.pageSize).GreaterThan(0);
        }
    }
}
