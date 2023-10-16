namespace HotelManagementSystem.ServiceContract
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }

}
