using TodoApi.Domain.Enums;

namespace TodoApi.Application.Dtos
{
    public class UpdateTodoRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public TodoStatus? TodoStatus { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
