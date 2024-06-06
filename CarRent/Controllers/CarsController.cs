using System.Security.Claims;
using CarRent.Data;
using CarRent.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRent.Controllers;

public class CarsController : Controller
{
    private readonly ApplicationDbContext _context;

    public CarsController(ApplicationDbContext dbContext)
    {
        _context = dbContext;
    }
    
    [HttpGet]
    [Authorize]
    public IActionResult AddCar()
    {
        return View();
    }
    
    [HttpGet]
    public async Task<IActionResult> CarDetails(int id)
    {
        var car = await _context.Cars.FindAsync(id);
        if (car == null)
        {
            return NotFound();
        }

        return View(car);
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddCar(AddCarViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Get the logged-in user's ID from the claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            // Find the user in the database
            var user = await _context.Users.FindAsync(int.Parse(userId));
            if (user == null)
            {
                return NotFound();
            }

            // Set the owner and phone number of the car
            var car = new Car
            {   
                Brand = model.Brand,
                Year = model.Year,
                PriceDay = model.PriceDay,
                Capacity = model.Capacity,
                Transmision = model.Transmision,
                City = model.City,
                TankCapacity = model.TankCapacity,
                Pic1 = model.Pic1,
                Pic2 = model.Pic2,
                Pic3 = model.Pic3,
                OwnerId = user.Id,
                UsersWhoFavorited = new List<User>(),
                PhoneNumber = user.PhoneNumber
            };

            // Add the car to the database
            var newCar = _context.Cars.Add(car);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("CarDetails", new { id = car.Id });
        }

        return BadRequest(ModelState);
    }
    
    [HttpGet]
    public async Task<IActionResult> Index(string city, int? minYear, int? maxYear, int? capacity)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var list = _context.Users.Include(u => u.FavoriteCars).SingleOrDefault(u => u.Id == userId)?.FavoriteCars?.ToList();
        
        var cars = _context.Cars.AsQueryable();

        if (!string.IsNullOrEmpty(city))
        {
            cars = cars.Where(c => c.City == city);
        }

        if (minYear.HasValue)
        {
            cars = cars.Where(c => c.Year >= minYear);
        }

        if (maxYear.HasValue)
        {
            cars = cars.Where(c => c.Year <= maxYear);
        }

        if (capacity.HasValue)
        {
            cars = cars.Where(c => c.Capacity == capacity);
        }

        var carList = await cars.ToListAsync();

        var model = new CarFilterViewModel
        {
            City = city,
            MinYear = minYear,
            MaxYear = maxYear,
            Capacity = capacity,
            UserId = userId,
            FavCars = list,
            Cars = carList
        };
        

        return View(model);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddToFavorites(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (userId == null)
        {
            return Unauthorized();
        }

        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        var car = await _context.Cars.FindAsync(id);
        if (car == null)
        {
            return NotFound();
        }

        if (car.UsersWhoFavorited == null)
        {
            car.UsersWhoFavorited = new List<User>();
        }

        if (!car.UsersWhoFavorited.Any(u => u.Id == userId))
        {
            car.UsersWhoFavorited.Add(user);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveFromFavorites(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var user = await _context.Users.FindAsync(int.Parse(userId));
            if (user == null)
            {
                return NotFound();
            }

            var car = _context.Cars.Include(u => u.UsersWhoFavorited).FirstOrDefault(u => u.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            if (car.UsersWhoFavorited != null && car.UsersWhoFavorited.Any(u => u.Id == user.Id))
            {
                car.UsersWhoFavorited.Remove(user);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RentCar(int carId)
        {
            Console.WriteLine(carId);
            string nameIdentifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var car = await _context.Cars.FindAsync(carId);

            var user = await _context.Users
                .Where(u => u.Id == int.Parse(nameIdentifier))
                .FirstOrDefaultAsync();
            
            // Extract rental days from request body (adjust based on your request format)
            int rentalDays;
            if (Request.Form.TryGetValue("rentalDays", out var rentalDaysString))
            {
                if (int.TryParse(rentalDaysString, out rentalDays))
                {
                    // Valid rental days extracted
                }
                else
                {
                    // Handle invalid rental days format
                    return BadRequest("Invalid rental days format"); // Or a more informative message
                }
            }
            else
            {
                // Handle missing rental days information
                return BadRequest("Missing rental days information");
            }
            
            var rental = new CarRental
            {
                Car = car,
                User = user,
                City = car.City,
                RentDate = DateTime.UtcNow,
                RentalDays = rentalDays,
                PricePaid = rentalDays * car.PriceDay
            };
            
            
            _context.CarRentals.Add(rental);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    
}