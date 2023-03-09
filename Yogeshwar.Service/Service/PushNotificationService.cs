namespace Yogeshwar.Service.Service;

[RegisterService(ServiceLifetime.Singleton, enableLazyLoading: false)]
internal class PushNotificationService
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="PushNotificationService"/> class.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    public PushNotificationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Sends the push asynchronous.
    /// </summary>
    /// <param name="dto">The dto.</param>
    /// <returns></returns>
    public async Task<string> SendPushNotificationAsync(PushNotificationDto dto)
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