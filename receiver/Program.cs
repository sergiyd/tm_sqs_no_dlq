namespace receiver;

using MassTransit;
using Microsoft.Extensions.Hosting;
using sender;


public class Program
{
  public static void Main(string[] args)
  {
    CreateHostBuilder(args).Build().Run();
  }

  public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
      .ConfigureServices((hostContext, services) =>
      {
        services.AddMassTransit(x =>
          {
            x.AddConsumer<MessageConsumer>();

            x.UsingAmazonSqs((context, cfg) =>
              {
                cfg.Host("eu-central-1", hostCfg =>
                  {
                    hostCfg.AccessKey("");
                    hostCfg.SecretKey("");
                  });

                cfg.UseRawJsonSerializer();
                cfg.Message<Message>(c => c.SetEntityName("sender"));

                cfg.ReceiveEndpoint("sender-receiver", configure =>
                  {
                    configure.ThrowOnSkippedMessages();
                    configure.RethrowFaultedMessages();

                    configure.Subscribe("sender", s =>
                      {
                        s.Durable = true;
                      });

                    configure.ThrowOnSkippedMessages();
                    configure.RethrowFaultedMessages();

                    configure.ConfigureConsumer<MessageConsumer>(context);

                    configure.ThrowOnSkippedMessages();
                    configure.RethrowFaultedMessages();
                  });
              });
          });
      });
}
