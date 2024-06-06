using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CarRent.Models;

public class CarFilterViewModel
{
    public string? City { get; set; }
    public int? MinYear { get; set; }
    public int? MaxYear { get; set; }
    public int? Capacity { get; set; }
    public int? UserId { get; set; }
    public List<Car> FavCars { get; set; }
    public IEnumerable<Car>? Cars { get; set; }

}

