using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reserve.Application.CommandHandlers;
using Reserve.Application.CommandHandlers.Data;
using Reserve.Application.Commands;
using Reserve.Application.Commands.Data;
using Reserve.Application.Queries;
using Reserve.Infrastructure.Data.CommandHandlers;
using Reserve.Infrastructure.Data.CommandHandlers.Data;
using Reserve.Application.QueryHandlers;
using Reserve.Infrastructure.Data;
using Reserve.Infrastructure.Data.QueryHandlers;
using Reserve.Presentation.Api.Models;
using Reserve.Application.Results;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Reserve API is running!");

app.MapGet("/hotels", async (
    [FromServices] IHotelGetAllQueryHandler handler,
    CancellationToken cancellationToken) => {

    var query = new HotelGetAllQuery();
    var dto = await handler.Handle(query, cancellationToken);

    return TypedResults.Ok(dto);
});

app.MapGet("/hotels/search", async (
    [FromQuery] string name,
    [FromServices] IHotelSearchQueryHandler handler,
    CancellationToken cancellationToken) => {

    var query = new HotelSearchQuery(name);
    var dto = await handler.Handle(query, cancellationToken);

    return TypedResults.Ok(dto);
});

app.MapGet("/hotels/{hotelId}/rooms/availability-search", async (
    [FromRoute] Guid hotelId,
    [FromQuery] DateTime from,
    [FromQuery] DateTime to,
    [FromQuery] int numberOfPeople,
    [FromServices] IFindHotelAvailabilityQueryHandler handler,
    CancellationToken cancellationToken) => {

    var query = new FindHotelAvailabilityQuery(hotelId, from, to, numberOfPeople);
    var dto = await handler.Handle(query, cancellationToken);

    return TypedResults.Ok(dto);
});

//List hotel bookings
app.MapGet("/hotels/{hotelId}/bookings", async (
    [FromRoute] Guid hotelId,
    [FromServices] IGetHotelBookingsQueryHandler handler,
    CancellationToken cancellationToken) => {

    var query = new GetHotelBookingsQuery(hotelId);
    var dto = await handler.Handle(query, cancellationToken);

    return TypedResults.Ok(dto);
});

// Book a room.
app.MapPost("/hotels/{hotelId}/bookings", async Task<Results<Created<object>, BadRequest<object>>> (
    [FromRoute] Guid hotelId,
    [FromBody] CreateBookingRequest request,
    [FromServices] ICreateBookingCommandHandler handler,
    CancellationToken cancellationToken) => {

    var command = new CreateBookingCommand(request.Name, request.RoomId, request.From, request.To);

    var result = await handler.Handle(command, cancellationToken);

    if (result.IsFailure)
    {
        var error = (ErrorResult<Guid>)result;
        return TypedResults.BadRequest<object>(new { error.Code, error.Message });
    }

    var success = (SuccessResult<Guid>)result;
    return TypedResults.Created($"/bookings/{success.Value}", (object)success.Value);
});

// Find available hotel rooms between two dates for a given number of people.
app.MapGet("/rooms/availability-search", async (
    [FromQuery] DateTime from,
    [FromQuery] DateTime to,
    [FromQuery] int numberOfPeople,
    [FromServices] IFindAvailabilityQueryHandler handler,
    CancellationToken cancellationToken) => {

    var query = new FindAvailabilityQuery(from, to, numberOfPeople);

    var dto = await handler.Handle(query, cancellationToken);

    return TypedResults.Ok(dto);
});

//Find booking details based on a booking reference.
app.MapGet("/bookings/{id}", async (
    [FromRoute(Name = "id")] Guid id,
    [FromServices] IBookingGetQueryHandler handler,
    CancellationToken cancellationToken) => {
    
    var query = new BookingGetQuery(id);

    var dto = await handler.Handle(query, cancellationToken);

    return TypedResults.Ok(dto);
});

if (builder.Configuration.GetValue<bool>("Features:AllowSeeding"))
{
    app.MapGet("/data/info", async (
        CancellationToken cancellationToken) => {  
              
        return TypedResults.Ok("Seeding is enabled.");
    });

    app.MapPost("/data/seed", async (
        [FromServices] IDatabaseSeedDataCommandHandler handler,
        CancellationToken cancellationToken) => {

        var command = new DatabaseSeedDataCommand();

        var result = await handler.Handle(command, cancellationToken);

        return (result == true)
            ? TypedResults.StatusCode(200)
            : TypedResults.StatusCode(500);
    });

    app.MapPost("/data/reset", async (
        [FromServices] IDatabaseResetDataCommandHandler handler,
        CancellationToken cancellationToken) => {

        var command = new DatabaseResetDataCommand();

        var result = await handler.Handle(command, cancellationToken);

        return (result == true)
            ? TypedResults.StatusCode(200)
            : TypedResults.StatusCode(500);
    }); 
}

app.Run();