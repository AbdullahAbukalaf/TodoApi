using TodoApi.Domain.Enums;

namespace TodoApi.Domain.Entities
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TodoStatus TodoStatus { get; set; } = TodoStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }




    }
}
