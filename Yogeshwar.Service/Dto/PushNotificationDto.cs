namespace Yogeshwar.Service.Dto;

public record struct PushNotificationDto
{
    /// <summary>
    /// Gets the title.
    /// </summary>
    /// <value>The title.</value>
    public required string Title { get; init; }

    /// <summary>
    /// Gets the message.
    /// </summary>
    /// <value>The message.</value>
    public required string Message { get; init; }
}