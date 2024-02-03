namespace MyProject.Models.Dto;

public class UserDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public int? Role { get; set; }
    public string? Avatar { get; set; }
}