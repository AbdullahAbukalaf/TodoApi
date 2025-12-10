using System.ComponentModel.DataAnnotations;

namespace TodoApi.Application.Dtos
{
    public record CreateTodoRequest(string Title, string? Description, DateTime? DueDate);
}

