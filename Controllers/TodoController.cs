using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Application.Abstractions;
using TodoApi.Application.Dtos;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/todos")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;
        private readonly IValidator<CreateTodoRequest> _createValidator;
        private readonly IValidator<UpdateTodoRequest> _updateValidator;
        public TodoController(ITodoService todoService, IValidator<CreateTodoRequest> createValidator, IValidator<UpdateTodoRequest> updateValidator)
        {
            _todoService = todoService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null, CancellationToken ct = default)
        {
            var (items, total) = await _todoService.GetAsync(pageNumber, pageSize, search, ct);
            return Ok(new {pageNumber, pageSize, items, total });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cl = default)
        {
            var item = await _todoService.GetByIdAsync(id, cl);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTodoRequest request, CancellationToken ct = default)
        {
            var val = await _createValidator.ValidateAsync(request, ct); 
            //if (!val.IsValid) return ValidationProblem(val.ToDictionary());
            if (!val.IsValid) return ValidationProblem(ModelState);
            var dto = await _todoService.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id,[FromBody] UpdateTodoRequest request, CancellationToken ct = default)
        {
            var val = await _updateValidator.ValidateAsync(request, ct);
            if (!val.IsValid) return ValidationProblem(ModelState);
            var dto = await _todoService.UpdateAsync(id, request, ct);
            return dto is null ? NotFound() : Ok(dto);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ct = default)
        {
            var ok = await _todoService.SoftDeleteAsync(id, ct);
            return ok ? NoContent() : NotFound();
        }

        [HttpGet("deleted")]
        public async Task<IActionResult> GetDeletedItems(CancellationToken ct = default)
        {
            var items = await _todoService.GetDeletedAsync(ct);
            return items is null ? NotFound() : Ok(items);
        }
    }
}
