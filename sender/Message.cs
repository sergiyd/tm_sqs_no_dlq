namespace sender;

using MassTransit;

public class Message : QueuePayload
{ }

[ExcludeFromTopology]
public abstract class QueuePayload
{
  public string Type { get; set; }
  public string Content { get; set; }
}
