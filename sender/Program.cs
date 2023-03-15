namespace sender;

using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
              x.AddDelayedMessageScheduler();
              x.UsingAmazonSqs((context, cfg) =>
                {
                  cfg.Host("eu-central-1", hostCfg =>
                  {
                    hostCfg.AccessKey("");
                    hostCfg.SecretKey("");
                  });
                  cfg.UseRawJsonSerializer();
                  cfg.UseDelayedMessageScheduler();

                  cfg.Message<Message>(c =>
                  {
                    c.SetEntityName("sender");
                  });

                  cfg.Publish<Message>(p =>
                  {
                    p.Durable = true;
                  });
                });
            });
            services.AddHostedService<Worker>();
          });
}
