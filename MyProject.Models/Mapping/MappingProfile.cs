using AutoMapper;
using MyProject.Models.Dto;
using MyProject.Models.Model;

namespace MyProject.Models.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();
    }
}