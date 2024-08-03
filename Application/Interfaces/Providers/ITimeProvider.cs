namespace Application.Interfaces.Providers;

public interface ITimeProvider
{ 
    DateTime Now { get; }
}