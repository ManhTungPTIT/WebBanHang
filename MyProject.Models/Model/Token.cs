using System.Runtime.InteropServices.JavaScript;

namespace MyProject.Models.Model;

public class Token
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public TimeSpan Expried { get; set; }
}