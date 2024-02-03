using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyProject.AppService.IService;
using MyProject.Models.Dto;
using MyProject.Models.Model;
using Newtonsoft.Json;


namespace MyProject.Controllers;

public class AuthenController : Controller
{
    private readonly IAuthenService _authenService;

    public AuthenController(IAuthenService authenService)
    {
        _authenService = authenService;
    }

    [HttpPost("v1/Login")]
    [EnableCors]
    public  IActionResult CheckLogin([FromBody]UserDto user)
    {
        var token = _authenService.Login(user);
        if (token.AccessToken.IsNullOrEmpty()) return Unauthorized();
        else
        {
            HttpContext.Response.Headers.Add("Token", JsonConvert.SerializeObject(token) );
            return Ok();
        }
    }

    [HttpPost]
    [EnableCors]
    [Route("v1/Register")]
    public async Task<IActionResult> Register([FromBody]UserDto user)
    {
        var check = await _authenService.Register(user);
        if (check)
        {
            var token = _authenService.Login(user);
            HttpContext.Response.Headers.Add("Token", JsonConvert.SerializeObject(token));
            return Ok();
        }
        else return StatusCode(101);
        
    }

    [HttpPost("/v1/RefreshToken")]
    [EnableCors]
    public Token RefreshToken([FromBody]Token token)
    {
        var newToken = _authenService.RefreshToken(token);
        return newToken;
    }
}