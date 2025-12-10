using FluentValidation;
using TodoApi.Application.Dtos;

namespace TodoApi.Application.Validation
{
    public class CreateTodoRequestValidator : AbstractValidator<CreateTodoRequest>
    {
        public CreateTodoRequestValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.DueDate)
                .Must(d => d == null || d.Value > DateTime.UtcNow)
                .WithMessage("Due date must be in the future.");
        }
    }

}
