using HotelManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Models
{
    public class Payment
    {
        public int Id { get; set; }
        [Required]
        public int BookingId { get; set; }
        public Booking Booking { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public int TransactionId { get; set; }
        [Required]
        public int ChargeId { get; set; }
    }
}