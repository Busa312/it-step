namespace CarRent.Models;

public class UserProfileViewModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }

    public ICollection<Car> FavoriteCars { get; set; }
    public ICollection<CarRental> RentedCars { get; set; }
    public ICollection<Car> MyCars { get; set; }
}