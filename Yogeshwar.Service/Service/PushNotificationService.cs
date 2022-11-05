namespace Yogeshwar.Service.Service;

internal class PushNotificationService
{
    private readonly IConfiguration _configuration;

    public PushNotificationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> SendPushAsync(PushNotificationDto dto)
    {
        var client = new OneSignalClient(_configuration["Notification:Token"]);
        var opt = new NotificationCreateOptions
        {
            AppId = Guid.Parse(_configuration["Notification:AppId"]!),
            IncludePlayerIds = _configuration.GetSection("Notification:DeviceIds").Get<string[]>()
        };

        opt.Headings.Add(LanguageCodes.English, dto.Title);
        opt.Contents.Add(LanguageCodes.English, dto.Message);

        var result = await client.Notifications.CreateAsync(opt).ConfigureAwait(false);
        return result.Id;
    }
}