using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using amorphie.template.core.Model;
using amorphie.template.Validator;
using amorphie.core.Module.minimal_api;
using amorphie.template.data;
using Microsoft.AspNetCore.Mvc;
using amorphie.template.core.Search;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using amorphie.core.Swagger;
using Microsoft.OpenApi.Models;
using amorphie.core.Identity;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Polly.Caching;
using amorphie.core.Extension;
using Elastic.Apm;

namespace amorphie.template.Module;


public sealed class StudentModule : BaseBBTRoute<StudentDTO, Student, TemplateDbContext>
{
    public StudentModule(WebApplication app)
        : base(app) { }

    public override string[]? PropertyCheckList => new string[] { "FirstMidName", "LastName" };

    public override string? UrlFragment => "student";

    public override void AddRoutes(RouteGroupBuilder routeGroupBuilder)
    {
        base.AddRoutes(routeGroupBuilder);

        routeGroupBuilder.MapGet("/search", SearchMethod);
        routeGroupBuilder.MapGet("/custom-method", CustomMethod);

        routeGroupBuilder.MapGet("/elastic-log", ElasticLogMethod);
        routeGroupBuilder.MapGet("/elastic-apm", ElasticApmMethod);
    }
    protected async Task<IResult> ElasticLogMethod([FromServices]ILogger<StudentModule> Log)
    {
        Log.LogInformation("Hello, world!");
        return Results.Ok();
    }
    protected async Task<IResult> ElasticApmMethod([FromServices] ILogger<StudentModule> Log)
    {
       // var transaction = Agent.Tracer.StartTransaction("MyTransactionName", "Custom");

        // Your code here
       // Log.LogInformation("This is a log message within the transaction.");
        //    transaction.Result = "Success"; // Set the result of the transaction

            // Any errors can be captured by calling transaction.CaptureException(exception)
   

        return Results.Ok();
    }
    protected override ValueTask<IResult> UpsertMethod([FromServices] IMapper mapper, [FromServices] FluentValidation.IValidator<Student> validator, [FromServices] TemplateDbContext context, [FromServices] IBBTIdentity bbtIdentity, [FromBody] StudentDTO data, HttpContext httpContext, CancellationToken token)
    {
        return base.UpsertMethod(mapper, validator, context, bbtIdentity, data, httpContext, token);
    }
    [AddSwaggerParameter("Test Required", ParameterLocation.Header, true)]
    protected async ValueTask<IResult> CustomMethod()
    {
        return Results.Ok();
    }

    protected async ValueTask<IResult> SearchMethod(
        [FromServices] TemplateDbContext context,
        [FromServices] IMapper mapper,
        [AsParameters] StudentSearch userSearch,
        HttpContext httpContext,
        CancellationToken token,
        [FromServices] DaprClient daprClient
    )
    {
        // await daprClient.CacheDelete("amorphie-state", "myCachedItem", "amorphie-lock");

        IList<Student> resultList = await daprClient.CacheReadWrite<IList<Student>>(async () =>
        {
            return await context
                            .Set<Student>()
                            .AsNoTracking()
                            .Where(
                                x =>
                                    x.FirstMidName.Contains(userSearch.Keyword!)
                                    || x.LastName.Contains(userSearch.Keyword!)
                            )
                            .Skip(userSearch.Page)
                            .Take(userSearch.PageSize)
                            .ToListAsync(token);
        }, "amorphie-state", "myCachedItem",30, "amorphie-lock");


        return (resultList != null && resultList.Count > 0)
            ? Results.Ok(mapper.Map<IList<StudentDTO>>(resultList))
            : Results.NoContent();
    }
}
