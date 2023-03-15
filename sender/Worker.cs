namespace sender;

using MassTransit;
using Microsoft.Extensions.Hosting;

public class Worker : BackgroundService
{
  readonly IBus _bus;

  public Worker(IBus bus)
  {
    _bus = bus;
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    while (!stoppingToken.IsCancellationRequested)
    {
      Console.WriteLine($"Publish: The time is {DateTimeOffset.Now}");

      await _bus.Publish(new Message { Content = $"The time is {DateTimeOffset.Now}", Type = "Simple" }, context => context.Headers.Set("Type", "Simple"), stoppingToken);
      await _bus.Publish(new Message { Content = $"The time is {DateTimeOffset.Now}", Type = "Complex" }, context => context.Headers.Set("Type", "Complex"), stoppingToken);

      await Task.Delay(1000, stoppingToken);
    }
  }
}
