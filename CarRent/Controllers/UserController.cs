using System.Security.Claims;
using CarRent.Data;
using CarRent.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRent.Controllers;

[Authorize]
public class UserController : Controller
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Profile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        var user = await _context.Users
            .Include(u => u.FavoriteCars)
            .Include(u => u.RentedCars).ThenInclude(cr => cr.Car)
            .Include(u => u.MyCars)
            .FirstOrDefaultAsync(u => u.Id == int.Parse(userId));

        if (user == null)
        {
            return NotFound();
        }

        var viewModel = new UserProfileViewModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email,
            FavoriteCars = user.FavoriteCars,
            RentedCars = user.RentedCars,
            MyCars = user.MyCars
        };

        return View(viewModel);
    }
}