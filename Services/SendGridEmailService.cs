using SendGrid;
using SendGrid.Helpers.Mail;

namespace DLARS.Services
{

    public interface ISendGridEmailService
    {
        Task<bool> SendPendingRequestNotificationEmailAsync(string email, int count, string date, string userAccount, string statusMessage);
    }


    public class SendGridEmailService : ISendGridEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<SendGridEmailService> _logger;

        public SendGridEmailService(IConfiguration config, ILogger<SendGridEmailService> logger)
        {
            _config = config;
            _logger = logger;
        }


        public async Task<bool> SendPendingRequestNotificationEmailAsync(string toEmail, int count, string date, 
                                                     string userAccount, string statusMessage)
        {
            var client = new SendGridClient(_config["SendGrid:ApiKey"]);

            var from = new EmailAddress(_config["SendGrid:FromEmail"], _config["SendGrid:FromName"]);
            var to = new EmailAddress(toEmail);

            var msg = MailHelper.CreateSingleTemplateEmail(
                from,
                to,
                _config["SendGrid:TemplateId"], 
                new
                {
                    userAccount = userAccount,
                    count = count,
                    date = date,
                    statusMessage = statusMessage
                }
            );

            var response = await client.SendEmailAsync(msg);

            _logger.LogInformation("SendGrid status code: {StatusCode}", response.StatusCode);

            return response.IsSuccessStatusCode;
        }



    }
}

