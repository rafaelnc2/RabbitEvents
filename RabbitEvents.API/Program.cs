using RabbitEvents.API.Extensions;
using RabbitEvents.API.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new GlobalExceptionFilter());
});
builder.Services.AddScoped(typeof(ValidatorFilter<,>));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(cfg =>
{
    cfg.EnableAnnotations();
});

builder.Services.AddScoped<GlobalExceptionFilter>();

builder.Services.AddBootstrapperRegister(builder.Configuration);

builder.Services.AddHostedServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
