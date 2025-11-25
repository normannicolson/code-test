using Microsoft.EntityFrameworkCore;
using Reserve.Application.CommandHandlers;
using Reserve.Application.CommandHandlers.Data;
using Reserve.Infrastructure.Data.CommandHandlers;
using Reserve.Infrastructure.Data.CommandHandlers.Data;
using Reserve.Application.QueryHandlers;
using Reserve.Infrastructure.Data;
using Reserve.Infrastructure.Data.QueryHandlers;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using Reserve.Presentation.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks()
    .AddDbContextCheck<Context>();

builder.Services.AddOpenTelemetry().UseAzureMonitor();

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<IContext, Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ReserveDatabase")));

builder.Services.AddScoped<IHotelGetAllQueryHandler, HotelGetAllQueryHandler>();
builder.Services.AddScoped<IHotelSearchQueryHandler, HotelSearchQueryHandler>();
builder.Services.AddScoped<IFindAvailabilityQueryHandler, FindAvailabilityQueryHandler>();
builder.Services.AddScoped<IFindHotelAvailabilityQueryHandler, FindHotelAvailabilityQueryHandler>();
builder.Services.AddScoped<IGetHotelBookingsQueryHandler, GetHotelBookingsQueryHandler>();

builder.Services.AddScoped<IBookingGetQueryHandler, BookingGetQueryHandler>();

builder.Services.AddScoped<ICreateBookingCommandHandler, CreateBookingCommandHandler>();

builder.Services.AddScoped<IDatabaseSeedDataCommandHandler, DatabaseSeedDataCommandHandler>();
builder.Services.AddScoped<IDatabaseResetDataCommandHandler, DatabaseResetDataCommandHandler>();

var app = builder.Build();

app.MapHealthChecks("/health");

app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapGet("/", (ILogger<Program> logger) => {

    logger.LogInformation("Reserve API accessed at root endpoint.");

    return "Reserve API is running!";
});

app.UseHotelEndpoints();
app.UseRoomEndpoints();
app.UseBookingEndpoints();


if (builder.Configuration.GetValue<bool>("Features:AllowSeeding"))
{
    app.UseDataEndpoints();
}

app.Run();