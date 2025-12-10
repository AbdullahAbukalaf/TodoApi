using TodoApi.Application.Dtos;
using TodoApi.Domain.Entities;

namespace TodoApi.Application.Abstractions
{
    public interface ITodoService
    {
        Task<(IEnumerable<TodoDto> Items, int total)> GetAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
        Task<TodoDto?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<TodoDto> CreateAsync(CreateTodoRequest request, CancellationToken ct = default);
        Task<TodoDto?> UpdateAsync(int id, UpdateTodoRequest request, CancellationToken ct = default);
        Task<bool> SoftDeleteAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<TodoDto>> GetDeletedAsync(CancellationToken ct = default);
    }
}
