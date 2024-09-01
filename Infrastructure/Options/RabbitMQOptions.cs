namespace Infrastructure.Options;
#pragma warning disable CS8618
public class RabbitMQOptions
{
    public string Host { get; set; }
    public string VirtualHost { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}