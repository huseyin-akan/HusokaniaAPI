namespace Application.Common.Abstractions.MessageQue;

public interface IMassTransitService
{
    Task Publish<T>(T message) where T : class;
    Task Send<T>(T message) where T : class;
}