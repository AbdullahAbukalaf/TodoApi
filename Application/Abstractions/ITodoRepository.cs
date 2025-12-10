using TodoApi.Application.Dtos;
using TodoApi.Domain.Entities;

namespace TodoApi.Application.Abstractions
{
    public interface ITodoRepository
    {
        // CRUD operations for TodoItem
        // plus pagination and soft delete
        // Methods should be asynchronous
        // Use CancellationToken for async methods
        // Include method to count total items for pagination
        // Soft delete should set IsDeleted to true and DeletedAt to current time
        // Pagination method should accept page number and page size
        // Count method should accept optional search parameter to filter items by title or description
        // Return types should be appropriate for each operation
        // Use Task<T> for async return types
        // Ensure methods are well-defined for future implementation
        // Follow C# conventions for naming and structure
        //  Keep the interface focused on TodoItem operations only
        // Avoid implementation details in the interface
        // Ensure methods are clear and concise
        // Use appropriate data types for parameters and return values
        // Consider potential exceptions that may arise in implementations
        // Document the interface methods for clarity
        // Ensure the interface is easy to understand and use
        // Keep the interface maintainable for future changes
        // Design the interface for scalability and extensibility
        // Ensure the interface adheres to SOLID principles
        // Focus on the core functionality required for managing TodoItems
        // Avoid unnecessary complexity in the interface design
        // Ensure the interface is aligned with the overall architecture of the application
        // Keep the interface consistent with other service interfaces in the application
        // Ensure the interface is testable and supports unit testing
        // Use Task for methods that do not return data
        // Use Task<T> for methods that return data

        Task<TodoItem?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<List<TodoItem>> GetPagedAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
        Task<int> CountAsync(string? search, CancellationToken ct = default);
        Task AddAsync(TodoItem todoItem, CancellationToken ct = default);
        Task SoftDeleteAsync(TodoItem todoItem, CancellationToken ct = default);
        Task UpdateAsync(TodoItem todoItem, CancellationToken ct = default);
        Task SaveChangesAsync(CancellationToken ct = default);
        Task<List<TodoItem>> GetDeletedAsync(CancellationToken ct = default);
    }
}
