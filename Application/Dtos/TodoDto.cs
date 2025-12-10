using TodoApi.Domain.Enums;

namespace TodoApi.Application.Dtos
{
    public record TodoDto(int Id, string Title, string? Description, TodoStatus TodoStatus, DateTime CreatedAt, DateTime? DueDate);
}
