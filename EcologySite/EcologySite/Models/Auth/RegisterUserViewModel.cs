namespace EcologySite.Models.Auth;

public class RegisterUserViewModel
{
    public string UserName { get; set; }
    public string Password { get; set; }
    
    public IFormFile AvatarImage { get; set; }
    //public int Age { get; set; }
}