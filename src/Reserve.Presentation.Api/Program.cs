using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reserve.Application.CommandHandlers.Data;
using Reserve.Application.Commands.Data;
using Reserve.Application.Queries;
using Reserve.Application.QueryHandlers;
using Reserve.Infrastructure.Data;
using Reserve.Infrastructure.Data.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<IContext, Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ReserveDatabase")));

builder.Services.AddScoped<IFindAvailabilityQueryHandler, FindAvailabilityQueryHandler>();
builder.Services.AddScoped<IBookingGetQueryHandler, BookingGetQueryHandler>();

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

app.MapGet("/hotels", () => {

    return Results.Ok("List of hotels");
});

//Find a hotel based on its name.
app.MapGet("/hotels/search", (string name) => {

    return Results.Ok($"Find a hotel based on its name {name}");
});

// Find available rooms between two dates for a given number of people.
app.MapGet("/rooms/search", async (
    [FromQuery]DateTime from,
    [FromQuery]DateTime to,
    [FromQuery]int numberOfPeople,
    [FromServices] IFindAvailabilityQueryHandler handler,
    CancellationToken cancellationToken) => {

    var query = new FindAvailabilityQuery(from, to);

    var dto = await handler.Handle(query, cancellationToken);

    return TypedResults.Ok(dto);
});

// Book a room.
app.MapPost("/bookings", () => {

    //Room Id
    
    //Slots Ids 

    return Results.Ok("Book a room.");
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
    app.MapGet("/data", async (
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