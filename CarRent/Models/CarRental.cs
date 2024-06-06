using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CarRent.Models;

public class CarRental
{
    public int Id { get; set; }
    [Key]
    public DateTime RentDate { get; set; }
    public int CarId { get; set; }
    public int UserId { get; set; }
    public string City { get; set; }
    public int RentalDays { get; set; }
    public int PricePaid { get; set; }
    [ForeignKey("CarId")]
    public virtual Car Car { get; set; }
    [ForeignKey("UserId")]
    public virtual User User { get; set; }
}