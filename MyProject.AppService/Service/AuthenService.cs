using MyProject.AppService.IService;
using MyProject.Infrastrcture;
using MyProject.Models.Dto;
using MyProject.Models.Model;

namespace MyProject.AppService.Service;

public class AuthenService : IAuthenService
{
    private readonly IAuthenRepository _repository;

    public AuthenService(IAuthenRepository repository)
    {
        _repository = repository;
    }

    public Token Login(UserDto user)
    {
        return _repository.LoginAsync(user);
    }

    public async Task<bool> Register(UserDto userDto)
    {
        return await _repository.RegisterAsync(userDto);
    }

    public Token RefreshToken(Token tokenApiModel)
    {
        return _repository.Refresh(tokenApiModel);
    }
}