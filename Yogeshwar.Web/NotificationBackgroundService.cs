namespace Yogeshwar.Web;

internal class NotificationBackgroundService : BackgroundService
{
    private const string Query =
        "SELECT (AC.Name + ' is pending from [Order #' + CAST(NF.OrderId as varchar) +' - ' + CS.FirstName + ' ' + CS.LastName + '] since ' + FORMAT(OD.OrderDate, 'dd-MMMM-yyyy')) AS [Message] FROM [Notification] NF JOIN ProductAccessories PA ON PA.Id = NF.ProductAccessoriesId JOIN Accessories AC ON AC.Id = PA.AccessoriesId JOIN [Order] OD ON OD.Id = NF.OrderId JOIN Customer CS ON CS.Id = OD.CustomerId WHERE NF.IsCompleted = 0";

    private readonly IConfiguration _configuration;

    public NotificationBackgroundService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var sqlConnection = new SqlConnection(_configuration["ConnectionStrings:Default"]);

        while (!stoppingToken.IsCancellationRequested)
        {
            var minute = double.Parse(_configuration["Notification:Time"] ?? "60");

            await Task.Delay(TimeSpan.FromMinutes(minute), stoppingToken);

            var messages = await sqlConnection.QueryAsync<string>(Query, commandType: CommandType.Text);

            foreach (var message in messages)
            {
                Console.WriteLine(message);
            }
        }
    }
}