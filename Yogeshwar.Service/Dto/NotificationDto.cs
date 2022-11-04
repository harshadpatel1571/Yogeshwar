namespace Yogeshwar.Service.Dto;

public class NotificationDto
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string ProductName { get; set; }

    public int OrderId { get; set; }

    public int ProductAccessoriesId { get; set; }

    public string ProductAccessoriesName { get; set; }

    public bool IsCompleted { get; set; }

    public string StrIsCompleted { get; set; }
}
