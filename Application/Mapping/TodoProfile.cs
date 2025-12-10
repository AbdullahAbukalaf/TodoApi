using AutoMapper;
using TodoApi.Application.Dtos;
using TodoApi.Domain.Entities;

namespace TodoApi.Application.Mapping
{
    public class TodoProfile : Profile
    {
        public TodoProfile()
        {
            CreateMap<TodoItem, TodoDto>(); // entity -> API

            CreateMap<CreateTodoRequest, TodoItem>()               // API -> entity
                .ForMember(d => d.CreatedAt, o => o.MapFrom(_ => DateTime.UtcNow)); // stamp now

            CreateMap<UpdateTodoRequest, TodoItem>()               // optional: ignore nulls on patchy updates
                .ForAllMembers(o => o.Condition((src, dest, val) => val != null));
        }
    }

}
