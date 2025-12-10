using Microsoft.EntityFrameworkCore;
using TodoApi.Application.Abstractions;
using TodoApi.Application.Dtos;
using TodoApi.Domain.Entities;
using TodoApi.Infrastructure.Persistence;

namespace TodoApi.Infrastructure.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly AppDbContext _db;
        public TodoRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task AddAsync(TodoItem todoItem, CancellationToken ct = default)
        => await _db.TodoItems.AddAsync(todoItem, ct).AsTask();

        public Task<int> CountAsync(string? search, CancellationToken ct = default)
        => _db.TodoItems.Where(t => string.IsNullOrEmpty(search) || t.Title.Contains(search) || (t.Description != null && t.Description.Contains(search))).CountAsync(ct);

        public async Task<List<TodoItem>> GetDeletedAsync(CancellationToken ct = default)
        => await _db.TodoItems.IgnoreQueryFilters().Where(t => t.IsDeleted).ToListAsync(ct);

        public Task<TodoItem?> GetByIdAsync(int id, CancellationToken ct = default)
        => _db.TodoItems.FirstOrDefaultAsync(t => t.Id == id, ct);

        public async Task<List<TodoItem>> GetPagedAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        {
            var q = _db.TodoItems.AsQueryable();
            if(!string.IsNullOrEmpty(search)) q = q.Where(t => t.Title.Contains(search) || (t.Description != null && t.Description.Contains(search)));
            return await q.OrderByDescending(t => t.CreatedAt).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        }

        public Task SaveChangesAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);

        public Task SoftDeleteAsync(TodoItem todoItem, CancellationToken ct = default)
        {
            todoItem.DeletedAt = DateTime.UtcNow;
            todoItem.IsDeleted = true;
            _db.TodoItems.Update(todoItem);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(TodoItem todoItem, CancellationToken ct = default)
        {
            _db.TodoItems.Update(todoItem);
            return Task.CompletedTask;
        }
    }
}
