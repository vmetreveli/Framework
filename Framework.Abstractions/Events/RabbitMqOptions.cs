namespace Framework.Abstractions.Events;

/// <summary>
///     Represents the configuration options for connecting to a RabbitMQ broker.
/// </summary>
public class RabbitMqOptions
{
    /// <summary>
    ///     Gets or sets the hostname or IP address of the RabbitMQ server.
    /// </summary>
    public string? Host { get; init; }

    /// <summary>
    ///     Gets or sets the username used to authenticate with the RabbitMQ server.
    /// </summary>
    public string? UserName { get; init; }

    /// <summary>
    ///     Gets or sets the password used to authenticate with the RabbitMQ server.
    /// </summary>
    public string? Password { get; init; }

    /// <summary>
    ///     Gets or sets the name of the exchange to which messages will be published.
    /// </summary>
    public string? ExchangeName { get; init; }
}