namespace Application.Models.Common;
#pragma warning disable CS8618
public class Message<T>
{
    public Guid Guid { get; } = Guid.NewGuid();
    public T Data { get; set; }
}