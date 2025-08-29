using DLARS.Constants;
using DLARS.Enums;
using DLARS.Helpers;
using DLARS.Hubs;
using DLARS.Repositories;
using DLARS.Services;
using Microsoft.AspNetCore.SignalR;
using Quartz;
using System.Globalization;

namespace DLARS.QuartzJobs
{
    public class NotificationJob : IJob
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IHubContext<RequestHub> _hubContext;
        private readonly ISendGridEmailService _emailService;
        private readonly ILogger<NotificationJob> _logger;

        public NotificationJob(
            IRequestRepository requestRepository,
            IUserAccountRepository userAccountRepository,
            IHubContext<RequestHub> hubContext,
            ISendGridEmailService emailService,
            ILogger<NotificationJob> logger)
        {
            _requestRepository = requestRepository;
            _userAccountRepository = userAccountRepository;
            _hubContext = hubContext;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var statusesToCheck = new[]
            {
                RequestStatus.WaitingForFirstSecretaryApproval,
                RequestStatus.WaitingForSecondSecretaryApproval,
                RequestStatus.WaitingForChairpersonApproval,
                RequestStatus.WaitingForDirectorApproval
            };

            var now = TimeHelper.GetPhilippineTimeNow();
            var formattedDate = now.ToString("MMMM dd, yyyy", CultureInfo.InvariantCulture);

            var allUsers = await _userAccountRepository.GetAllUserWithRoleAsync();

            foreach (var status in statusesToCheck)
            {
                var count = await _requestRepository.GetPendingRequestsOlderThanAsync(status, TimeSpan.FromHours(8));
                _logger.LogInformation("Status: {Status}, Count: {Count}", status, count);

                var role = status switch
                {
                    RequestStatus.WaitingForFirstSecretaryApproval => "Secretary",
                    RequestStatus.WaitingForSecondSecretaryApproval => "Secretary",
                    RequestStatus.WaitingForChairpersonApproval => "Chairperson",
                    RequestStatus.WaitingForDirectorApproval => "Director",
                    _ => null
                };

                var statusMessage = status switch
                {
                    RequestStatus.WaitingForFirstSecretaryApproval => "waiting for initial secretary approval",
                    RequestStatus.WaitingForSecondSecretaryApproval => "waiting for second secretary approval",
                    RequestStatus.WaitingForChairpersonApproval => "waiting for chairperson approval",
                    RequestStatus.WaitingForDirectorApproval => "waiting for director approval",
                    _ => "pending"
                };

                var group = $"status-{Enum.GetName(typeof(RequestStatus), status)}";
             
                await _hubContext.Clients.Group(group).SendAsync(SignalREvents.ReceivePendingRequestCount, new { status = status.ToString(), count });
                await _hubContext.Clients.Group(group).SendAsync(SignalREvents.HangfireTriggered);

                if (count > 0 && role != null)
                {
                    var targetUsers = allUsers
                        .Where(u => u.Role == role && u.Status == "Active")
                        .ToList();

                    foreach (var user in targetUsers)
                    {
                        var result = await _emailService.SendPendingRequestNotificationEmailAsync(
                            user.Email,
                            count,
                            formattedDate,
                            user.FirstName ?? user.Email,
                            statusMessage
                        );

                        if (result)
                            _logger.LogInformation("Email sent to {Email} for {Role}", user.Email, role);
                        else
                            _logger.LogWarning("Failed to send email to {Email} for {Role}", user.Email, role);
                    }
                }
            }

            _logger.LogInformation("Notification job ran at {Now}", now);
        }
    }
}
