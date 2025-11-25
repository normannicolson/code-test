using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Reserve.Application.CommandHandlers;
using Reserve.Application.Commands;
using Reserve.Application.Dtos;
using Reserve.Application.Queries;
using Reserve.Application.QueryHandlers;
using Reserve.Application.Results;
using Reserve.Presentation.Api.Models;

namespace Reserve.Presentation.Api.Endpoints;

public static class HotelEndpoints
{
    public static WebApplication UseHotelEndpoints(this WebApplication app)
    {
        app.MapGet("/hotels", GetAllHotels)
            .WithName(nameof(GetAllHotels));

        app.MapGet("/hotels/search", SearchHotels)
            .WithName(nameof(SearchHotels));

        app.MapGet("/hotels/{hotelId:Guid}/rooms/availability-search", SearchHotelRoomAvailability)
            .WithName(nameof(SearchHotelRoomAvailability));

        app.MapGet("/hotels/{hotelId:Guid}/bookings", GetHotelBookings)
            .WithName(nameof(GetHotelBookings));

        app.MapPost("/hotels/{hotelId:Guid}/bookings", CreateHotelBooking)
            .WithName(nameof(CreateHotelBooking));

        return app;
    }

    public static WebApplication UseRoomEndpoints(this WebApplication app)
    {
        app.MapGet("/rooms/availability-search", SearchRoomAvailability)
            .WithName(nameof(SearchRoomAvailability));

        return app;
    }

    public static WebApplication UseBookingEndpoints(this WebApplication app)
    {
        app.MapGet("/bookings/{id:Guid}", GetBooking)
            .WithName(nameof(GetBooking));

        return app;
    }

    private static async Task<Results<Ok<object>, BadRequest<object>>> GetAllHotels(
        [FromServices] IHotelGetAllQueryHandler handler,
        [FromServices] LinkGenerator linkGenerator,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var query = new HotelGetAllQuery();
        var result = await handler.Handle(query, cancellationToken);

        if (result.IsFailure)
        {
            var error = (ErrorResult<IEnumerable<HotelDto>>)result;
            return TypedResults.BadRequest<object>(new { error.Code, error.Message });
        }

        var success = (SuccessResult<IEnumerable<HotelDto>>)result;

        var hotel = success.Value;

        var hateoasModel = new HateoasModel<IEnumerable<HotelDto>>(hotel);

        return TypedResults.Ok<object>(hateoasModel);
    }

    private static async Task<Results<Ok<object>, BadRequest<object>>> SearchHotels(
        [FromQuery] string name,
        [FromServices] IHotelSearchQueryHandler handler,
        [FromServices] LinkGenerator linkGenerator,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var query = new HotelSearchQuery(name);
        var result = await handler.Handle(query, cancellationToken);

        if (result.IsFailure)
        {
            var error = (ErrorResult<IEnumerable<HotelDto>>)result;
            return TypedResults.BadRequest<object>(new { error.Code, error.Message });
        }

        var success = (SuccessResult<IEnumerable<HotelDto>>)result;
        var hateoasModel = new HateoasModel<IEnumerable<HotelDto>>(success.Value);

        foreach (var hotel in success.Value.Take(1))
        { 
            hateoasModel.AddLink(
                linkGenerator.GetUriByName(httpContext, "SearchHotelRoomAvailability", new { hotelId = hotel.Id })!,
                "search-rooms",
                "GET");

            hateoasModel.AddLink(
                linkGenerator.GetUriByName(httpContext, "GetHotelBookings", new { hotelId = hotel.Id })!,
                "bookings",
                "GET");

            hateoasModel.AddLink(
                linkGenerator.GetUriByName(httpContext, "CreateHotelBooking", new { hotelId = hotel.Id })!,
                "create-booking",
                "POST");
        }

        return TypedResults.Ok<object>(hateoasModel);
    }

    private static async Task<Results<Ok<object>, BadRequest<object>>> SearchHotelRoomAvailability(
        [FromRoute] Guid hotelId,
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        [FromQuery] int numberOfPeople,
        [FromServices] IFindHotelAvailabilityQueryHandler handler,
        [FromServices] LinkGenerator linkGenerator,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var query = new FindHotelAvailabilityQuery(hotelId, from, to, numberOfPeople);
        var result = await handler.Handle(query, cancellationToken);

        if (result.IsFailure)
        {
            var error = (ErrorResult<IEnumerable<RoomDto>>)result;
            return TypedResults.BadRequest<object>(new { error.Code, error.Message });
        }

        var success = (SuccessResult<IEnumerable<RoomDto>>)result;
        var hateoasModel = new HateoasModel<IEnumerable<RoomDto>>(success.Value);

        foreach (var hotel in success.Value.Take(1))
        { 
            hateoasModel.AddLink(
                linkGenerator.GetUriByName(httpContext, "CreateHotelBooking", new { hotelId = hotel.Id })!,
                "create-booking",
                "POST");
        }

        return TypedResults.Ok<object>(hateoasModel);
    }

    private static async Task<Results<Ok<object>, BadRequest<object>>> GetHotelBookings(
        [FromRoute] Guid hotelId,
        [FromServices] IGetHotelBookingsQueryHandler handler,
        [FromServices] LinkGenerator linkGenerator,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var query = new GetHotelBookingsQuery(hotelId);
        var result = await handler.Handle(query, cancellationToken);

        if (result.IsFailure)
        {
            var error = (ErrorResult<IEnumerable<BookingDto>>)result;
            return TypedResults.BadRequest<object>(new { error.Code, error.Message });
        }

        var success = (SuccessResult<IEnumerable<BookingDto>>)result;
        var hateoasModel = new HateoasModel<IEnumerable<BookingDto>>(success.Value);

        return TypedResults.Ok<object>(hateoasModel);
    }

    private static async Task<Results<Created<object>, BadRequest<object>>> CreateHotelBooking(
        [FromRoute] Guid hotelId,
        [FromBody] CreateBookingRequest request,
        [FromServices] ICreateBookingCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateBookingCommand(request.Name, request.RoomId, request.From, request.To);
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            var error = (ErrorResult<Guid>)result;
            return TypedResults.BadRequest<object>(new { error.Code, error.Message });
        }

        var success = (SuccessResult<Guid>)result;
        
        return TypedResults.Created($"/bookings/{success.Value}", (object)success.Value);
    }

    private static async Task<Results<Ok<object>, BadRequest<object>>> SearchRoomAvailability(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        [FromQuery] int numberOfPeople,
        [FromServices] IFindAvailabilityQueryHandler handler,
        [FromServices] LinkGenerator linkGenerator,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var query = new FindAvailabilityQuery(from, to, numberOfPeople);
        var result = await handler.Handle(query, cancellationToken);

        if (result.IsFailure)
        {
            var error = (ErrorResult<IEnumerable<RoomDto>>)result;
            return TypedResults.BadRequest<object>(new { error.Code, error.Message });
        }

        var success = (SuccessResult<IEnumerable<RoomDto>>)result;
        var hateoasModel = new HateoasModel<IEnumerable<RoomDto>>(success.Value);

        return TypedResults.Ok<object>(hateoasModel);
    }

    private static async Task<Results<Ok<object>, BadRequest<object>, NotFound<object>>> GetBooking(
        [FromRoute(Name = "id")] Guid id,
        [FromServices] IBookingGetQueryHandler handler,
        [FromServices] LinkGenerator linkGenerator,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var query = new BookingGetQuery(id);
        var result = await handler.Handle(query, cancellationToken);

        if (result.IsFailure)
        {
            var error = (ErrorResult<BookingDto>)result;

            if (error.Code == "BOOKING_NOT_FOUND")
            {
                return TypedResults.NotFound<object>(new { error.Code, error.Message });
            }

            return TypedResults.BadRequest<object>(new { error.Code, error.Message });
        }

        var success = (SuccessResult<BookingDto>)result;
       var hateoasModel = new HateoasModel<BookingDto>(success.Value);

        return TypedResults.Ok<object>(hateoasModel);
    }
}

public record CreateBookingRequest(string Name, Guid RoomId, DateTime From, DateTime To);
