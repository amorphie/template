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

namespace amorphie.template.Module;

public class StudentModule : BaseBBTRoute<StudentDTO, Student, TemplateDbContext>
{
    public StudentModule(WebApplication app)
        : base(app) { }

    public override string[]? PropertyCheckList => new string[] { "FirstMidName", "LastName" };

    public override string? UrlFragment => "student";

    public override void AddRoutes(RouteGroupBuilder routeGroupBuilder)
    {
        base.AddRoutes(routeGroupBuilder);

        routeGroupBuilder.MapGet("/search", SearchMethod);
    }

    protected async  ValueTask<IResult> SearchMethod(
        [FromServices] TemplateDbContext context,
        [FromServices] IMapper mapper,
        [AsParameters] StudentSearch userSearch
    )
    {
        IList<Student> resultList = await context
            .Set<Student>()
            .AsNoTracking()
            .Where(x=>x.FirstMidName.Contains(userSearch.Keyword!) || x.LastName.Contains(userSearch.Keyword!))
            .Skip(userSearch.Page)
            .Take(userSearch.PageSize)
            .ToListAsync();

        return (resultList != null && resultList.Count > 0)
            ? Results.Ok(mapper.Map<IList<StudentDTO>>(resultList))
            : Results.NoContent();
    }
}
