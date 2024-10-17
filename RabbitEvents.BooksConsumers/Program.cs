using RabbitEvents.BooksConsumers.Extensions;

var builder = Host.CreateApplicationBuilder(args);

var config = builder.Configuration;

builder.Services.AddBootstrapperRegistration(config);

builder.Services.AddConsumers();

var host = builder.Build();

host.Run();
