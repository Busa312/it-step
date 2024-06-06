using CarRent.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarRent.Data;

public class ApplicationDbInitializer
{
    public static async Task Seed(IServiceProvider serviceProvider)
    {
        using (var context = new ApplicationDbContext(
                   serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
        {
            context.Database.EnsureCreated();
            
            if (context.Users.Any())
            {
                return;
            }
            
            var passwordHasher = new PasswordHasher<User>();
            var user = new User
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "admin@admin.com",
                PhoneNumber = "1234567890",
                Password = passwordHasher.HashPassword(null, "password123")
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Add initial cars
            var car1 = new Car
            {
                Brand = "Toyota",
                Year = 2020,
                PriceDay = 50,
                Capacity = 5,
                Transmision = "Automatic",
                City = "New York",
                TankCapacity = 50,
                Pic1 = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSs-gCKq62J_ADfJHUc-kXitFab0gjYJPs9TQ&s",
                Pic2 = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQUrETbVjL35U4QF4LDdQ0r5xfekEX38MOuqw&s",
                Pic3 = "https://www.motortrend.com/uploads/sites/10/2020/05/2020-toyota-c-hr-le-suv-angular-front.png",
                OwnerId = user.Id,
                PhoneNumber = user.PhoneNumber
            };

            var car2 = new Car
            {
                Brand = "Honda",
                Year = 2019,
                PriceDay = 45,
                Capacity = 5,
                Transmision = "Manual",
                City = "Los Angeles",
                TankCapacity = 45,
                Pic1 = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQhsyQnKtajEuioQAwzIiL2aDlwD36cFVH5sA&s",
                Pic2 = "https://edgecast-img.yahoo.net/mysterio/api/23E4F75EB148D3FAFBFECB0C3BB21D7FBA0FAEBF3F8823DC031F5F5671989D17/autoblog/resizefill_w660_h372;quality_80;format_webp;cc_31536000;/https://s.aolcdn.com/commerce/autodata/images/USC90HOC024B121001.jpg",
                Pic3 = "https://hips.hearstapps.com/hmg-prod/images/2019-honda-civic-sedan-101-1537806283.jpg",
                OwnerId = user.Id,
                PhoneNumber = user.PhoneNumber
            };

            context.Cars.AddRange(car1, car2);
            await context.SaveChangesAsync();
        }
    }
}