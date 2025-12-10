using FluentValidation;
using TodoApi.Application.Dtos;

namespace TodoApi.Application.Validation
{
    public class UpdateTodoRequestValidator : AbstractValidator<UpdateTodoRequest>
    {
        public UpdateTodoRequestValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.").MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");
            RuleFor(x => x.Description).MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters.");
        }
    }
}
