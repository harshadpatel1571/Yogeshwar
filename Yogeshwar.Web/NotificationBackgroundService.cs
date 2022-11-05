namespace Yogeshwar.Web;

internal class NotificationBackgroundService : BackgroundService
{
    private const string Query =
        "SELECT (AC.Name + ' is pending from [Order #' + CAST(NF.OrderId as varchar) +' - ' + CS.FirstName + ' ' + CS.LastName + '] since ' + FORMAT(OD.OrderDate, 'dd-MMMM-yyyy')) AS [Message] FROM [Notification] NF JOIN ProductAccessories PA ON PA.Id = NF.ProductAccessoriesId JOIN Accessories AC ON AC.Id = PA.AccessoriesId JOIN [Order] OD ON OD.Id = NF.OrderId JOIN Customer CS ON CS.Id = OD.CustomerId WHERE NF.IsCompleted = 0";

    private readonly IConfiguration _configuration;
    private readonly PushNotificationService _pushNotificationService;

    public NotificationBackgroundService(IConfiguration configuration, PushNotificationService pushNotificationService)
    {
        _configuration = configuration;
        _pushNotificationService = pushNotificationService;
    }

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

            var result = await _pushNotificationService.SendPushAsync(new PushNotificationDto
                { Title = "Pending Accessories", Message = message }).ConfigureAwait(false);

            Console.WriteLine(result);
        }
    }
}