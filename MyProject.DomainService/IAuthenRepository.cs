using MyProject.Models.Dto;
using MyProject.Models.Model;

namespace MyProject.Infrastrcture;

public interface IAuthenRepository
{
    Token LoginAsync(UserDto user);
    Task<bool> RegisterAsync(UserDto userDto);
    Token Refresh(Token tokenApiModel);
}