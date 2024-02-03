using MyProject.Models.Dto;
using MyProject.Models.Model;

namespace MyProject.AppService.IService;

public interface IAuthenService
{
    Token Login(UserDto user);
    Task<bool> Register(UserDto userDto);
    Token RefreshToken(Token tokenApiModel);
}