namespace receiver;

using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using sender;

public class MessageConsumer :
    IConsumer<Message>
{
  readonly ILogger<MessageConsumer> _logger;

  public MessageConsumer(ILogger<MessageConsumer> logger)
  {
    _logger = logger;
  }

  public Task Consume(ConsumeContext<Message> context)
  {
    int.Parse(context.Message.Content);

    _logger.LogInformation($"Received: {context.Message.Content}");

    return Task.CompletedTask;
  }
}
