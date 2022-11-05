namespace Yogeshwar.Service.Dto;

public record struct PushNotificationDto
{
    public required string Title { get; init; }

    public required string Message { get; init; }
}