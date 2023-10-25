namespace HotelManagementSystem.Models
{
    public class AvailabilityResult
    {
        public int SubTotal { get; set; }
        public int ServiceCharge { get; set; }
        public bool IsAvailable { get; set; }
        public int SGST { get; set; }
        public int CGST { get; set; }
        public int TotalAmount { get; set; }
        public Dictionary<DateTime, bool> AvailabilityByDate { get; set; } = new Dictionary<DateTime, bool>();
    }
}
