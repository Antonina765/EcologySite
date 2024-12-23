namespace MapMinimalApi.Models;

public class Location
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string UserId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
