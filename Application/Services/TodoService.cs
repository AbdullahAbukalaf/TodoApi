using AutoMapper;
using Azure.Core;
using System.Drawing;
using TodoApi.Application.Abstractions;
using TodoApi.Application.Dtos;
using TodoApi.Domain.Entities;

namespace TodoApi.Application.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository; 
        private readonly IMapper _mapper;
        public TodoService(ITodoRepository todoRepository,IMapper mapper)
        {
            _todoRepository = todoRepository;
            _mapper = mapper;
        }
        public async Task<TodoDto> CreateAsync(CreateTodoRequest request, CancellationToken ct = default)
        {
            var entity = _mapper.Map<TodoItem>(request);
            await _todoRepository.AddAsync(entity, ct);
            await _todoRepository.SaveChangesAsync(ct);
            return _mapper.Map<TodoDto>(entity);
        }

        public async Task<IEnumerable<TodoDto>> GetDeletedAsync(CancellationToken ct = default)
        {
            var data = await _todoRepository.GetDeletedAsync(ct);
            var dtos = data.Select(e => _mapper.Map<TodoDto>(e)).ToList();
            return dtos;
        }

        public async Task<(IEnumerable<TodoDto> Items, int total)> GetAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        {
            pageNumber = Math.Max(pageNumber, 1);
            pageSize = Math.Clamp(pageSize, 1, 100);
            var total = await _todoRepository.CountAsync(search, ct);
            var data = await _todoRepository.GetPagedAsync(pageNumber, pageSize, search, ct);
            var dtos = data.Select(d => _mapper.Map<TodoDto>(d)).ToList();
            return (dtos, total);
        }

        public async Task<TodoDto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
           var item = await _todoRepository.GetByIdAsync(id, ct);
            return item is null ? null : _mapper.Map<TodoDto>(item);
        }

        public async Task<bool> SoftDeleteAsync(int id, CancellationToken ct = default)
        {
            var item = await _todoRepository.GetByIdAsync(id, ct);
            if (item is null) return false;
            await _todoRepository.SoftDeleteAsync(item, ct);
            await _todoRepository.SaveChangesAsync(ct);
            return true;
        }

        public async Task<TodoDto?> UpdateAsync(int id, UpdateTodoRequest request, CancellationToken ct = default)
        {
            var item = await _todoRepository.GetByIdAsync(id, ct);
            if (item is null) return null;


            if (request.Title is not null) item.Title = request.Title;
            if (request.Description is not null) item.Description = request.Description;
            if (request.TodoStatus.HasValue) item.TodoStatus = request.TodoStatus.Value;
            if (request.DueDate.HasValue) item.DueDate = request.DueDate.Value;


            await _todoRepository.UpdateAsync(item, ct);
            await _todoRepository.SaveChangesAsync(ct);
            return _mapper.Map<TodoDto>(item);
        }
    }
}
