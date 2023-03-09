namespace Yogeshwar.Web;

internal class NotificationBackgroundService : BackgroundService
{
    private const string Query =
        "SELECT (AC.Name + ' is pending from [Order #' + CAST(NF.OrderId as varchar) +' - ' + CS.FirstName + ' ' + CS.LastName + '] since ' + FORMAT(OD.OrderDate, 'dd-MMMM-yyyy')) AS [Message] FROM [Notification] NF JOIN ProductAccessories PA ON PA.Id = NF.ProductAccessoriesId JOIN Accessories AC ON AC.Id = PA.AccessoriesId JOIN [Order] OD ON OD.Id = NF.OrderId JOIN Customer CS ON CS.Id = OD.CustomerId WHERE NF.IsCompleted = 0";

    private readonly IConfiguration _configuration;
    private readonly PushNotificationService _pushNotificationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationBackgroundService"/> class.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <param name="pushNotificationService">The push notification service.</param>
    public NotificationBackgroundService(IConfiguration configuration, PushNotificationService pushNotificationService)
    {
        _configuration = configuration;
        _pushNotificationService = pushNotificationService;
    }

    /// <summary>
    /// This method is called when the <see cref="T:Microsoft.Extensions.Hosting.IHostedService" /> starts. The implementation should return a task that represents
    /// the lifetime of the long running operation(s) being performed.
    /// </summary>
    /// <param name="stoppingToken">Triggered when <see cref="M:Microsoft.Extensions.Hosting.IHostedService.StopAsync(System.Threading.CancellationToken)" /> is called.</param>
    /// <remarks>
    /// See <see href="https://docs.microsoft.com/dotnet/core/extensions/workers">Worker Services in .NET</see> for implementation guidelines.
    /// </remarks>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var sqlConnection = new SqlConnection(_configuration["ConnectionStrings:Default"]);
        await using var _ = sqlConnection.ConfigureAwait(false);

        while (!stoppingToken.IsCancellationRequested)
        {
            var minute = double.Parse(_configuration["Notification:Time"] ?? "60");

            await Task.Delay(TimeSpan.FromMinutes(minute), stoppingToken).ConfigureAwait(false);

            var messages = await sqlConnection.QueryAsync<string>(Query, commandType: CommandType.Text)
                .ConfigureAwait(false);

            var message = string.Join(Environment.NewLine, messages);

            var result = await _pushNotificationService.SendPushNotificationAsync(new PushNotificationDto
                { Title = "Pending Accessories", Message = message }).ConfigureAwait(false);

            Console.WriteLine(result);
        }
    }
}