using System.Threading.Tasks;

namespace WebApplication9.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
