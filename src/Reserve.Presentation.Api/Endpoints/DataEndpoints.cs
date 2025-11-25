using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Reserve.Application.CommandHandlers.Data;
using Reserve.Application.Commands.Data;
using Reserve.Application.Results;

namespace Reserve.Presentation.Api.Endpoints;

public static class DataEndpoints
{
    public static WebApplication UseDataEndpoints(this WebApplication app)
    {
        app.MapGet("/data/info", async (
            CancellationToken cancellationToken) => {  
                
            return TypedResults.Ok("Seeding is enabled.");
        });

        app.MapPost("/data/seed", async Task<Results<Ok, BadRequest<object>>> (
            [FromServices] IDatabaseSeedDataCommandHandler handler,
            CancellationToken cancellationToken) => {

            var command = new DatabaseSeedDataCommand();
            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
            {
                var error = (ErrorResult<bool>)result;
                return TypedResults.BadRequest<object>(new { error.Code, error.Message });
            }

            return TypedResults.Ok();
        });

        app.MapPost("/data/reset", async Task<Results<Ok, BadRequest<object>>> (
            [FromServices] IDatabaseResetDataCommandHandler handler,
            CancellationToken cancellationToken) => {

            var command = new DatabaseResetDataCommand();
            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
            {
                var error = (ErrorResult<bool>)result;
                return TypedResults.BadRequest<object>(new { error.Code, error.Message });
            }

            return TypedResults.Ok();
        }); 

        return app;
    }
}


